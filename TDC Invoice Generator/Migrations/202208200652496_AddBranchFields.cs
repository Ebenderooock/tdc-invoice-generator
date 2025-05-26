namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBranchFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Branches", "ContactPerson", c => c.String(maxLength: 100));
            AddColumn("dbo.Branches", "ContactNumber", c => c.String(maxLength: 20));
            AddColumn("dbo.Branches", "Address", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {

        }
    }
}
