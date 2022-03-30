namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Leavetype_ShiftLc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlternativePersonAssign",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 20),
                        ParentId = c.String(maxLength: 20),
                        StaffId = c.String(nullable: false, maxLength: 20),
                        AlternativeStaffId = c.String(nullable: false, maxLength: 20),
                        IsCancelled = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        IntimationMailSent = c.Boolean(nullable: false),
                        ConfirmationMailSent = c.Boolean(nullable: false),
                        CancellationMailSent = c.Boolean(nullable: false),
                        RejectMailSent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequestApplication", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.LeaveTypeMaster",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 6),
                        Name = c.String(nullable: false, maxLength: 100),
                        ShortName = c.String(nullable: false, maxLength: 50),
                        LeaveType = c.String(nullable: false, maxLength: 20),
                        PaidLeave = c.Boolean(nullable: false),
                        Accountable = c.Boolean(nullable: false),
                        CarryForward = c.Boolean(nullable: false),
                        MaxAccDays = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxAccYears = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxDaysPerReq = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ElgInMonths = c.Int(nullable: false),
                        IsCalcToWorkingDays = c.Boolean(nullable: false),
                        CalcToWorkingDays = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ConsiderWO = c.Boolean(nullable: false),
                        ConsiderPH = c.Boolean(nullable: false),
                        IsExcessEligibleAllowed = c.Boolean(nullable: false),
                        IsEnCashmentAllowed = c.Boolean(nullable: false),
                        EncashmentLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditFreq = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditDays = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProRata = c.Boolean(nullable: false),
                        RoundOffTo = c.String(),
                        RoundOffValue = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(),
                        ModifiedOn = c.DateTime(storeType: "smalldatetime"),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Shifts", "LocationId", c => c.String(maxLength: 10));
            CreateIndex("dbo.Shifts", "LocationId");
            AddForeignKey("dbo.Shifts", "LocationId", "dbo.Location", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shifts", "LocationId", "dbo.Location");
            DropForeignKey("dbo.AlternativePersonAssign", "ParentId", "dbo.RequestApplication");
            DropIndex("dbo.Shifts", new[] { "LocationId" });
            DropIndex("dbo.AlternativePersonAssign", new[] { "ParentId" });
            DropColumn("dbo.Shifts", "LocationId");
            DropTable("dbo.LeaveTypeMaster");
            DropTable("dbo.AlternativePersonAssign");
        }
    }
}
