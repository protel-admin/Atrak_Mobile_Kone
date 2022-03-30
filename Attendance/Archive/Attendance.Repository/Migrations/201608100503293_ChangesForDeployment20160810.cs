namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesForDeployment20160810 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttendanceControlTable",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.String(nullable: false, maxLength: 10),
                        FromDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        ToDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        IsProcessed = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        CreatedBy = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ApplicationEntry", "TotalDays", c => c.String());
            AddColumn("dbo.ApplicationEntry", "IsCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShiftChangeApplication", "CreatedOn", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            AddColumn("dbo.ShiftChangeApplication", "CreatedBy", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShiftChangeApplication", "CreatedBy");
            DropColumn("dbo.ShiftChangeApplication", "CreatedOn");
            DropColumn("dbo.ApplicationEntry", "IsCancelled");
            DropColumn("dbo.ApplicationEntry", "TotalDays");
            DropTable("dbo.AttendanceControlTable");
        }
    }
}
