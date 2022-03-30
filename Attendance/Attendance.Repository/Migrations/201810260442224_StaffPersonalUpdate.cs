namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StaffPersonalUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffPersonal", "FatherName", c => c.String(maxLength: 100));
            AddColumn("dbo.StaffPersonal", "MotherName", c => c.String(maxLength: 100));
            AddColumn("dbo.StaffPersonal", "FatherAadharNo", c => c.String(maxLength: 12));
            AddColumn("dbo.StaffPersonal", "MotherAadharNo", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffPersonal", "MotherAadharNo");
            DropColumn("dbo.StaffPersonal", "FatherAadharNo");
            DropColumn("dbo.StaffPersonal", "MotherName");
            DropColumn("dbo.StaffPersonal", "FatherName");
        }
    }
}
