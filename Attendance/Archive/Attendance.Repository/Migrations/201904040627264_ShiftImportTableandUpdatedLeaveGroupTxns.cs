namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShiftImportTableandUpdatedLeaveGroupTxns : DbMigration
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
            
            AddColumn("dbo.LeaveGroupTxn", "PaidLeave", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "Accountable", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "CarryForward", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "MaxAccDays", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LeaveGroupTxn", "MaxAccYears", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LeaveGroupTxn", "MaxDaysPerReq", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LeaveGroupTxn", "MinDaysPerReq", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LeaveGroupTxn", "ElgInMonths", c => c.Int(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "IsCalcToWorkingDays", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "CalcToWorkingDays", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LeaveGroupTxn", "ConsiderWO", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "ConsiderPH", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "IsExcessEligibleAllowed", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "IsEnCashmentAllowed", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "EncashmentLimit", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LeaveGroupTxn", "CreditFreq", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LeaveGroupTxn", "CreditDays", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LeaveGroupTxn", "ProRata", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "RoundOffTo", c => c.String());
            AddColumn("dbo.LeaveGroupTxn", "RoundOffValue", c => c.Int(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "CreatedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.LeaveGroupTxn", "CreatedBy", c => c.String());
            AddColumn("dbo.LeaveGroupTxn", "ModifiedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.LeaveGroupTxn", "ModifiedBy", c => c.String());
            AddColumn("dbo.LeaveGroupTxn", "LeavePayType", c => c.String());
            AddColumn("dbo.LeaveGroupTxn", "IsHalfDayApplicable", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "CheckBalance", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "ComponentId", c => c.Int(nullable: false));
            AddColumn("dbo.LeaveGroupTxn", "LCAFor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LeaveGroupTxn", "LCAFor");
            DropColumn("dbo.LeaveGroupTxn", "ComponentId");
            DropColumn("dbo.LeaveGroupTxn", "CheckBalance");
            DropColumn("dbo.LeaveGroupTxn", "IsHalfDayApplicable");
            DropColumn("dbo.LeaveGroupTxn", "LeavePayType");
            DropColumn("dbo.LeaveGroupTxn", "ModifiedBy");
            DropColumn("dbo.LeaveGroupTxn", "ModifiedOn");
            DropColumn("dbo.LeaveGroupTxn", "CreatedBy");
            DropColumn("dbo.LeaveGroupTxn", "CreatedOn");
            DropColumn("dbo.LeaveGroupTxn", "RoundOffValue");
            DropColumn("dbo.LeaveGroupTxn", "RoundOffTo");
            DropColumn("dbo.LeaveGroupTxn", "ProRata");
            DropColumn("dbo.LeaveGroupTxn", "CreditDays");
            DropColumn("dbo.LeaveGroupTxn", "CreditFreq");
            DropColumn("dbo.LeaveGroupTxn", "EncashmentLimit");
            DropColumn("dbo.LeaveGroupTxn", "IsEnCashmentAllowed");
            DropColumn("dbo.LeaveGroupTxn", "IsExcessEligibleAllowed");
            DropColumn("dbo.LeaveGroupTxn", "ConsiderPH");
            DropColumn("dbo.LeaveGroupTxn", "ConsiderWO");
            DropColumn("dbo.LeaveGroupTxn", "CalcToWorkingDays");
            DropColumn("dbo.LeaveGroupTxn", "IsCalcToWorkingDays");
            DropColumn("dbo.LeaveGroupTxn", "ElgInMonths");
            DropColumn("dbo.LeaveGroupTxn", "MinDaysPerReq");
            DropColumn("dbo.LeaveGroupTxn", "MaxDaysPerReq");
            DropColumn("dbo.LeaveGroupTxn", "MaxAccYears");
            DropColumn("dbo.LeaveGroupTxn", "MaxAccDays");
            DropColumn("dbo.LeaveGroupTxn", "CarryForward");
            DropColumn("dbo.LeaveGroupTxn", "Accountable");
            DropColumn("dbo.LeaveGroupTxn", "PaidLeave");
            DropTable("dbo.ShiftsImportData");
        }
    }
}
