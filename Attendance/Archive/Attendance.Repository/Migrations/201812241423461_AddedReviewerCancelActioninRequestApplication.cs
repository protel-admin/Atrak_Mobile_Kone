namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedReviewerCancelActioninRequestApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestApplication", "IsReviewerCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestApplication", "ReviewerCancelledDate", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.RequestApplication", "ReviewerCancelledBy", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestApplication", "ReviewerCancelledBy");
            DropColumn("dbo.RequestApplication", "ReviewerCancelledDate");
            DropColumn("dbo.RequestApplication", "IsReviewerCancelled");
        }
    }
}
