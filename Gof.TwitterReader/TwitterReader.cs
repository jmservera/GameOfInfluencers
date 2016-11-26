using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Tweetinvi;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Tweetinvi.Events;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Fabric.Health;
using Gof.Persistence;
using Gof;

namespace TwitterReader
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class TwitterReader : StatefulService
    {
        public TwitterReader(StatefulServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see http://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new ServiceReplicaListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            var influencers = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, DateTime>>("influencers");
            var influencerList = await ServiceExtensions.GetPersistenceService(DateTime.Now.ToString()).RetrieveInfluencerListAsync();

            //bug
            if (influencerList.Count() == 0)
            {
                var healthInformation= new HealthInformation(
                    "TwitterReader",
                     "influencers should not be 0", HealthState.Error);
                this.Partition.ReportReplicaHealth(healthInformation);
            }

            readNextInfluencers(cancellationToken, influencerList.ToArray());

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string influencer = null;

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var list = await influencers.CreateEnumerableAsync(tx, EnumerationMode.Ordered);
                    using (var enumerator = list.GetAsyncEnumerator())
                    {
                        while (await enumerator.MoveNextAsync(cancellationToken))
                        {
                            if (enumerator.Current.Value == DateTime.MinValue || enumerator.Current.Value < DateTime.Now.AddMinutes(-20))
                            {
                                if (await influencers.TryUpdateAsync(tx, enumerator.Current.Key, DateTime.Now, enumerator.Current.Value))
                                {
                                    influencer = enumerator.Current.Key;


                                    break;
                                }
                            }
                        }
                    }
                    if (influencer != null)
                    {
                        ServiceEventSource.Current.ServiceMessage(this, "Adding reader for influencer: {0}", influencer);
                        readNextInfluencers(cancellationToken, influencerList.ToArray());
                    }
                    else
                    {
                        //ServiceEventSource.Current.ServiceMessage(this, "No more influencers to read");
                    }
                    //var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    //ServiceEventSource.Current.ServiceMessage(this, "Current Counter Value: {0}",
                    //    result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    //await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    //// If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                    //// discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        private async void readNextInfluencers(CancellationToken cancellationToken, string[] influencers)
        {
            try
            {
                //var influencers = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, DateTime>>("influencers");
                Auth.SetUserCredentials(
                    getKey("ApiKey"),
                    getKey("ApiSecret"),
                    getKey("TokenKey"),
                    getKey("TokenSecret"));

                var keywords = await ServiceExtensions.GetPersistenceService(DateTime.Now.ToString()).RetrieveKeywordListAsync();

                SemaphoreSlim semaphore = new SemaphoreSlim(1);
                var twitStream = Stream.CreateFilteredStream();
                try
                {
                    foreach (var influencer in influencers)
                    {
                        twitStream.AddFollow(User.GetUserFromScreenName(influencer));
                    }
                    twitStream.MatchingTweetReceived += async (o, e) =>
                    {
                        List<Task> taskList = new List<Task>();
                        taskList.Add(saveTweet(e));
                        var uppertweet = e.Tweet.Text.ToUpper();
                        foreach (var word in keywords)
                        {
                            if (uppertweet.Contains(word.ToUpper()))
                            {
                                taskList.Add(saveTweet(e, word));
                            }
                        }
                        foreach (var hashtag in e.Tweet.Hashtags.Distinct())
                        {
                            if (keywords.FirstOrDefault((s) => s.ToUpper() == hashtag.Text.ToUpper()) == null)
                            {
                                taskList.Add(saveTweet(e, hashtag.Text));
                            }
                        }
                        await Task.WhenAll(taskList);
                        ServiceEventSource.Current.ServiceMessage(this, "Current Tweet: {0}", e.Tweet.Text);
                    };

                    await semaphore.WaitAsync();
                    twitStream.LimitReached += (o, e) => { semaphore.Release(); };
                    twitStream.DisconnectMessageReceived += (o, e) => { semaphore.Release(); };
                    await twitStream.StartStreamMatchingAllConditionsAsync();
                    await semaphore.WaitAsync(cancellationToken);
                }
                finally
                {
                    twitStream.StopStream();
                    ServiceEventSource.Current.ServiceMessage(this, "Influencer stream stopped: {0}", influencers);
                }
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(this, "Fatal error with twitter API: {0}", ex.Message);

                HealthInformation healthInformation = new HealthInformation("TwitterAPI", ex.Message, HealthState.Error);
                this.Partition.ReportReplicaHealth(healthInformation);
            }
        }

        private Task saveTweet(MatchedTweetReceivedEventArgs e)
        {
            Gof.Twitter.Tweet tweet = formatTweet(e);
            IPersistence persistence = ServiceExtensions.GetPersistenceService(e.Tweet.CreatedBy.Name);
            return persistence.SaveRawTweetAsync(tweet);
        }

        private string getKey(string key)
        {
            return Context.CodePackageActivationContext.GetConfigurationPackageObject("Config").Settings.Sections["Keys"].Parameters[key].Value;
        }

        private async Task saveTweet(MatchedTweetReceivedEventArgs e, string keyword)
        {
            var tweet = formatTweet(e);
            tweet.Keyword = keyword;

            //we use keyword hashcode as partition key, it is assumed to be sparse
            IPersistence persistence = ServiceExtensions.GetPersistenceService(keyword);

            try
            {
                if (e.Tweet.IsRetweet)
                {
                    await persistence.SaveReTweetAsync(tweet);
                }
                else
                {
                    await persistence.SaveTweetAsync(tweet);
                }
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(this, "Tweet store failed: {0}\r\n\t{1}", e.Tweet.Text, ex.Message);
                throw;
            }
        }

        private static Gof.Twitter.Tweet formatTweet(MatchedTweetReceivedEventArgs e)
        {
            var author = e.Tweet.RetweetedTweet != null ? e.Tweet.RetweetedTweet.CreatedBy : e.Tweet.CreatedBy;
            var user = e.Tweet.CreatedBy;

            var tweet = new Gof.Twitter.Tweet
            {
                Id = e.Tweet.Id,
                TimeLineUser = new Gof.Twitter.User { Alias = user.ScreenName, Name = user.Name, Id = user.Id },
                Author = new Gof.Twitter.User { Alias = author.ScreenName, Name = author.Name, Id = author.Id },
                Content = e.Tweet.Text,
                Date = e.Tweet.TweetLocalCreationDate,
                IsRetweet = e.Tweet.IsRetweet,
                Language = e.Tweet.Language.ToString(),
                Hashtags = e.Tweet.Hashtags.Select(s => new Tuple<int[], string>(s.Indices, s.Text)),
                Raw = Newtonsoft.Json.JsonConvert.SerializeObject(e.Tweet),
                Url = e.Tweet.Url
            };
            return tweet;
        }
    }
}