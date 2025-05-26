namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedRoles : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO AspNetRoles (Id, Name) VALUES ('de0d5968-b46d-499a-b90a-7f56c6ff59d4', 'Admin')");
            Sql("INSERT INTO AspNetRoles (Id, Name) VALUES ('47dcf9a6-d573-444a-9b84-728d328b6de8', 'Invoicing')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM AspNetRoles WHERE Id = 'de0d5968-b46d-499a-b90a-7f56c6ff59d4'");
            Sql("DELETE FROM AspNetRoles WHERE Id = '47dcf9a6-d573-444a-9b84-728d328b6de8'");
        }
    }
}
