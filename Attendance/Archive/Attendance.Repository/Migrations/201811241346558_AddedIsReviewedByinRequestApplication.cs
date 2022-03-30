namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsReviewedByinRequestApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestApplication", "IsReviewed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestApplication", "IsReviewed");
        }
    }
}
