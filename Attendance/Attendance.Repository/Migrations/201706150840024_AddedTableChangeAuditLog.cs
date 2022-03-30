namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTableChangeAuditLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangeAuditLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 10),
                        ChangeLog = c.String(nullable: false),
                        ActionType = c.String(nullable: false, maxLength: 6),
                        TableName = c.String(nullable: false, maxLength: 100),
                        PrimaryKeyValue = c.String(nullable: false, maxLength: 10),
                        CreatedOn = c.DateTime(nullable: false, storeType: "smalldatetime"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChangeAuditLog");
        }
    }
}
