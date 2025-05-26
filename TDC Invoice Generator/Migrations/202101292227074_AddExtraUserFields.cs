namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExtraUserFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 75));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 75));
            AddColumn("dbo.AspNetUsers", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "CreatedUser", c => c.String(maxLength: 75));
            AddColumn("dbo.AspNetUsers", "ModifiedDate", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "ModifiedUser", c => c.String());
            AddColumn("dbo.AspNetUsers", "DeletedDate", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "DeletedUser", c => c.String(maxLength: 75));
            AddColumn("dbo.AspNetUsers", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsDeleted");
            DropColumn("dbo.AspNetUsers", "DeletedUser");
            DropColumn("dbo.AspNetUsers", "DeletedDate");
            DropColumn("dbo.AspNetUsers", "ModifiedUser");
            DropColumn("dbo.AspNetUsers", "ModifiedDate");
            DropColumn("dbo.AspNetUsers", "CreatedUser");
            DropColumn("dbo.AspNetUsers", "CreatedDate");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
