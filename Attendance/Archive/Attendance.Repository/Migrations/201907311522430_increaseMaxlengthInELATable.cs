namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class increaseMaxlengthInELATable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EmployeeLeaveAccount", "RefId", c => c.String(maxLength: 30));
            AlterColumn("dbo.EmployeeLeaveAccount", "TransctionBy", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmployeeLeaveAccount", "TransctionBy", c => c.String(maxLength: 20));
            AlterColumn("dbo.EmployeeLeaveAccount", "RefId", c => c.String(maxLength: 20));
        }
    }
}
