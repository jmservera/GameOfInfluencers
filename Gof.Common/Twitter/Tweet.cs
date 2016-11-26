using System;
using System.Collections.Generic;

namespace Gof.Twitter
{
    public class Tweet
    {
        public long Id { get; set; }
        public User Author { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Keyword { get; set; }
        public User TimeLineUser { get; set; }
        public bool IsRetweet { get; set; }
        public string Language { get; set; }
        public IEnumerable<Tuple<int[], string>> Hashtags { get; set; }
        public string Raw { get; set; }
        public string Url { get; set; }
    }
}