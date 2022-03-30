namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RolesandResponsibilities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RolesAndResponsibilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.String(nullable: false, maxLength: 20),
                        Roles = c.String(),
                        Responsibilities = c.String(),
                        CreatedOn = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(),
                        ModifiedOn = c.DateTime(storeType: "smalldatetime"),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RolesAndResponsibilities");
        }
    }
}
