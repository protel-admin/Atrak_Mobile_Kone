namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkStation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Workstation",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false, maxLength: 50),
                        ShortName = c.String(nullable: false, maxLength: 5),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkstationAllocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Staffid = c.String(nullable: false, maxLength: 20),
                        WorkstationId = c.String(nullable: false, maxLength: 10),
                        TransactionDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        CreatedOn = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Workstation", t => t.WorkstationId)
                .Index(t => t.WorkstationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkstationAllocation", "WorkstationId", "dbo.Workstation");
            DropIndex("dbo.WorkstationAllocation", new[] { "WorkstationId" });
            DropTable("dbo.WorkstationAllocation");
            DropTable("dbo.Workstation");
        }
    }
}
