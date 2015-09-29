namespace admin_mode.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class whatthefuckhappenednow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            DropColumn("dbo.AspNetUsers", "Addrees");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Addrees", c => c.String());
            DropColumn("dbo.AspNetUsers", "Address");
        }
    }
}
