namespace Gof.Persistence.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uniqueindexscreename : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Influencers", "ScreenName", c => c.String(maxLength: 450));
            CreateIndex("dbo.Influencers", "ScreenName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Influencers", new[] { "ScreenName" });
            AlterColumn("dbo.Influencers", "ScreenName", c => c.String());
        }
    }
}
