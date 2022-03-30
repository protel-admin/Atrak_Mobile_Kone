namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShiftExtensionAndDoubleShiftTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShiftExtensionAndDoubleShift",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.String(maxLength: 30),
                        TxnDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        ShiftExtensionType = c.String(maxLength: 30),
                        DurationOfHoursExtension = c.String(maxLength: 30),
                        NoOfHoursBeforeShift = c.Int(nullable: false),
                        NoOfHoursAfterShift = c.Int(nullable: false),
                        Shift1 = c.String(maxLength: 50),
                        Shift2 = c.String(maxLength: 50),
                        Shift3 = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ShiftExtensionAndDoubleShift");
        }
    }
}
