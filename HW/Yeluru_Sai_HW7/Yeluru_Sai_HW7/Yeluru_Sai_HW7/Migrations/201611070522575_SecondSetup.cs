namespace Yeluru_Sai_HW7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondSetup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MemberEvents", "Member_MemberID", "dbo.Members");
            DropForeignKey("dbo.MemberEvents", "Event_EventID", "dbo.Events");
            DropForeignKey("dbo.Events", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Events", new[] { "AppUser_Id" });
            DropIndex("dbo.MemberEvents", new[] { "Member_MemberID" });
            DropIndex("dbo.MemberEvents", new[] { "Event_EventID" });
            CreateTable(
                "dbo.AppUserEvents",
                c => new
                    {
                        AppUser_Id = c.String(nullable: false, maxLength: 128),
                        Event_EventID = c.Short(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppUser_Id, t.Event_EventID })
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.Event_EventID, cascadeDelete: true)
                .Index(t => t.AppUser_Id)
                .Index(t => t.Event_EventID);
            
            AddColumn("dbo.AspNetUsers", "UserID", c => c.Short(nullable: false));
            DropColumn("dbo.Events", "AppUser_Id");
            DropColumn("dbo.AspNetUsers", "MemberID");
            DropTable("dbo.Members");
            DropTable("dbo.MemberEvents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MemberEvents",
                c => new
                    {
                        Member_MemberID = c.Short(nullable: false),
                        Event_EventID = c.Short(nullable: false),
                    })
                .PrimaryKey(t => new { t.Member_MemberID, t.Event_EventID });
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        MemberID = c.Short(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        OKToText = c.Boolean(nullable: false),
                        McCombsMajors = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MemberID);
            
            AddColumn("dbo.AspNetUsers", "MemberID", c => c.Short(nullable: false));
            AddColumn("dbo.Events", "AppUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.AppUserEvents", "Event_EventID", "dbo.Events");
            DropForeignKey("dbo.AppUserEvents", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AppUserEvents", new[] { "Event_EventID" });
            DropIndex("dbo.AppUserEvents", new[] { "AppUser_Id" });
            DropColumn("dbo.AspNetUsers", "UserID");
            DropTable("dbo.AppUserEvents");
            CreateIndex("dbo.MemberEvents", "Event_EventID");
            CreateIndex("dbo.MemberEvents", "Member_MemberID");
            CreateIndex("dbo.Events", "AppUser_Id");
            AddForeignKey("dbo.Events", "AppUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.MemberEvents", "Event_EventID", "dbo.Events", "EventID", cascadeDelete: true);
            AddForeignKey("dbo.MemberEvents", "Member_MemberID", "dbo.Members", "MemberID", cascadeDelete: true);
        }
    }
}
