namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTableTesting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Testing",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 20),
                        StaffId = c.String(maxLength: 20),
                        StaffAge = c.String(maxLength: 2),
                        PhoneNo = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Testing");
        }
    }
}
