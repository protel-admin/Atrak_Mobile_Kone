namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInDtypeOfShiftsTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Shifts", "MinDayHours", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Shifts", "MinWeekHours", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Shifts", "MinWeekHours", c => c.Int(nullable: false));
            AlterColumn("dbo.Shifts", "MinDayHours", c => c.Int(nullable: false));
        }
    }
}
