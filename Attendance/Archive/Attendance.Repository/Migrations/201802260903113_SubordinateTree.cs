namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubordinateTree : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubordinateTree",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StaffId = c.String(nullable: false, maxLength: 20),
                        ReportingStaffId = c.String(nullable: false, maxLength: 20),
                        Signature = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SubordinateTree");
        }
    }
}
