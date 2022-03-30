namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedField_IsMobileAppEligible_In_StaffOfficial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffOfficial", "IsMobileAppEligible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffOfficial", "IsMobileAppEligible");
        }
    }
}
