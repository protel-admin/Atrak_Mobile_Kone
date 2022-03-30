namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldInHolidayGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HolidayGroup", "CreatedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.HolidayGroup", "CreatedBy", c => c.String());
            AddColumn("dbo.HolidayGroup", "ModifiedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.HolidayGroup", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HolidayGroup", "ModifiedBy");
            DropColumn("dbo.HolidayGroup", "ModifiedOn");
            DropColumn("dbo.HolidayGroup", "CreatedBy");
            DropColumn("dbo.HolidayGroup", "CreatedOn");
        }
    }
}
