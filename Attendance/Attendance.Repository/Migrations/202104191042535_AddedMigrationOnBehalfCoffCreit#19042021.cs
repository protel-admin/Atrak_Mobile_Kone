namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMigrationOnBehalfCoffCreit19042021 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CoffReq",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 10),
                        Staffid = c.String(nullable: false, maxLength: 50),
                        CoffReqFrom = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        CoffReqTo = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        TotalDays = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Reason = c.String(nullable: false, maxLength: 200),
                        IsCancelled = c.Boolean(nullable: false),
                        ExpiryDate = c.DateTime(storeType: "smalldatetime"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Staff", t => t.Staffid)
                .Index(t => t.Staffid);
            
            AddColumn("dbo.EmployeeLeaveAccount", "IsLapsed", c => c.Boolean(nullable: false));
            AddColumn("dbo.EmployeeLeaveAccount", "TransactionBy", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CoffReq", "Staffid", "dbo.Staff");
            DropIndex("dbo.CoffReq", new[] { "Staffid" });
            DropColumn("dbo.EmployeeLeaveAccount", "TransactionBy");
            DropColumn("dbo.EmployeeLeaveAccount", "IsLapsed");
            DropTable("dbo.CoffReq");
        }
    }
}
