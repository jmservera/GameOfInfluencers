using Gof.Twitter;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gof.Persistence
{
    public interface IPersistence:IService
    {
        Task SaveTweetAsync(Tweet tweet);
        Task SaveReTweetAsync(Tweet tweet);

        Task<IEnumerable<String>> RetrieveInfluencerListAsync();
        Task<IEnumerable<String>> RetrieveKeywordListAsync();
        Task SaveRawTweetAsync(Tweet tweet);
    }
}
