namespace Yeluru_Sai_HW7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
                        AppUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EventID)
                .ForeignKey("dbo.Committees", t => t.SponsoringCommittee_CommitteeID)
                .ForeignKey("dbo.AspNetUsers", t => t.AppUser_Id)
                .Index(t => t.SponsoringCommittee_CommitteeID)
                .Index(t => t.AppUser_Id);
            
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
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(nullable: false),
                    UserId = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(maxLength: 256),
                        PhoneNumber = c.String(),
                        OKToText = c.Boolean(nullable: false),
                        McCombsMajors = c.Int(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
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
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Events", "AppUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Events", "SponsoringCommittee_CommitteeID", "dbo.Committees");
            DropForeignKey("dbo.MemberEvents", "Event_EventID", "dbo.Events");
            DropForeignKey("dbo.MemberEvents", "Member_MemberID", "dbo.Members");
            DropIndex("dbo.MemberEvents", new[] { "Event_EventID" });
            DropIndex("dbo.MemberEvents", new[] { "Member_MemberID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Events", new[] { "AppUser_Id" });
            DropIndex("dbo.Events", new[] { "SponsoringCommittee_CommitteeID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.MemberEvents");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Members");
            DropTable("dbo.Events");
            DropTable("dbo.Committees");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
