namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedfieldinauditlog : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ChangeAuditLog", "PrimaryKeyValue", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ChangeAuditLog", "PrimaryKeyValue", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
