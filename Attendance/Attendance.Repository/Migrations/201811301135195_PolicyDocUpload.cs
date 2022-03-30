namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PolicyDocUpload : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PolicyDocUpload",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PolicyName = c.String(nullable: false),
                        FileType = c.String(nullable: false),
                        FileExtension = c.Binary(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        Createdby = c.String(),
                        isCancelled = c.Boolean(nullable: false),
                        CancelledOn = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        CancelledBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PolicyDocUpload");
        }
    }
}
