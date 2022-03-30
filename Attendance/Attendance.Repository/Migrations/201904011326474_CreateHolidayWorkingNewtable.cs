namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateHolidayWorkingNewtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HolidayWorking",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.String(nullable: false, maxLength: 20),
                        ShiftId = c.String(maxLength: 10),
                        TxnDate = c.DateTime(storeType: "smalldatetime"),
                        ShiftInDate = c.DateTime(storeType: "smalldatetime"),
                        ShiftInTime = c.DateTime(storeType: "smalldatetime"),
                        ShiftOutDate = c.DateTime(storeType: "smalldatetime"),
                        ShiftOutTime = c.DateTime(storeType: "smalldatetime"),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HolidayWorking");
        }
    }
}
