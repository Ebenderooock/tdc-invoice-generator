namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCodeToProducts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Code", c => c.String(nullable: false, maxLength: 75));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Code");
        }
    }
}
