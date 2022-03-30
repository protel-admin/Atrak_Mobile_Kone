namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableShiftbulkImport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShiftsImportData",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StaffId = c.String(nullable: false, maxLength: 20),
                        ShiftId = c.String(nullable: false, maxLength: 10),
                        ShiftFromDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        ShiftToDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        IsProcessed = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        CreatedBy = c.String(maxLength: 50),
                        ProcessedOn = c.DateTime(storeType: "smalldatetime"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ShiftsImportData");
        }
    }
}
