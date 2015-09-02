namespace admin_mode.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newModelForAddingNewUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddNewUserViewModels",
                c => new
                    {
                        Email = c.String(nullable: false, maxLength: 128),
                        Password = c.String(nullable: false, maxLength: 100),
                        UserName = c.String(nullable: false, maxLength: 100),
                        EnrollmentDate = c.DateTime(nullable: false),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Email);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AddNewUserViewModels");
        }
    }
}
