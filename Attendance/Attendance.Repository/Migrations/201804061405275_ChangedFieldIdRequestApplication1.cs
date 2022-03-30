namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedFieldIdRequestApplication1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.RequestApplication");
            AlterColumn("dbo.RequestApplication", "Id", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.RequestApplication", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.RequestApplication");
            AlterColumn("dbo.RequestApplication", "Id", c => c.Double(nullable: false));
            AddPrimaryKey("dbo.RequestApplication", "Id");
        }
    }
}
