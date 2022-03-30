namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRolesAndResponsibilitiestablemaxlength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RolesAndResponsibilities", "Authorities", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RolesAndResponsibilities", "Authorities", c => c.String(maxLength: 500));
        }
    }
}
