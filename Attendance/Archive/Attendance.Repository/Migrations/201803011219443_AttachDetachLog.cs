namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttachDetachLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttachDetachLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.String(nullable: false, maxLength: 10),
                        IsAttached = c.Boolean(nullable: false),
                        ReportingManager = c.String(maxLength: 10),
                        StateChangedOn = c.DateTime(nullable: false, storeType: "smalldatetime"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AttachDetachLog");
        }
    }
}
