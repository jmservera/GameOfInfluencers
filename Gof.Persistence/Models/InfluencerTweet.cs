using System;
using System.ComponentModel.DataAnnotations;

namespace Gof.Persistence.Service.Models
{
    public class InfluencerTweet
    {
        [Key]
        public string TweetId { get; set; }
        public string Influencer { get; set; }
        public string TweetContent { get; set; }
        public DateTime Date { get; set; }
        public string Keyword { get; set; }
        public int RT_Count { get; set; }
        public int Fav_Count { get; set; }
    }
}
