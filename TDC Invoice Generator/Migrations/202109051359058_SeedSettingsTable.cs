namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedSettingsTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Settings (Id, LastWaybillNumber, CreatedDate, CreatedUser) VALUES ('4d89ab77-8a08-45d8-b5f5-ab08abca1cf6', 0, GETDATE(), 'System Seed')");
        }

        public override void Down()
        {
            Sql("DELETE FROM Settings");
        }
    }
}
