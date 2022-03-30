namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDataFieldForApplicationApproval1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ApplicationApproval");
            AlterColumn("dbo.ApplicationApproval", "Id", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.ApplicationApproval", "ParentId", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.ApplicationApproval", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ApplicationApproval");
            AlterColumn("dbo.ApplicationApproval", "ParentId", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.ApplicationApproval", "Id", c => c.String(nullable: false, maxLength: 10));
            AddPrimaryKey("dbo.ApplicationApproval", "Id");
        }
    }
}
