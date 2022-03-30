namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableEmpShiftPlanAndColumnPatternDescInWDP : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeShiftPlan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.String(nullable: false, maxLength: 10),
                        ShiftId = c.String(nullable: false, maxLength: 6),
                        PatternId = c.Int(nullable: false),
                        DayPatternId = c.Int(nullable: false),
                        WeeklyOffId = c.String(nullable: false, maxLength: 10),
                        IsGeneralShift = c.Boolean(nullable: false),
                        UseDayPattern = c.Boolean(nullable: false),
                        UseWeeklyOff = c.Boolean(nullable: false),
                        LastUpdatedDate = c.DateTime(storeType: "smalldatetime"),
                        LastUpdatedShiftId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        CreatedBy = c.String(nullable: false, maxLength: 10),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.WorkingDayPattern", "PatternDesc", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkingDayPattern", "PatternDesc");
            DropTable("dbo.EmployeeShiftPlan");
        }
    }
}
