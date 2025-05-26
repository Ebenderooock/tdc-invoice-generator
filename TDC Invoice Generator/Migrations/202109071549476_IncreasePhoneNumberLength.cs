namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreasePhoneNumberLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "ContactPersonPhoneNumber", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "ContactPersonPhoneNumber", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
