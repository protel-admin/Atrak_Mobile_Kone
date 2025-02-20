namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsAttachDetachFieldToStaff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Staff", "IsAttached", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Staff", "IsAttached");
        }
    }
}
