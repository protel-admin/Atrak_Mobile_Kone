namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnIsOTEligibleinStaffOfficial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffOfficial", "IsOTEligible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffOfficial", "IsOTEligible");
        }
    }
}
