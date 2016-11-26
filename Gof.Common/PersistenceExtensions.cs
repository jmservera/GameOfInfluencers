using Gof.Persistence;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gof
{
    public static class ServiceExtensions
    {
        public static IPersistence GetPersistenceService()
        {
            //use pseudo random distribution
            return GetPersistenceService(DateTime.Now.ToString());
        }
        public static IPersistence GetPersistenceService(string partitionKey)
        {
            return ServiceProxy.Create<IPersistence>(new Uri("fabric:/GameOfInfluencers/PersistenceService"),
                            partitionKey: new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(partitionKey.GetHashCode())
                            );
        }
    }
}
