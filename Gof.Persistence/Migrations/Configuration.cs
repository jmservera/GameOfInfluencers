namespace Gof.Persistence.Service.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Gof.Persistence.Service.Database.InfluencerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Gof.Persistence.Service.Database.InfluencerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //only seed the initial Influencer List if it is empty
            if (context.Influencers.Count() == 0)
            {
                context.Influencers.AddOrUpdate(
                    p => p.ScreenName,
                    new Influencer { ScreenName = "Azure" },
                    new Influencer
                    {
                        ScreenName = "Googlecloud"
                    }, new Influencer
                    {
                        ScreenName = "AwsCloud"
                    }, new Influencer
                    {
                        ScreenName = "RedHatCloud"
                    }, new Influencer
                    {
                        ScreenName = "ibmcloud"
                    }
                    );
                context.Keywords.AddOrUpdate(
                    p => p.Name,
                    new Keyword { Name = "Azure" },
                new Keyword { Name = "Microsoft" },
                new Keyword { Name = "Cloud Computing" },
                new Keyword { Name = "Devops" },
                new Keyword { Name = "nodeJs" },
                new Keyword { Name = "Big Data" },
                new Keyword { Name = "IOT" },
                new Keyword { Name = "Mobile Development" },
                new Keyword { Name = "Software Development" },
                new Keyword { Name = "Information Technology" },
                new Keyword { Name = "SaaS (Software as a Service)" },
                new Keyword { Name = "System Administartion" },
                new Keyword { Name = "javasript" },
                new Keyword { Name = "Project Spark" },
                new Keyword { Name = "Internet of Things" },
                new Keyword { Name = "IOS Development" },
                new Keyword { Name = "Visual Studio" },
                new Keyword { Name = "IT Professionals" },
                new Keyword { Name = "IaaS (Infrastructure as a Service)" },
                new Keyword { Name = "Configuration Management" },
                new Keyword { Name = "HTML5" },
                new Keyword { Name = "Data Analytics" },
                new Keyword { Name = "Smart Sensors" },
                new Keyword { Name = "Android Development" },
                new Keyword { Name = ".Net" },
                new Keyword { Name = "New Technology" },
                new Keyword { Name = "PaaS (Platform as a Service)" },
                new Keyword { Name = "Continuous Delivery" },
                new Keyword { Name = "AngularJS" },
                new Keyword { Name = "NoSql" },
                new Keyword { Name = "Arduino" },
                new Keyword { Name = "Mobile Technology" },
                new Keyword { Name = "ASP.NET" },
                new Keyword { Name = "Information Technology Leadership" },
                new Keyword { Name = "AWS" },
                new Keyword { Name = "Virtualization" },
                new Keyword { Name = "Jquery" },
                new Keyword { Name = "mongoDB" },
                new Keyword { Name = "Robotics" },
                new Keyword { Name = "Game Development" },
                new Keyword { Name = "computer programming" },
                new Keyword { Name = "Technology Professionals" },
                new Keyword { Name = "Amazon Web Services" },
                new Keyword { Name = "ruby" },
                new Keyword { Name = "Machine Learning" },
                new Keyword { Name = "programming languages" },
                new Keyword { Name = "Software engineering" },
                new Keyword { Name = "php" },
                new Keyword { Name = "Hadoop" },
                new Keyword { Name = "Microsoft Azure" },
                new Keyword { Name = "python" },
                new Keyword { Name = "MapReduce" },
                new Keyword { Name = "docker" },
                new Keyword { Name = "Ruby on Rails" },
                new Keyword { Name = "Java" },
                new Keyword { Name = "Web Dev" },
                new Keyword { Name = "Web Development" },
                new Keyword { Name = "Google Cloud" }
                    );
            }
        }
    }
}
