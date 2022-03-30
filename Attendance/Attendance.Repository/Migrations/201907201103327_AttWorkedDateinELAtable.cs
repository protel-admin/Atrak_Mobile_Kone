namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttWorkedDateinELAtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeLeaveAccount", "IsManuallyExtended", c => c.Boolean(nullable: false));
            AddColumn("dbo.EmployeeLeaveAccount", "ExtensionPeriod", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.EmployeeLeaveAccount", "WorkedDate", c => c.DateTime(storeType: "smalldatetime"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeLeaveAccount", "WorkedDate");
            DropColumn("dbo.EmployeeLeaveAccount", "ExtensionPeriod");
            DropColumn("dbo.EmployeeLeaveAccount", "IsManuallyExtended");
        }
    }
}
