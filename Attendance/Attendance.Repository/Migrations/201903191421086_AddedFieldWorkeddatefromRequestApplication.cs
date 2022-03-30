namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldWorkeddatefromRequestApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestApplication", "ExpiryDate", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            AddColumn("dbo.RequestApplication", "WorkedDate", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestApplication", "WorkedDate");
            DropColumn("dbo.RequestApplication", "ExpiryDate");
        }
    }
}
