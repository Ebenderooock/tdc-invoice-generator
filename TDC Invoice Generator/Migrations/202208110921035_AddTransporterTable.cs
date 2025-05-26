namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransporterTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transporters",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 75),
                        ContactPerson = c.String(nullable: false, maxLength: 100),
                        ContactNumber = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Invoices", "TransporterId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Invoices", "TransporterId");
            AddForeignKey("dbo.Invoices", "TransporterId", "dbo.Transporters", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "TransporterId", "dbo.Transporters");
            DropIndex("dbo.Invoices", new[] { "TransporterId" });
            DropColumn("dbo.Invoices", "TransporterId");
            DropTable("dbo.Transporters");
        }
    }
}
