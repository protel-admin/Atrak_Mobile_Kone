namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldReasonInESP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeShiftPlan", "Reason", c => c.String(nullable: false, maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeShiftPlan", "Reason");
        }
    }
}
