namespace Yeluru_Sai_HW7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Committees",
                c => new
                    {
                        CommitteeID = c.Short(nullable: false, identity: true),
                        CommitteeName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CommitteeID);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventID = c.Short(nullable: false, identity: true),
                        EventTitle = c.String(nullable: false),
                        EventDate = c.DateTime(nullable: false),
                        EventLocation = c.String(nullable: false),
                        MembersOnly = c.Boolean(nullable: false),
                        SponsoringCommittee_CommitteeID = c.Short(),
                    })
                .PrimaryKey(t => t.EventID)
                .ForeignKey("dbo.Committees", t => t.SponsoringCommittee_CommitteeID)
                .Index(t => t.SponsoringCommittee_CommitteeID);
            
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
            
            CreateTable(
                "dbo.MemberEvents",
                c => new
                    {
                        Member_MemberID = c.Short(nullable: false),
                        Event_EventID = c.Short(nullable: false),
                    })
                .PrimaryKey(t => new { t.Member_MemberID, t.Event_EventID })
                .ForeignKey("dbo.Members", t => t.Member_MemberID, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.Event_EventID, cascadeDelete: true)
                .Index(t => t.Member_MemberID)
                .Index(t => t.Event_EventID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "SponsoringCommittee_CommitteeID", "dbo.Committees");
            DropForeignKey("dbo.MemberEvents", "Event_EventID", "dbo.Events");
            DropForeignKey("dbo.MemberEvents", "Member_MemberID", "dbo.Members");
            DropIndex("dbo.MemberEvents", new[] { "Event_EventID" });
            DropIndex("dbo.MemberEvents", new[] { "Member_MemberID" });
            DropIndex("dbo.Events", new[] { "SponsoringCommittee_CommitteeID" });
            DropTable("dbo.MemberEvents");
            DropTable("dbo.Members");
            DropTable("dbo.Events");
            DropTable("dbo.Committees");
        }
    }
}
