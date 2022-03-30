namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRequiredFieldfromRequestApplication : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestApplication", "ExpiryDate", c => c.DateTime(storeType: "smalldatetime"));
            AlterColumn("dbo.RequestApplication", "WorkedDate", c => c.DateTime(storeType: "smalldatetime"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestApplication", "WorkedDate", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            AlterColumn("dbo.RequestApplication", "ExpiryDate", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
        }
    }
}
