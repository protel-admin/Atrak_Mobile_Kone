namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StaffOfficialUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffOfficial", "IsAutoShift", c => c.Boolean(nullable: false));
            AddColumn("dbo.StaffOfficial", "IsGeneralShift", c => c.Boolean(nullable: false));
            AddColumn("dbo.StaffOfficial", "IsShiftPattern", c => c.Boolean(nullable: false));
            AddColumn("dbo.StaffOfficial", "Isflexishift", c => c.Boolean(nullable: false));
            AddColumn("dbo.StaffOfficial", "ShiftId", c => c.String(maxLength: 10));
            AddColumn("dbo.StaffOfficial", "ShiftPatternId", c => c.Int(nullable: false));
            AddColumn("dbo.StaffOfficial", "Flexishift", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.StaffOfficial", "ApproverLevel", c => c.Int(nullable: false));
            CreateIndex("dbo.StaffOfficial", "ShiftId");
            CreateIndex("dbo.StaffOfficial", "ShiftPatternId");
            AddForeignKey("dbo.StaffOfficial", "ShiftPatternId", "dbo.ShiftPattern", "Id");
            AddForeignKey("dbo.StaffOfficial", "ShiftId", "dbo.Shifts", "Id");
            DropColumn("dbo.StaffOfficial", "OTReviewer");
            DropColumn("dbo.StaffOfficial", "OTReportingManager");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StaffOfficial", "OTReportingManager", c => c.String(maxLength: 20));
            AddColumn("dbo.StaffOfficial", "OTReviewer", c => c.String(maxLength: 20));
            DropForeignKey("dbo.StaffOfficial", "ShiftId", "dbo.Shifts");
            DropForeignKey("dbo.StaffOfficial", "ShiftPatternId", "dbo.ShiftPattern");
            DropIndex("dbo.StaffOfficial", new[] { "ShiftPatternId" });
            DropIndex("dbo.StaffOfficial", new[] { "ShiftId" });
            DropColumn("dbo.StaffOfficial", "ApproverLevel");
            DropColumn("dbo.StaffOfficial", "Flexishift");
            DropColumn("dbo.StaffOfficial", "ShiftPatternId");
            DropColumn("dbo.StaffOfficial", "ShiftId");
            DropColumn("dbo.StaffOfficial", "Isflexishift");
            DropColumn("dbo.StaffOfficial", "IsShiftPattern");
            DropColumn("dbo.StaffOfficial", "IsGeneralShift");
            DropColumn("dbo.StaffOfficial", "IsAutoShift");
        }
    }
}
