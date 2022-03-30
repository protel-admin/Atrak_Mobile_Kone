namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedColumnInStaffEditRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffEditRequest", "AdditionalFieldValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffEditRequest", "AdditionalFieldValue");
        }
    }
}
