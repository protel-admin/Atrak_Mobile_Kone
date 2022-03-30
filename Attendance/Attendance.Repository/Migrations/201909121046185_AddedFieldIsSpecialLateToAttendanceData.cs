namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldIsSpecialLateToAttendanceData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttendanceData", "IsSpecialLate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttendanceData", "IsSpecialLate");
        }
    }
}
