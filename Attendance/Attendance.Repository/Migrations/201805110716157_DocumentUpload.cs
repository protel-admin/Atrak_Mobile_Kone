namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentUpload : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentUpload",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.String(maxLength: 20),
                        FileContent = c.Binary(),
                        IsActive = c.Boolean(nullable: false),
                        TypeOfDocument = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequestApplication", t => t.ParentId)
                .Index(t => t.ParentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentUpload", "ParentId", "dbo.RequestApplication");
            DropIndex("dbo.DocumentUpload", new[] { "ParentId" });
            DropTable("dbo.DocumentUpload");
        }
    }
}
