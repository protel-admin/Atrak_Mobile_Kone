namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTableStaffEditRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StaffEditRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestId = c.String(nullable: false, maxLength: 10),
                        UserId = c.String(nullable: false, maxLength: 10),
                        Staff = c.String(),
                        StaffOfficial = c.String(),
                        StaffPersonal = c.String(),
                        Createdon = c.String(nullable: false),
                        Createdby = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StaffEditRequest");
        }
    }
}
