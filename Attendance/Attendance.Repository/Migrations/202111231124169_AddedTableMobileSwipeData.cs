namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTableMobileSwipeData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlternativePersonAssign",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.String(nullable: false, maxLength: 20),
                        StaffId = c.String(nullable: false, maxLength: 20),
                        AlternativeStaffId = c.String(nullable: false, maxLength: 20),
                        IsCancelled = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsReviewed = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        IntimationMailSent = c.Boolean(nullable: false),
                        ConfirmationMailSent = c.Boolean(nullable: false),
                        CancellationMailSent = c.Boolean(nullable: false),
                        RejectMailSent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MobileSwipeTransaction",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StaffId = c.String(),
                        PunchMode = c.String(),
                        PunchDateTime = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Lattitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Radius = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ApplicationApproval", "ReviewerstatusId", c => c.Int(nullable: false));
            AddColumn("dbo.ApplicationApproval", "ReviewedBy", c => c.String(maxLength: 20));
            AddColumn("dbo.ApplicationApproval", "ReviewedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.ApplicationApproval", "ReviewerOwner", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.RequestApplication", "IsReviewed", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestApplication", "IsApproverCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestApplication", "ApproverCancelledDate", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.RequestApplication", "ApproverCancelledBy", c => c.String(maxLength: 10));
            AddColumn("dbo.RequestApplication", "IsReviewerCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestApplication", "ReviewerCancelledDate", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.RequestApplication", "ReviewerCancelledBy", c => c.String(maxLength: 10));
            AlterColumn("dbo.RequestApplication", "StartDate", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            AlterColumn("dbo.RequestApplication", "EndDate", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestApplication", "EndDate", c => c.DateTime(storeType: "smalldatetime"));
            AlterColumn("dbo.RequestApplication", "StartDate", c => c.DateTime(storeType: "smalldatetime"));
            DropColumn("dbo.RequestApplication", "ReviewerCancelledBy");
            DropColumn("dbo.RequestApplication", "ReviewerCancelledDate");
            DropColumn("dbo.RequestApplication", "IsReviewerCancelled");
            DropColumn("dbo.RequestApplication", "ApproverCancelledBy");
            DropColumn("dbo.RequestApplication", "ApproverCancelledDate");
            DropColumn("dbo.RequestApplication", "IsApproverCancelled");
            DropColumn("dbo.RequestApplication", "IsReviewed");
            DropColumn("dbo.ApplicationApproval", "ReviewerOwner");
            DropColumn("dbo.ApplicationApproval", "ReviewedOn");
            DropColumn("dbo.ApplicationApproval", "ReviewedBy");
            DropColumn("dbo.ApplicationApproval", "ReviewerstatusId");
            DropTable("dbo.MobileSwipeTransaction");
            DropTable("dbo.AlternativePersonAssign");
        }
    }
}
