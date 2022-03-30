namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldInEmployeeShiftPlan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeShiftPlan", "NoOfDaysShift", c => c.Int(nullable: false));
            AddColumn("dbo.EmployeeShiftPlan", "IsMonthlyPattern", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeShiftPlan", "IsMonthlyPattern");
            DropColumn("dbo.EmployeeShiftPlan", "NoOfDaysShift");
        }
    }
}
