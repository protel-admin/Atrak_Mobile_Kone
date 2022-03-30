namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnIsmanualstatusToEtraHoursApprovedByInAttendancedataTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttendanceData", "IsManualStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.AttendanceData", "ApprovedExtraHours", c => c.String());
            AddColumn("dbo.AttendanceData", "ConsiderExtraHoursFor", c => c.String());
            AddColumn("dbo.AttendanceData", "IsExtraHoursProcessed", c => c.Boolean(nullable: false));
            AddColumn("dbo.AttendanceData", "ExtraHoursApprovedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.AttendanceData", "ExtraHoursApprovedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttendanceData", "ExtraHoursApprovedBy");
            DropColumn("dbo.AttendanceData", "ExtraHoursApprovedOn");
            DropColumn("dbo.AttendanceData", "IsExtraHoursProcessed");
            DropColumn("dbo.AttendanceData", "ConsiderExtraHoursFor");
            DropColumn("dbo.AttendanceData", "ApprovedExtraHours");
            DropColumn("dbo.AttendanceData", "IsManualStatus");
        }
    }
}
