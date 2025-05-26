namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePalletsToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InvoiceItems", "Pallets", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InvoiceItems", "Pallets", c => c.Byte());
        }
    }
}
