namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldIsRejectedToRequestApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestApplication", "IsRejected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestApplication", "IsRejected");
        }
    }
}
