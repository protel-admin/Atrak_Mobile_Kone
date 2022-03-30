namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmergencyDetailsinstaffpersonal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffPersonal", "EmergencyContactPerson1", c => c.String(maxLength: 150));
            AddColumn("dbo.StaffPersonal", "EmergencyContactPerson2", c => c.String(maxLength: 150));
            AddColumn("dbo.StaffPersonal", "EmergencyContactNo1", c => c.String(maxLength: 12));
            AddColumn("dbo.StaffPersonal", "EmergencyContactNo2", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffPersonal", "EmergencyContactNo2");
            DropColumn("dbo.StaffPersonal", "EmergencyContactNo1");
            DropColumn("dbo.StaffPersonal", "EmergencyContactPerson2");
            DropColumn("dbo.StaffPersonal", "EmergencyContactPerson1");
        }
    }
}
