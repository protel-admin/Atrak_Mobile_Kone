namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldFelxiAutoManualShiftsinELP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeShiftPlan", "IsFlexiShift", c => c.Boolean(nullable: false));
            AddColumn("dbo.EmployeeShiftPlan", "IsAutoShift", c => c.Boolean(nullable: false));
            AddColumn("dbo.EmployeeShiftPlan", "IsManualShift", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeShiftPlan", "IsManualShift");
            DropColumn("dbo.EmployeeShiftPlan", "IsAutoShift");
            DropColumn("dbo.EmployeeShiftPlan", "IsFlexiShift");
        }
    }
}
