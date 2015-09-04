namespace admin_mode.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class combobox : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComboItems",
                c => new
                    {
                        CombooItemId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CombooItemId);
            
            CreateTable(
                "dbo.ApplicationUserComboItems",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        ComboItem_CombooItemId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.ComboItem_CombooItemId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.ComboItems", t => t.ComboItem_CombooItemId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ComboItem_CombooItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserComboItems", "ComboItem_CombooItemId", "dbo.ComboItems");
            DropForeignKey("dbo.ApplicationUserComboItems", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserComboItems", new[] { "ComboItem_CombooItemId" });
            DropIndex("dbo.ApplicationUserComboItems", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserComboItems");
            DropTable("dbo.ComboItems");
        }
    }
}
