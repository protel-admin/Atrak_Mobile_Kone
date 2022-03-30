namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShiftConfigurationinStaffOfficial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffOfficial", "AutoShift", c => c.Boolean(nullable: false));
            AddColumn("dbo.StaffOfficial", "GeneralShift", c => c.Boolean(nullable: false));
            AddColumn("dbo.StaffOfficial", "ShiftPattern", c => c.Boolean(nullable: false));
            AddColumn("dbo.StaffOfficial", "ShiftId", c => c.String(maxLength: 10));
            AddColumn("dbo.StaffOfficial", "ShiftPatternId", c => c.Int(nullable: false));
            CreateIndex("dbo.StaffOfficial", "ShiftId");
            CreateIndex("dbo.StaffOfficial", "ShiftPatternId");
            AddForeignKey("dbo.StaffOfficial", "ShiftPatternId", "dbo.ShiftPattern", "Id");
            AddForeignKey("dbo.StaffOfficial", "ShiftId", "dbo.Shifts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StaffOfficial", "ShiftId", "dbo.Shifts");
            DropForeignKey("dbo.StaffOfficial", "ShiftPatternId", "dbo.ShiftPattern");
            DropIndex("dbo.StaffOfficial", new[] { "ShiftPatternId" });
            DropIndex("dbo.StaffOfficial", new[] { "ShiftId" });
            DropColumn("dbo.StaffOfficial", "ShiftPatternId");
            DropColumn("dbo.StaffOfficial", "ShiftId");
            DropColumn("dbo.StaffOfficial", "ShiftPattern");
            DropColumn("dbo.StaffOfficial", "GeneralShift");
            DropColumn("dbo.StaffOfficial", "AutoShift");
        }
    }
}
