namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLateComingTable : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.LateComing");
            AlterColumn("dbo.LateComing", "Id", c => c.String(nullable: false, maxLength: 10));
            AddPrimaryKey("dbo.LateComing", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.LateComing");
            AlterColumn("dbo.LateComing", "Id", c => c.String(nullable: false, maxLength: 10));
            AddPrimaryKey("dbo.LateComing", "Id");
        }
    }
}
