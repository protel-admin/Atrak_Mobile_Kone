namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForgotPassword : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AtrakUserDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChangeDetailsPW",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.String(),
                        UserName = c.String(),
                        OldPassword = c.String(),
                        NewPassword = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChangeDetailsPW");
            DropTable("dbo.AtrakUserDetails");
        }
    }
}
