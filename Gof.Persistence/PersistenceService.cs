using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Gof.Persistence;
using Gof.Twitter;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Gof.Persistence.Service.Database;
using Gof.Persistence.Service.Models;
using System.Data.Entity;
using Microsoft.WindowsAzure.Storage;

namespace Gof.Persistence.Service
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class PersistenceService : StatelessService, IPersistence
    {

        public PersistenceService(StatelessServiceContext context)
            : base(context)
        {
            //InfluencerContextConfiguration.ServiceContext = context;
        }

        private string getKey(string key)
        {
            return Context.CodePackageActivationContext.GetConfigurationPackageObject("Config").Settings.Sections["Data"].Parameters[key].Value;
        }

        public async Task SaveTweetAsync(Tweet tweet)
        {
            try
            {
                using (InfluencerContext db = new InfluencerContext(getKey("ConnectionString")))
                {
                    ServiceEventSource.Current.ServiceMessage(this, "Saving Tweet: {1} {2} {0}", tweet.Content, tweet.Keyword, tweet.Author.Name);

                    db.Tweets.Add(new InfluencerTweet
                    {
                        Date = tweet.Date,
                        Influencer = tweet.Author.Alias,
                        TweetId = $"{tweet.Id}-tweet.Keyword",
                        TweetContent = tweet.Content,
                        Keyword = tweet.Keyword
                    });
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(this, "Failure: {0}", ex.Message);
                throw getException(ex);
            }
        }

        public async Task SaveReTweetAsync(Tweet tweet)
        {
            ServiceEventSource.Current.ServiceMessage(this, "Saving RT: {1} {2} {0}", tweet.Content, tweet.Keyword, tweet.Author.Name);
            try
            {
                using (InfluencerContext db = new InfluencerContext(getKey("ConnectionString")))
                {
                     db.InfluencerRTs.Add(new InfluencerRT
                    {
                        Author = tweet.Author.Alias,
                        Date = tweet.Date,
                        Influencer = tweet.TimeLineUser.Alias,
                        Keyword = tweet.Keyword,
                        TweetContent = tweet.Content,
                        TweetId = $"{tweet.Id}-{tweet.Keyword}"
                    });
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(this, "Failure: {0}", ex.Message);
                throw getException(ex);
            }
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] {
                new ServiceInstanceListener(context=>
                    this.CreateServiceRemotingListener(context),"rpcListener")
            };
        }

        public Task<IEnumerable<String>> RetrieveInfluencerListAsync()
        {

                return Task.Run(() =>
                {
                    using (InfluencerContext db = new InfluencerContext(getKey("ConnectionString")))
                    {
                        try
                        { return (IEnumerable<string>)db.Influencers.Select(i => i.ScreenName).ToList();
                        }
                        catch (Exception ex)
                        {
                            ServiceEventSource.Current.ServiceMessage(this, "Failure: {0}", ex.Message);
                            throw getException(ex);
                        }
                    }
                });
        }

        private Exception getException(Exception ex)
        {
            return new Exception($"{ex.GetType().Name} Source:{ex.Source} Message:{ex.Message}\r\n{ex.InnerException?.GetType().Name} {ex.InnerException?.Message}");
        }

        public Task<IEnumerable<String>> RetrieveKeywordListAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    using (InfluencerContext db = new InfluencerContext(getKey("ConnectionString")))
                    {
                        return (IEnumerable<string>)db.Keywords.Select(k => k.Name).ToList();
                    }
                }
                catch (Exception ex)
                {
                    ServiceEventSource.Current.ServiceMessage(this, "Failure: {0}", ex.Message);
                    throw getException(ex);
                }
            });
        }

        public async Task SaveRawTweetAsync(Tweet tweet)
        {
            try
            {
                var account = CloudStorageAccount.Parse(getKey("StorageConnectionString"));
                Microsoft.WindowsAzure.Storage.Table.CloudTableClient c = new Microsoft.WindowsAzure.Storage.Table.CloudTableClient(account.TableStorageUri, account.Credentials);
                var table = c.GetTableReference("tweets");
                var created = await table.CreateIfNotExistsAsync();
                var op = Microsoft.WindowsAzure.Storage.Table.TableOperation.Insert(new TweetEntity(tweet));
                var result = await table.ExecuteAsync(op);
            }
            catch (Exception ex)
            {
                throw new Exception($"SaveRawTweet: {ex.Source} {ex.Message} {ex.InnerException?.Message}");
            }
        }

        ///// <summary>
        ///// This is the main entry point for your service instance.
        ///// </summary>
        ///// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        //protected override async Task RunAsync(CancellationToken cancellationToken)
        //{
        //    // TODO: Replace the following sample code with your own logic 
        //    //       or remove this RunAsync override if it's not needed in your service.

        //    long iterations = 0;

        //    while (true)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();

        //        ServiceEventSource.Current.ServiceMessage(this, "Working-{0}", ++iterations);

        //        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        //    }
        //}
    }
}