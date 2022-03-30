namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveForeignKeyShiftPatternIdfromStaffOfficial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StaffOfficial", "ShiftPatternId", "dbo.ShiftPattern");
            DropForeignKey("dbo.StaffOfficial", "ShiftId", "dbo.Shifts");
            DropForeignKey("dbo.StaffOfficial", "WeeklyOffId", "dbo.WeeklyOffs");
            DropIndex("dbo.StaffOfficial", new[] { "WeeklyOffId" });
            DropIndex("dbo.StaffOfficial", new[] { "ShiftId" });
            DropIndex("dbo.StaffOfficial", new[] { "ShiftPatternId" });
            AlterColumn("dbo.StaffOfficial", "WeeklyOffId", c => c.String());
            AlterColumn("dbo.StaffOfficial", "ShiftId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StaffOfficial", "ShiftId", c => c.String(maxLength: 10));
            AlterColumn("dbo.StaffOfficial", "WeeklyOffId", c => c.String(maxLength: 10));
            CreateIndex("dbo.StaffOfficial", "ShiftPatternId");
            CreateIndex("dbo.StaffOfficial", "ShiftId");
            CreateIndex("dbo.StaffOfficial", "WeeklyOffId");
            AddForeignKey("dbo.StaffOfficial", "WeeklyOffId", "dbo.WeeklyOffs", "Id");
            AddForeignKey("dbo.StaffOfficial", "ShiftId", "dbo.Shifts", "Id");
            AddForeignKey("dbo.StaffOfficial", "ShiftPatternId", "dbo.ShiftPattern", "Id");
        }
    }
}
