namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFieldStartDateInESP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeShiftPlan", "StartDate", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeShiftPlan", "StartDate");
        }
    }
}
