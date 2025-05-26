namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreaseAddressLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "ContactPersonName", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "ContactPersonName", c => c.String(nullable: false, maxLength: 75));
        }
    }
}
