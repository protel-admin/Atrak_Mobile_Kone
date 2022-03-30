namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSomeFieldsinApplicationApprovalAndStaffOfficial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationApproval", "Approval2Owner", c => c.String(maxLength: 20));
            AddColumn("dbo.ApplicationApproval", "Approval2statusId", c => c.Int(nullable: false));
            AddColumn("dbo.ApplicationApproval", "Approval2On", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.ApplicationApproval", "Approval2By", c => c.String(maxLength: 20));
            AddColumn("dbo.StaffOfficial", "Approver2", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffOfficial", "Approver2");
            DropColumn("dbo.ApplicationApproval", "Approval2By");
            DropColumn("dbo.ApplicationApproval", "Approval2On");
            DropColumn("dbo.ApplicationApproval", "Approval2statusId");
            DropColumn("dbo.ApplicationApproval", "Approval2Owner");
        }
    }
}
