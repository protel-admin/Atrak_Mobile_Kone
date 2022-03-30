namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveReqFieldFromShifts : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Shifts", "GraceLateBy", c => c.DateTime(storeType: "smalldatetime"));
            AlterColumn("dbo.Shifts", "GraceEarlyBY", c => c.DateTime(storeType: "smalldatetime"));
            AlterColumn("dbo.Shifts", "BreakStartTime", c => c.DateTime(storeType: "smalldatetime"));
            AlterColumn("dbo.Shifts", "BreakEndTime", c => c.DateTime(storeType: "smalldatetime"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Shifts", "BreakEndTime", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            AlterColumn("dbo.Shifts", "BreakStartTime", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            AlterColumn("dbo.Shifts", "GraceEarlyBY", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            AlterColumn("dbo.Shifts", "GraceLateBy", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
        }
    }
}
