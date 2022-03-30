namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFieldAppTypeAndAppIdInACT : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttendanceControlTable", "ApplicationType", c => c.String());
            AddColumn("dbo.AttendanceControlTable", "ApplicationId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttendanceControlTable", "ApplicationId");
            DropColumn("dbo.AttendanceControlTable", "ApplicationType");
        }
    }
}
