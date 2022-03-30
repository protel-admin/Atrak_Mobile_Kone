namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OTapplicationIdUpdate : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.OTApplication");
            AlterColumn("dbo.OTApplication", "Id", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.OTApplication", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.OTApplication");
            AlterColumn("dbo.OTApplication", "Id", c => c.String(nullable: false, maxLength: 10));
            AddPrimaryKey("dbo.OTApplication", "Id");
        }
    }
}
