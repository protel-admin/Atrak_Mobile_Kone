namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BenchReportingManager : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BenchReportingManager",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.String(nullable: false, maxLength: 20),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        CreatedBy = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BenchReportingManager");
        }
    }
}
