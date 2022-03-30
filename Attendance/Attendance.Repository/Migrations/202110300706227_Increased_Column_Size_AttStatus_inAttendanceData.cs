namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Increased_Column_Size_AttStatus_inAttendanceData : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AttendanceData", "AttendanceStatus", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AttendanceData", "AttendanceStatus", c => c.String(nullable: false, maxLength: 5));
        }
    }
}
