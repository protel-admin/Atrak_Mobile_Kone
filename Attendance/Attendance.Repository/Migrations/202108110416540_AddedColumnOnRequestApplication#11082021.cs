namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedColumnOnRequestApplication11082021 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestApplication", "ShiftExtensionType", c => c.String(maxLength: 20));
            AddColumn("dbo.RequestApplication", "DurationOfHoursExtension", c => c.String(maxLength: 30));
            AddColumn("dbo.RequestApplication", "HoursBeforeShift", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.RequestApplication", "HoursAfterShift", c => c.DateTime(storeType: "smalldatetime"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestApplication", "HoursAfterShift");
            DropColumn("dbo.RequestApplication", "HoursBeforeShift");
            DropColumn("dbo.RequestApplication", "DurationOfHoursExtension");
            DropColumn("dbo.RequestApplication", "ShiftExtensionType");
        }
    }
}
