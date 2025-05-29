namespace InvoiceGenerator.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDriverAndVehicleRegistration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Code = c.String(nullable: false, maxLength: 75),
                        Name = c.String(nullable: false, maxLength: 75),
                        ContactPerson = c.String(maxLength: 100),
                        ContactNumber = c.String(maxLength: 20),
                        Address = c.String(maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUser = c.String(nullable: false, maxLength: 75),
                        ModifiedDate = c.DateTime(),
                        ModifiedUser = c.String(maxLength: 75),
                        DeletedDate = c.DateTime(),
                        DeletedUser = c.String(maxLength: 75),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Code = c.String(nullable: false, maxLength: 75),
                        Name = c.String(nullable: false, maxLength: 75),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUser = c.String(nullable: false, maxLength: 75),
                        ModifiedDate = c.DateTime(),
                        ModifiedUser = c.String(maxLength: 75),
                        DeletedDate = c.DateTime(),
                        DeletedUser = c.String(maxLength: 75),
                        IsDeleted = c.Boolean(nullable: false),
                        BranchId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.ProductBranches",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProductId = c.String(nullable: false, maxLength: 128),
                        BranchId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.ClientBranches",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ClientId = c.String(nullable: false, maxLength: 128),
                        BranchId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AccountNumber = c.String(nullable: false, maxLength: 75),
                        Name = c.String(nullable: false, maxLength: 75),
                        Address = c.String(nullable: false, maxLength: 100),
                        ContactPersonName = c.String(nullable: false, maxLength: 75),
                        ContactPersonPhoneNumber = c.String(nullable: false, maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUser = c.String(nullable: false, maxLength: 75),
                        ModifiedDate = c.DateTime(),
                        ModifiedUser = c.String(maxLength: 75),
                        DeletedDate = c.DateTime(),
                        DeletedUser = c.String(maxLength: 75),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ClientId = c.String(nullable: false, maxLength: 128),
                        TransporterId = c.String(maxLength: 128),
                        TransporterPoNumber = c.String(maxLength: 75),
                        InvoiceDate = c.DateTime(nullable: false),
                        PoNumber = c.String(nullable: false, maxLength: 75),
                        GeneralWaybillNumber = c.String(maxLength: 75),
                        Driver = c.String(nullable: false, maxLength: 75),
                        VehicleRegistrationNumber = c.String(maxLength: 75),
                        BranchCode = c.String(nullable: false, maxLength: 3),
                        Status = c.String(nullable: false, maxLength: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUser = c.String(nullable: false, maxLength: 75),
                        ModifiedDate = c.DateTime(),
                        ModifiedUser = c.String(maxLength: 75),
                        DeletedDate = c.DateTime(),
                        DeletedUser = c.String(maxLength: 75),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.Transporters", t => t.TransporterId)
                .Index(t => t.ClientId)
                .Index(t => t.TransporterId);
            
            CreateTable(
                "dbo.InvoiceItems",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        InvoiceId = c.String(nullable: false, maxLength: 128),
                        Quantity = c.Short(),
                        UnitSize = c.Decimal(precision: 18, scale: 3),
                        TotalKg = c.Decimal(precision: 18, scale: 3),
                        ProductId = c.String(nullable: false, maxLength: 128),
                        Pallets = c.String(),
                        BatchNumbers = c.String(),
                        Order = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUser = c.String(nullable: false, maxLength: 75),
                        ModifiedDate = c.DateTime(),
                        ModifiedUser = c.String(maxLength: 75),
                        DeletedDate = c.DateTime(),
                        DeletedUser = c.String(maxLength: 75),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.InvoiceId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Transporters",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 75),
                        ContactPerson = c.String(nullable: false, maxLength: 100),
                        ContactNumber = c.String(nullable: false, maxLength: 20),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUser = c.String(nullable: false, maxLength: 75),
                        ModifiedDate = c.DateTime(),
                        ModifiedUser = c.String(maxLength: 75),
                        DeletedDate = c.DateTime(),
                        DeletedUser = c.String(maxLength: 75),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LastWaybillNumber = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUser = c.String(nullable: false, maxLength: 75),
                        ModifiedDate = c.DateTime(),
                        ModifiedUser = c.String(maxLength: 75),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 75),
                        LastName = c.String(nullable: false, maxLength: 75),
                        BranchCode = c.String(nullable: false, maxLength: 3),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUser = c.String(maxLength: 75),
                        ModifiedDate = c.DateTime(),
                        ModifiedUser = c.String(),
                        DeletedDate = c.DateTime(),
                        DeletedUser = c.String(maxLength: 75),
                        IsDeleted = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ClientBranches", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Invoices", "TransporterId", "dbo.Transporters");
            DropForeignKey("dbo.InvoiceItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.InvoiceItems", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientBranches", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.ProductBranches", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductBranches", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Products", "BranchId", "dbo.Branches");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.InvoiceItems", new[] { "ProductId" });
            DropIndex("dbo.InvoiceItems", new[] { "InvoiceId" });
            DropIndex("dbo.Invoices", new[] { "TransporterId" });
            DropIndex("dbo.Invoices", new[] { "ClientId" });
            DropIndex("dbo.ClientBranches", new[] { "BranchId" });
            DropIndex("dbo.ClientBranches", new[] { "ClientId" });
            DropIndex("dbo.ProductBranches", new[] { "BranchId" });
            DropIndex("dbo.ProductBranches", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "BranchId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Settings");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Transporters");
            DropTable("dbo.InvoiceItems");
            DropTable("dbo.Invoices");
            DropTable("dbo.Clients");
            DropTable("dbo.ClientBranches");
            DropTable("dbo.ProductBranches");
            DropTable("dbo.Products");
            DropTable("dbo.Branches");
        }
    }
}
