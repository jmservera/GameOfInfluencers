namespace Gof.Persistence.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InfluencerRTs",
                c => new
                {
                    TweetId = c.String(nullable: false, maxLength: 128),
                    Author = c.String(),
                    Influencer = c.String(),
                    TweetContent = c.String(),
                    Date = c.DateTime(nullable: false),
                    Keyword = c.String(),
                })
                .PrimaryKey(t => t.TweetId);

            CreateTable(
                "dbo.InfluencerTweets",
                c => new
                {
                    TweetId = c.String(nullable: false, maxLength: 128),
                    Influencer = c.String(),
                    TweetContent = c.String(),
                    Date = c.DateTime(nullable: false),
                    Keyword = c.String(),
                    RT_Count = c.Int(nullable: false),
                    Fav_Count = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.TweetId);

        }
        
        public override void Down()
        {
            DropTable("dbo.InfluencerTweets");
            DropTable("dbo.InfluencerRTs");
        }
    }
}
