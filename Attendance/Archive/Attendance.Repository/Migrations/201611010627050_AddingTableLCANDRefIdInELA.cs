namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingTableLCANDRefIdInELA : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LateComing",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 10),
                        StaffId = c.String(nullable: false, maxLength: 10),
                        TxnDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        ShiftIn = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        ShiftOut = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        SwipeIn = c.DateTime(storeType: "smalldatetime"),
                        SwipeOut = c.DateTime(storeType: "smalldatetime"),
                        IsLate = c.Boolean(nullable: false),
                        IsEarly = c.Boolean(nullable: false),
                        LateHours = c.DateTime(storeType: "smalldatetime"),
                        EarlyHours = c.DateTime(storeType: "smalldatetime"),
                        IsAbsentMarked = c.Boolean(nullable: false),
                        IsLeaveDeducted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.EmployeeLeaveAccount", "RefId", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeLeaveAccount", "RefId");
            DropTable("dbo.LateComing");
        }
    }
}
