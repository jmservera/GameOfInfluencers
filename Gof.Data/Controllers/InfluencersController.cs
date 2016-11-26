using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Gof.Data.Controllers
{
    [ServiceRequestActionFilter]
    public class InfluencersController : ApiController
    {
        // GET api/values 
        public async Task<IEnumerable<string>> Get()
        {
            return await ServiceExtensions.GetPersistenceService().RetrieveInfluencerListAsync();
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}
