namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumnManualShiftinStaffOfficialtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffOfficial", "ManualShift", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffOfficial", "ManualShift");
        }
    }
}
