namespace BookManagerBL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Step1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "Location_LocationId", "dbo.Locations");
            DropForeignKey("dbo.Books", "Publisher_PublisherId", "dbo.Publishers");
            DropIndex("dbo.Books", new[] { "Location_LocationId" });
            DropIndex("dbo.Books", new[] { "Publisher_PublisherId" });
            RenameColumn(table: "dbo.Books", name: "Location_LocationId", newName: "LocationId");
            RenameColumn(table: "dbo.Books", name: "Publisher_PublisherId", newName: "PublisherId");
            AlterColumn("dbo.Books", "LocationId", c => c.Int(nullable: false));
            AlterColumn("dbo.Books", "PublisherId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "PublisherId");
            CreateIndex("dbo.Books", "LocationId");
            AddForeignKey("dbo.Books", "LocationId", "dbo.Locations", "LocationId", cascadeDelete: true);
            AddForeignKey("dbo.Books", "PublisherId", "dbo.Publishers", "PublisherId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "PublisherId", "dbo.Publishers");
            DropForeignKey("dbo.Books", "LocationId", "dbo.Locations");
            DropIndex("dbo.Books", new[] { "LocationId" });
            DropIndex("dbo.Books", new[] { "PublisherId" });
            AlterColumn("dbo.Books", "PublisherId", c => c.Int());
            AlterColumn("dbo.Books", "LocationId", c => c.Int());
            RenameColumn(table: "dbo.Books", name: "PublisherId", newName: "Publisher_PublisherId");
            RenameColumn(table: "dbo.Books", name: "LocationId", newName: "Location_LocationId");
            CreateIndex("dbo.Books", "Publisher_PublisherId");
            CreateIndex("dbo.Books", "Location_LocationId");
            AddForeignKey("dbo.Books", "Publisher_PublisherId", "dbo.Publishers", "PublisherId");
            AddForeignKey("dbo.Books", "Location_LocationId", "dbo.Locations", "LocationId");
        }
    }
}
