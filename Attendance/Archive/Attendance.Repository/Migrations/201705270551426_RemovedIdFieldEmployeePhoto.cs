namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedIdFieldEmployeePhoto : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.EmployeePhoto");
            AlterColumn("dbo.EmployeePhoto", "StaffId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.EmployeePhoto", "StaffId");
            DropColumn("dbo.EmployeePhoto", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmployeePhoto", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.EmployeePhoto");
            AlterColumn("dbo.EmployeePhoto", "StaffId", c => c.String(nullable: false));
            AddPrimaryKey("dbo.EmployeePhoto", "Id");
        }
    }
}
