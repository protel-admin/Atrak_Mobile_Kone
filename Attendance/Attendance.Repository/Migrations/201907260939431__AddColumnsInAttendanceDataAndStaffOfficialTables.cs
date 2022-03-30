namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _AddColumnsInAttendanceDataAndStaffOfficialTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttendanceData", "ActualShiftIN", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.AttendanceData", "ActualShiftOUT", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.AttendanceData", "ActualWorkingHours", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.AttendanceData", "FHAccount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AttendanceData", "SHAccount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AttendanceData", "ProcessId", c => c.Int(nullable: false));
            AddColumn("dbo.AttendanceData", "IsToBeLeaveDeducted", c => c.Boolean(nullable: false));
            AddColumn("dbo.AttendanceData", "IsAutoLeaveDeducted", c => c.Boolean(nullable: false));
            AddColumn("dbo.StaffOfficial", "ApproverLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffOfficial", "ApproverLevel");
            DropColumn("dbo.AttendanceData", "IsAutoLeaveDeducted");
            DropColumn("dbo.AttendanceData", "IsToBeLeaveDeducted");
            DropColumn("dbo.AttendanceData", "ProcessId");
            DropColumn("dbo.AttendanceData", "SHAccount");
            DropColumn("dbo.AttendanceData", "FHAccount");
            DropColumn("dbo.AttendanceData", "ActualWorkingHours");
            DropColumn("dbo.AttendanceData", "ActualShiftOUT");
            DropColumn("dbo.AttendanceData", "ActualShiftIN");
        }
    }
}
