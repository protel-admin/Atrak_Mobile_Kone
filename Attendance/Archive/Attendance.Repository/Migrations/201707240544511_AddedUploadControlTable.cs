namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUploadControlTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UploadControlTable",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Filename = c.String(maxLength: 100),
                        TypeOfData = c.String(nullable: false, maxLength: 5),
                        UploadedOn = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        UploadedBy = c.String(nullable: false, maxLength: 50),
                        IsProcessed = c.Boolean(nullable: false),
                        ProcessedOn = c.DateTime(storeType: "smalldatetime"),
                        ProcessStatus = c.String(maxLength: 20),
                        IsError = c.Boolean(nullable: false),
                        ErrorMessage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UploadControlTable");
        }
    }
}
