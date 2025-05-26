namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 75),
                        Address = c.String(nullable: false, maxLength: 75),
                        ContactPersonName = c.String(nullable: false, maxLength: 75),
                        ContactPersonPhoneNumber = c.String(nullable: false, maxLength: 10),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUser = c.String(nullable: false, maxLength: 75),
                        ModifiedDate = c.DateTime(),
                        ModifiedUser = c.String(maxLength: 75),
                        DeletedDate = c.DateTime(),
                        DeletedUser = c.String(maxLength: 75),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Clients");
        }
    }
}
