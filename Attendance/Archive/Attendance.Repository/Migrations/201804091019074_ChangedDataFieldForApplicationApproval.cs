namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDataFieldForApplicationApproval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestApplication", "IsCancelApprovalRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestApplication", "IsCancelApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestApplication", "IsCancelRejected", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ApplicationApproval", "ApprovedBy", c => c.String(maxLength: 20));
            AlterColumn("dbo.ApplicationApproval", "ApprovedOn", c => c.DateTime(storeType: "smalldatetime"));
            AlterColumn("dbo.ApplicationApproval", "Comment", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationApproval", "Comment", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.ApplicationApproval", "ApprovedOn", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            AlterColumn("dbo.ApplicationApproval", "ApprovedBy", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.RequestApplication", "IsCancelRejected");
            DropColumn("dbo.RequestApplication", "IsCancelApproved");
            DropColumn("dbo.RequestApplication", "IsCancelApprovalRequired");
        }
    }
}
