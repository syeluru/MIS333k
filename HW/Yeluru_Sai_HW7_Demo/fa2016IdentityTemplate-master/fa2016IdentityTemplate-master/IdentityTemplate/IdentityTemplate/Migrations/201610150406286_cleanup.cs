namespace IdentityTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cleanup : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "FName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "FName", c => c.String());
        }
    }
}
