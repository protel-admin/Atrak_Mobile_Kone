namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedLastNameRequiredFieldfromStaff : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Staff", "LastName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Staff", "LastName", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
