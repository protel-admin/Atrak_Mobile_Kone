namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTableManualShiftChangeGrid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ManualShiftChangeGrid",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.String(),
                        ShiftId = c.String(),
                        TxnDate = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false, storeType: "smalldatetime"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ManualShiftChangeGrid");
        }
    }
}
