using Gof.Persistence.Service.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace Gof.Persistence.Service.Database
{
    public class InfluencerContext : DbContext
    {
        public InfluencerContext(string connectionString) : base(connectionString) { }
        //public InfluencerContext(): base("AzureSQLdb") { }

        public DbSet<InfluencerTweet> Tweets { get; set; }
        public DbSet<InfluencerRT> InfluencerRTs { get; set; }
        public DbSet<Influencer> Influencers { get; set; }
        public DbSet<Keyword> Keywords { get; set; }

    }

    public class InfluencerContextConfiguration : DbConfiguration
    {
        //public static System.Fabric.StatelessServiceContext ServiceContext { get; set; }
        public InfluencerContextConfiguration()
        {
            SetExecutionStrategy(
                "System.Data.SqlClient",
                () => new SqlAzureExecutionStrategy(1, TimeSpan.FromSeconds(30)));
            SetDatabaseInitializer(
    new MigrateDatabaseToLatestVersion<InfluencerContext, Migrations.Configuration>(true));

            //SetContextFactory(() =>
            //{
            //    return new InfluencerContext(getKey("ConnectionString"));
            //});

        }
        //private string getKey(string key)
        //{
        //    return ServiceContext.CodePackageActivationContext.GetConfigurationPackageObject("Config").Settings.Sections["Data"].Parameters[key].Value;
        //}
    }
}