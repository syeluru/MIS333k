namespace Yeluru_Sai_HW7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fifth : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "UserID", c => c.Int(nullable: false));
        }
    }
}
