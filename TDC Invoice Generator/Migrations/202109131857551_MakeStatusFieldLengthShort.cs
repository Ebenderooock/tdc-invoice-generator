namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeStatusFieldLengthShort : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Invoices", "Status", c => c.String(nullable: false, maxLength: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Invoices", "Status", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
