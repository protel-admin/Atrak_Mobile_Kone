namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTableRequestApplication : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestApplication",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        StaffId = c.String(nullable: false),
                        LeaveTypeId = c.String(),
                        LeaveStartDurationId = c.Int(nullable: false),
                        StartDate = c.DateTime(storeType: "smalldatetime"),
                        EndDate = c.DateTime(storeType: "smalldatetime"),
                        LeaveEndDurationId = c.Int(nullable: false),
                        TotalDays = c.String(),
                        PermissionType = c.String(),
                        OTRange = c.String(),
                        ODDuration = c.String(),
                        NewShiftId = c.String(),
                        RHId = c.Int(nullable: false),
                        TotalHours = c.DateTime(storeType: "smalldatetime"),
                        Remarks = c.String(),
                        ReasonId = c.Int(nullable: false),
                        ContactNumber = c.String(),
                        PunchType = c.String(),
                        ApplicationDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        AppliedBy = c.String(nullable: false),
                        IsCancelled = c.Boolean(nullable: false),
                        CancelledDate = c.DateTime(storeType: "smalldatetime"),
                        CancelledBy = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RequestApplication");
        }
    }
}
