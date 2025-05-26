namespace InvoiceGenerator_Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBranchSelectionToClientsAndProducts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientBranches",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ClientId = c.String(nullable: false, maxLength: 128),
                        BranchId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.ProductBranches",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductId = c.String(nullable: false, maxLength: 128),
                        BranchId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.BranchId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductBranches", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductBranches", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.ClientBranches", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientBranches", "BranchId", "dbo.Branches");
            DropIndex("dbo.ProductBranches", new[] { "BranchId" });
            DropIndex("dbo.ProductBranches", new[] { "ProductId" });
            DropIndex("dbo.ClientBranches", new[] { "BranchId" });
            DropIndex("dbo.ClientBranches", new[] { "ClientId" });
            DropTable("dbo.ProductBranches");
            DropTable("dbo.ClientBranches");
        }
    }
}
