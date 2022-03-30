namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMigrationOnDocumentUpload14072021 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentUpload", "CreatedOn", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            AddColumn("dbo.DocumentUpload", "CreatedBy", c => c.String());
            AddColumn("dbo.DocumentUpload", "IsApplicableForUserView", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentUpload", "IsApplicableForUserView");
            DropColumn("dbo.DocumentUpload", "CreatedBy");
            DropColumn("dbo.DocumentUpload", "CreatedOn");
        }
    }
}
