namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountNumberToClient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "AccountNumber", c => c.String(nullable: false, maxLength: 75));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "AccountNumber");
        }
    }
}
