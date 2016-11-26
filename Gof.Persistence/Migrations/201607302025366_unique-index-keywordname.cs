namespace Gof.Persistence.Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uniqueindexkeywordname : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Keywords", "Name", c => c.String(maxLength: 450));
            CreateIndex("dbo.Keywords", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Keywords", new[] { "Name" });
            AlterColumn("dbo.Keywords", "Name", c => c.String());
        }
    }
}
