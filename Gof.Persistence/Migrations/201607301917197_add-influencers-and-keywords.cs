namespace Gof.Persistence.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addinfluencersandkeywords : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Influencers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ScreenName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Keywords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Keywords");
            DropTable("dbo.Influencers");
        }
    }
}
