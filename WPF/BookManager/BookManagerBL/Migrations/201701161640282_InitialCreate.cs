namespace BookManagerBL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.AuthorId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        PublishedYear = c.Int(nullable: false),
                        Location_LocationId = c.Int(),
                        Publisher_PublisherId = c.Int(),
                        Author_AuthorId = c.Int(),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId)
                .ForeignKey("dbo.Publishers", t => t.Publisher_PublisherId)
                .ForeignKey("dbo.Authors", t => t.Author_AuthorId)
                .Index(t => t.Location_LocationId)
                .Index(t => t.Publisher_PublisherId)
                .Index(t => t.Author_AuthorId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        QuickName = c.String(),
                        Room = c.String(),
                        Shelf = c.String(),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.Publishers",
                c => new
                    {
                        PublisherId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.PublisherId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "Author_AuthorId", "dbo.Authors");
            DropForeignKey("dbo.Books", "Publisher_PublisherId", "dbo.Publishers");
            DropForeignKey("dbo.Books", "Location_LocationId", "dbo.Locations");
            DropIndex("dbo.Books", new[] { "Author_AuthorId" });
            DropIndex("dbo.Books", new[] { "Publisher_PublisherId" });
            DropIndex("dbo.Books", new[] { "Location_LocationId" });
            DropTable("dbo.Publishers");
            DropTable("dbo.Locations");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
