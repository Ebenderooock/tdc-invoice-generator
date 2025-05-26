namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredToClientIdOnInvoices : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Invoices", "ClientId", "dbo.Clients");
            DropIndex("dbo.Invoices", new[] { "ClientId" });
            AlterColumn("dbo.Invoices", "ClientId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Invoices", "ClientId");
            AddForeignKey("dbo.Invoices", "ClientId", "dbo.Clients", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "ClientId", "dbo.Clients");
            DropIndex("dbo.Invoices", new[] { "ClientId" });
            AlterColumn("dbo.Invoices", "ClientId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Invoices", "ClientId");
            AddForeignKey("dbo.Invoices", "ClientId", "dbo.Clients", "Id");
        }
    }
}
