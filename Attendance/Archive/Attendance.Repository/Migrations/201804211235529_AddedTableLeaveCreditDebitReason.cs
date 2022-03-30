namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTableLeaveCreditDebitReason : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LeaveCreditDebitReason",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        ShortName = c.String(nullable: false, maxLength: 5),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.EmployeeLeaveAccount", "FinancialYearStart", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.EmployeeLeaveAccount", "FinancialYearEnd", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.EmployeeLeaveAccount", "LeaveCreditDebitReasonId", c => c.Int(nullable: false));
            AddColumn("dbo.EmployeeLeaveAccount", "TransctionBy", c => c.String());
            AddColumn("dbo.EmployeeLeaveAccount", "IsSystemAction", c => c.String());
            CreateIndex("dbo.EmployeeLeaveAccount", "LeaveCreditDebitReasonId");
            AddForeignKey("dbo.EmployeeLeaveAccount", "LeaveCreditDebitReasonId", "dbo.LeaveCreditDebitReason", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeLeaveAccount", "LeaveCreditDebitReasonId", "dbo.LeaveCreditDebitReason");
            DropIndex("dbo.EmployeeLeaveAccount", new[] { "LeaveCreditDebitReasonId" });
            DropColumn("dbo.EmployeeLeaveAccount", "IsSystemAction");
            DropColumn("dbo.EmployeeLeaveAccount", "TransctionBy");
            DropColumn("dbo.EmployeeLeaveAccount", "LeaveCreditDebitReasonId");
            DropColumn("dbo.EmployeeLeaveAccount", "FinancialYearEnd");
            DropColumn("dbo.EmployeeLeaveAccount", "FinancialYearStart");
            DropTable("dbo.LeaveCreditDebitReason");
        }
    }
}
