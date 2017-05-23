namespace ODataDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Customer = c.String(),
                        Product_ProductId = c.Int(),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Products", t => t.Product_ProductId)
                .Index(t => t.Product_ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ProductCategory_CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Categories", t => t.ProductCategory_CategoryId)
                .Index(t => t.ProductCategory_CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ProductCategory_CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Orders", "Product_ProductId", "dbo.Products");
            DropIndex("dbo.Products", new[] { "ProductCategory_CategoryId" });
            DropIndex("dbo.Orders", new[] { "Product_ProductId" });
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
            DropTable("dbo.Categories");
        }
    }
}
