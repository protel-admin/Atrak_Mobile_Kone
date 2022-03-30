namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldAadharNoInStaffPer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffPersonal", "AadharNo", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffPersonal", "AadharNo");
        }
    }
}
