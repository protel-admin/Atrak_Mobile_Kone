namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldInAttendanceData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttendanceData", "IsShiftPlanMissing", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttendanceData", "IsShiftPlanMissing");
        }
    }
}
