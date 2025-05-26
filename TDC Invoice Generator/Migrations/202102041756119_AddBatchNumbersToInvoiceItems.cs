namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBatchNumbersToInvoiceItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceItems", "BatchNumbers", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceItems", "BatchNumbers");
        }
    }
}
