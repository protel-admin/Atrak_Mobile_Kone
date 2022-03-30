namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedFieldsInRequestApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestApplication", "RequestApplicationType", c => c.String(nullable: false, maxLength: 2));
            AlterColumn("dbo.RequestApplication", "StaffId", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.RequestApplication", "LeaveTypeId", c => c.String(maxLength: 10));
            AlterColumn("dbo.RequestApplication", "PermissionType", c => c.String(maxLength: 20));
            AlterColumn("dbo.RequestApplication", "OTRange", c => c.String(maxLength: 20));
            AlterColumn("dbo.RequestApplication", "ODDuration", c => c.String(maxLength: 20));
            AlterColumn("dbo.RequestApplication", "NewShiftId", c => c.String(maxLength: 10));
            AlterColumn("dbo.RequestApplication", "Remarks", c => c.String(maxLength: 200));
            AlterColumn("dbo.RequestApplication", "ContactNumber", c => c.String(maxLength: 20));
            AlterColumn("dbo.RequestApplication", "PunchType", c => c.String(maxLength: 5));
            AlterColumn("dbo.RequestApplication", "AppliedBy", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.RequestApplication", "CancelledBy", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestApplication", "CancelledBy", c => c.String());
            AlterColumn("dbo.RequestApplication", "AppliedBy", c => c.String(nullable: false));
            AlterColumn("dbo.RequestApplication", "PunchType", c => c.String());
            AlterColumn("dbo.RequestApplication", "ContactNumber", c => c.String());
            AlterColumn("dbo.RequestApplication", "Remarks", c => c.String());
            AlterColumn("dbo.RequestApplication", "NewShiftId", c => c.String());
            AlterColumn("dbo.RequestApplication", "ODDuration", c => c.String());
            AlterColumn("dbo.RequestApplication", "OTRange", c => c.String());
            AlterColumn("dbo.RequestApplication", "PermissionType", c => c.String());
            AlterColumn("dbo.RequestApplication", "LeaveTypeId", c => c.String());
            AlterColumn("dbo.RequestApplication", "StaffId", c => c.String(nullable: false));
            DropColumn("dbo.RequestApplication", "RequestApplicationType");
        }
    }
}
