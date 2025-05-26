namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderToInvoiceItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceItems", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceItems", "Order");
        }
    }
}
