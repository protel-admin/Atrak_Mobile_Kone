namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEmployeeLeaveAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeLeaveAccount", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.EmployeeLeaveAccount", "Month", c => c.Int(nullable: false));
            AddColumn("dbo.EmployeeLeaveAccount", "IsManuallyExtended", c => c.Boolean(nullable: false));
            AddColumn("dbo.EmployeeLeaveAccount", "ExtensionPeriod", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EmployeeLeaveAccount", "TransctionBy", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmployeeLeaveAccount", "TransctionBy", c => c.String(maxLength: 10));
            DropColumn("dbo.EmployeeLeaveAccount", "ExtensionPeriod");
            DropColumn("dbo.EmployeeLeaveAccount", "IsManuallyExtended");
            DropColumn("dbo.EmployeeLeaveAccount", "Month");
            DropColumn("dbo.EmployeeLeaveAccount", "Year");
        }
    }
}
