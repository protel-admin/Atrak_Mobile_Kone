namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEmployeePhotoTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeePhoto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.String(nullable: false),
                        EmpPhoto = c.Binary(nullable: false),
                        CreatedOn = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(),
                        ModifiedOn = c.DateTime(storeType: "smalldatetime"),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmployeePhoto");
        }
    }
}
