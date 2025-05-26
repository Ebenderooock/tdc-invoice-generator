namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeGeneralWaybillNumberString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Invoices", "GeneralWaybillNumber", c => c.String(nullable: false, maxLength: 75));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Invoices", "GeneralWaybillNumber", c => c.Int(nullable: false));
        }
    }
}
