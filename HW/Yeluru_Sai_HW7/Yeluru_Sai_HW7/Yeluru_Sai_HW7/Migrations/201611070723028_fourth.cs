namespace Yeluru_Sai_HW7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fourth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UserID");
        }
    }
}
