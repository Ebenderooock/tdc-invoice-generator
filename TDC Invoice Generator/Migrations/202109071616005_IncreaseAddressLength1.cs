namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreaseAddressLength1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "Address", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Clients", "ContactPersonName", c => c.String(nullable: false, maxLength: 75));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "ContactPersonName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Clients", "Address", c => c.String(nullable: false, maxLength: 75));
        }
    }
}
