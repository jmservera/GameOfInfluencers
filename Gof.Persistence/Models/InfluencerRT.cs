using System;
using System.ComponentModel.DataAnnotations;

namespace Gof.Persistence.Service.Models
{
    public class InfluencerRT
    {
        [Key]
        public string TweetId { get; set; }
        public string Author { get; set; }
        public string Influencer { get; set; }
        public string TweetContent { get; set; }
        public DateTime Date { get; set; }
        public string Keyword { get; set; }
    }
}
