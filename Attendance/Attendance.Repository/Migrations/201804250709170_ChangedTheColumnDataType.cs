namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTheColumnDataType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EmployeeLeaveAccount", "TransctionBy", c => c.String(maxLength: 10));
            AlterColumn("dbo.EmployeeLeaveAccount", "IsSystemAction", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmployeeLeaveAccount", "IsSystemAction", c => c.String());
            AlterColumn("dbo.EmployeeLeaveAccount", "TransctionBy", c => c.String());
        }
    }
}
