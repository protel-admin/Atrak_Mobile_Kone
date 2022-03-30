namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldInWeeklyOffs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WeeklyOffs", "CreatedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.WeeklyOffs", "CreatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.WeeklyOffs", "ModifiedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.WeeklyOffs", "ModifiedBy", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WeeklyOffs", "ModifiedBy");
            DropColumn("dbo.WeeklyOffs", "ModifiedOn");
            DropColumn("dbo.WeeklyOffs", "CreatedBy");
            DropColumn("dbo.WeeklyOffs", "CreatedOn");
        }
    }
}
