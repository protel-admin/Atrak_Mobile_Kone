namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMigrationOnAttendanceChange19042021 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttendanceStatusChange",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.String(nullable: false, maxLength: 20),
                        ShiftDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        Status = c.String(nullable: false, maxLength: 10),
                        Remarks = c.String(nullable: false, maxLength: 200),
                        IsCancelled = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        CreatedBy = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AttendanceStatusChange");
        }
    }
}
