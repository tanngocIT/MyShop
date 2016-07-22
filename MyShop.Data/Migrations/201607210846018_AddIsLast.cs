namespace MyShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsLast : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductCategories", "IsLast", c => c.Boolean());
            AddColumn("dbo.PostCategories", "IsLast", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PostCategories", "IsLast");
            DropColumn("dbo.ProductCategories", "IsLast");
        }
    }
}
