namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedIdentityEmployeePhoto : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.EmployeePhoto");
            AlterColumn("dbo.EmployeePhoto", "StaffId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.EmployeePhoto", "StaffId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.EmployeePhoto");
            AlterColumn("dbo.EmployeePhoto", "StaffId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.EmployeePhoto", "StaffId");
        }
    }
}
