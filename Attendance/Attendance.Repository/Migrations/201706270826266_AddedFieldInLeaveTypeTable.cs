namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldInLeaveTypeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LeaveType", "CreatedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.LeaveType", "CreatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.LeaveType", "ModifiedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.LeaveType", "ModifiedBy", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LeaveType", "ModifiedBy");
            DropColumn("dbo.LeaveType", "ModifiedOn");
            DropColumn("dbo.LeaveType", "CreatedBy");
            DropColumn("dbo.LeaveType", "CreatedOn");
        }
    }
}
