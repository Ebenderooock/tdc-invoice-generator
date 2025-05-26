namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRequiredFieldsFromInvoiceItems : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InvoiceItems", "UnitSize", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InvoiceItems", "BatchNumbers", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InvoiceItems", "BatchNumbers", c => c.String(nullable: false));
            AlterColumn("dbo.InvoiceItems", "UnitSize", c => c.Short(nullable: false));
        }
    }
}
