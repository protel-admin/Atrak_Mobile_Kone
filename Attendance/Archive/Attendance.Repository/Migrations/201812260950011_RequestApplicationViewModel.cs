namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestApplicationViewModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestApplication", "IsApproverCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestApplication", "ApproverCancelledDate", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.RequestApplication", "ApproverCancelledBy", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestApplication", "ApproverCancelledBy");
            DropColumn("dbo.RequestApplication", "ApproverCancelledDate");
            DropColumn("dbo.RequestApplication", "IsApproverCancelled");
        }
    }
}
