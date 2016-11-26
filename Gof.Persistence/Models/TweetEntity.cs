using Gof.Twitter;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;

namespace Gof.Persistence.Service.Models
{
    public class TweetEntity : TableEntity
    {
        public TweetEntity()
        {

        }
        public TweetEntity(Tweet tweet) : base(tweet.Author?.Alias, tweet.Id.ToString())
        {
            Content = tweet.Content;
            Date = tweet.Date;
            IsRetweet = tweet.IsRetweet;
            Language = tweet.Language;
            Raw = tweet.Raw;
            TimeLineUser= tweet.TimeLineUser.Alias;
            Url = tweet.Url;
        }
        public TweetEntity(string author, long id) : base(author, id.ToString())
        {

        }

        public string Author { get { return PartitionKey; } }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Id { get { return RowKey; } }
        public bool IsRetweet { get; set; }
        public string Language { get; private set; }
        public string Raw { get; set; }
        public string TimeLineUser { get; set; }
        public string Url { get; set; }
    }
}