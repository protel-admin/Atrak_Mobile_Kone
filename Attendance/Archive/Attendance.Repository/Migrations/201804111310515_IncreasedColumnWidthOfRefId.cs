namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreasedColumnWidthOfRefId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EmployeeLeaveAccount", "RefId", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmployeeLeaveAccount", "RefId", c => c.String(maxLength: 10));
        }
    }
}
