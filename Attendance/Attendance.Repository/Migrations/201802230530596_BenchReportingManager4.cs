namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BenchReportingManager4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BenchReportingManager", "StaffId", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BenchReportingManager", "StaffId", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
