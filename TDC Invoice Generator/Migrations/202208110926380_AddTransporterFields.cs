namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransporterFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transporters", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Transporters", "CreatedUser", c => c.String(nullable: false, maxLength: 75));
            AddColumn("dbo.Transporters", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.Transporters", "ModifiedUser", c => c.String(maxLength: 75));
            AddColumn("dbo.Transporters", "DeletedDate", c => c.DateTime());
            AddColumn("dbo.Transporters", "DeletedUser", c => c.String(maxLength: 75));
            AddColumn("dbo.Transporters", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transporters", "IsDeleted");
            DropColumn("dbo.Transporters", "DeletedUser");
            DropColumn("dbo.Transporters", "DeletedDate");
            DropColumn("dbo.Transporters", "ModifiedUser");
            DropColumn("dbo.Transporters", "ModifiedDate");
            DropColumn("dbo.Transporters", "CreatedUser");
            DropColumn("dbo.Transporters", "CreatedDate");
        }
    }
}
