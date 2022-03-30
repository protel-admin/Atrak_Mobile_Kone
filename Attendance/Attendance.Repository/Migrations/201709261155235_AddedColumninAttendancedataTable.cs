namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedColumninAttendancedataTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttendanceData", "IsBreakExceeded", c => c.Boolean(nullable: false));
            AddColumn("dbo.AttendanceData", "IsBreakExceedValid", c => c.Boolean(nullable: false));
            AddColumn("dbo.AttendanceData", "IsBreakDisputed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttendanceData", "IsBreakDisputed");
            DropColumn("dbo.AttendanceData", "IsBreakExceedValid");
            DropColumn("dbo.AttendanceData", "IsBreakExceeded");
        }
    }
}
