namespace PrintShopServiceImplementDataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerFIO = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Indents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        PrintId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateImplement = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Prints", t => t.PrintId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.PrintId);
            
            CreateTable(
                "dbo.Prints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PrintName = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PrintIngredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PrintId = c.Int(nullable: false),
                        IngredientId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ingredients", t => t.IngredientId, cascadeDelete: true)
                .ForeignKey("dbo.Prints", t => t.PrintId, cascadeDelete: true)
                .Index(t => t.PrintId)
                .Index(t => t.IngredientId);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IngredientName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StockIngredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StockId = c.Int(nullable: false),
                        IngredientId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ingredients", t => t.IngredientId, cascadeDelete: true)
                .ForeignKey("dbo.Stocks", t => t.StockId, cascadeDelete: true)
                .Index(t => t.StockId)
                .Index(t => t.IngredientId);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StockName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrintIngredients", "PrintId", "dbo.Prints");
            DropForeignKey("dbo.StockIngredients", "StockId", "dbo.Stocks");
            DropForeignKey("dbo.StockIngredients", "IngredientId", "dbo.Ingredients");
            DropForeignKey("dbo.PrintIngredients", "IngredientId", "dbo.Ingredients");
            DropForeignKey("dbo.Indents", "PrintId", "dbo.Prints");
            DropForeignKey("dbo.Indents", "CustomerId", "dbo.Customers");
            DropIndex("dbo.StockIngredients", new[] { "IngredientId" });
            DropIndex("dbo.StockIngredients", new[] { "StockId" });
            DropIndex("dbo.PrintIngredients", new[] { "IngredientId" });
            DropIndex("dbo.PrintIngredients", new[] { "PrintId" });
            DropIndex("dbo.Indents", new[] { "PrintId" });
            DropIndex("dbo.Indents", new[] { "CustomerId" });
            DropTable("dbo.Stocks");
            DropTable("dbo.StockIngredients");
            DropTable("dbo.Ingredients");
            DropTable("dbo.PrintIngredients");
            DropTable("dbo.Prints");
            DropTable("dbo.Indents");
            DropTable("dbo.Customers");
        }
    }
}
