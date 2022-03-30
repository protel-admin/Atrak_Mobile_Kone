namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedColumnLateAndEarlyConfirmationOnStaffOfficial06082021 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffOfficial", "IsEarlyConfirmation", c => c.Boolean(nullable: false));
            AddColumn("dbo.StaffOfficial", "IsLateConfirmation", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffOfficial", "IsLateConfirmation");
            DropColumn("dbo.StaffOfficial", "IsEarlyConfirmation");
        }
    }
}
