namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedfieldinManualPunchTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ManualPunch", "PunchType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ManualPunch", "PunchType");
        }
    }
}
