namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRequiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InvoiceItems", "Quantity", c => c.Short());
            AlterColumn("dbo.InvoiceItems", "UnitSize", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.InvoiceItems", "TotalKg", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.InvoiceItems", "Pallets", c => c.Byte());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InvoiceItems", "Pallets", c => c.Byte(nullable: false));
            AlterColumn("dbo.InvoiceItems", "TotalKg", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InvoiceItems", "UnitSize", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InvoiceItems", "Quantity", c => c.Short(nullable: false));
        }
    }
}
