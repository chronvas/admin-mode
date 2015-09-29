namespace admin_mode.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADDED_ADDRESS : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Addrees", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Addrees");
        }
    }
}
