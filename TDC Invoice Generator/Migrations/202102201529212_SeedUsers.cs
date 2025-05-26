namespace TDC_Invoice_Generator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [FirstName], [LastName], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [CreatedDate], [CreatedUser]) VALUES 
                (N'1c099372-db5c-4533-b361-66614acaa6a1', N'hellmut@tdconcepts.africa', 'Hellmut', 'Probst', 0, N'ABX5GUcACAMHaXyB2XiES2/yY5uZVdPzmYoifoJ6ClN1/50e27MkjLPzqsVfPgnXsA==', N'c1764162-a540-4418-a74b-fcb77a3e9428', NULL, 0, 0, NULL, 1, 0, N'hellmut@tdconcepts.africa', GETDATE(), 'Admin')
                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'1c099372-db5c-4533-b361-66614acaa6a1', N'de0d5968-b46d-499a-b90a-7f56c6ff59d4')
            ");
        }
        
        public override void Down()
        {
        }
    }
}
