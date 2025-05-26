namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvoicesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ClientId = c.String(maxLength: 128),
                        InvoiceDate = c.DateTime(nullable: false),
                        PoNumber = c.String(nullable: false, maxLength: 75),
                        GeneralWaybillNumber = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUser = c.String(nullable: false, maxLength: 75),
                        ModifiedDate = c.DateTime(),
                        ModifiedUser = c.String(maxLength: 75),
                        DeletedDate = c.DateTime(),
                        DeletedUser = c.String(maxLength: 75),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .Index(t => t.ClientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "ClientId", "dbo.Clients");
            DropIndex("dbo.Invoices", new[] { "ClientId" });
            DropTable("dbo.Invoices");
        }
    }
}
