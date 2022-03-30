namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTableCompensatoryWorking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompensatoryWorking",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeaveDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        CompensatoryWorkingDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        Reason = c.String(maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CompensatoryWorking");
        }
    }
}
