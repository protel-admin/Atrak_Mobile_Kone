namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldQualificationfromStaffOfficial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffPersonal", "Qualification", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffPersonal", "Qualification");
        }
    }
}
