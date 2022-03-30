namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOTapplicationandAlternativePersonAssign : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.OTApplication");
            AddColumn("dbo.AlternativePersonAssign", "IsReviewed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.OTApplication", "Id", c => c.String(nullable: false, maxLength: 40));
            AddPrimaryKey("dbo.OTApplication", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.OTApplication");
            AlterColumn("dbo.OTApplication", "Id", c => c.String(nullable: false, maxLength: 10));
            DropColumn("dbo.AlternativePersonAssign", "IsReviewed");
            AddPrimaryKey("dbo.OTApplication", "Id");
        }
    }
}
