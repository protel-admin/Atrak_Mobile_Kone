namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldVolumeIdInStaffOfficial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffOfficial", "VolumeId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffOfficial", "VolumeId");
        }
    }
}
