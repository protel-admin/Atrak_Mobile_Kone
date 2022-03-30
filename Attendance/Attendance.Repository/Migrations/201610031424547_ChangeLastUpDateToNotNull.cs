namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLastUpDateToNotNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EmployeeShiftPlan", "LastUpdatedDate", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmployeeShiftPlan", "LastUpdatedDate", c => c.DateTime(storeType: "smalldatetime"));
        }
    }
}
