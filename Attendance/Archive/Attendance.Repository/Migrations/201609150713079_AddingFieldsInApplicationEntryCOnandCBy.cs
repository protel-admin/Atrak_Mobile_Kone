namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFieldsInApplicationEntryCOnandCBy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationEntry", "CancelledOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.ApplicationEntry", "CancelledBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationEntry", "CancelledBy");
            DropColumn("dbo.ApplicationEntry", "CancelledOn");
        }
    }
}
