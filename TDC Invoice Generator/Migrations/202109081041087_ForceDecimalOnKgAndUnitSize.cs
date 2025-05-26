namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForceDecimalOnKgAndUnitSize : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InvoiceItems", "UnitSize", c => c.Decimal(precision: 18, scale: 3));
            AlterColumn("dbo.InvoiceItems", "TotalKg", c => c.Decimal(precision: 18, scale: 3));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InvoiceItems", "TotalKg", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.InvoiceItems", "UnitSize", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
