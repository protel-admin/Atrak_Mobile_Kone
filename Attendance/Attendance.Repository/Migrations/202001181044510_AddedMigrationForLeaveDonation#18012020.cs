namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMigrationForLeaveDonation18012020 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RALeaveDonation", "LeaveTypeId", "dbo.LeaveType");
            DropForeignKey("dbo.RALeaveDonation", "DonarStaffID", "dbo.Staff");
            DropForeignKey("dbo.RALeaveDonation", "ReceiverStaffID", "dbo.Staff");
            DropIndex("dbo.RALeaveDonation", new[] { "LeaveTypeId" });
            DropIndex("dbo.RALeaveDonation", new[] { "DonarStaffID" });
            DropIndex("dbo.RALeaveDonation", new[] { "ReceiverStaffID" });
            DropTable("dbo.RALeaveDonation");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RALeaveDonation",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 10),
                        LeaveTypeId = c.String(maxLength: 10),
                        IsCancelled = c.Boolean(nullable: false),
                        CancelledOn = c.String(),
                        CancelledBy = c.String(),
                        DonarStaffID = c.String(maxLength: 50),
                        ReceiverStaffID = c.String(maxLength: 50),
                        TransactionDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        LeaveCount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Narration = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.RALeaveDonation", "ReceiverStaffID");
            CreateIndex("dbo.RALeaveDonation", "DonarStaffID");
            CreateIndex("dbo.RALeaveDonation", "LeaveTypeId");
            AddForeignKey("dbo.RALeaveDonation", "ReceiverStaffID", "dbo.Staff", "Id");
            AddForeignKey("dbo.RALeaveDonation", "DonarStaffID", "dbo.Staff", "Id");
            AddForeignKey("dbo.RALeaveDonation", "LeaveTypeId", "dbo.LeaveType", "Id");
        }
    }
}
