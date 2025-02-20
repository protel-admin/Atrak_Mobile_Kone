namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRPMToNullInStafffOff : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StaffOfficial", "ReportingManager", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StaffOfficial", "ReportingManager", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
