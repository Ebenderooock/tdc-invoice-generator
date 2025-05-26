namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBranchCodeToInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "BranchCode", c => c.String(nullable: false, maxLength: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoices", "BranchCode");
        }
    }
}
