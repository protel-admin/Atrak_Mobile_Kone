namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFieldIsWeekPatternInESP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeShiftPlan", "IsWeekPattern", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeShiftPlan", "IsWeekPattern");
        }
    }
}
