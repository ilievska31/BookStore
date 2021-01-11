namespace BookStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_book_model : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Count", c => c.Int(nullable: false));
            AddColumn("dbo.Books", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "ImageUrl");
            DropColumn("dbo.Books", "Count");
        }
    }
}
