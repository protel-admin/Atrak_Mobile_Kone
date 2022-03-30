namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFieldVolumeIdinSO : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StaffOfficial", "VolumeId", c => c.String(maxLength: 10));
            CreateIndex("dbo.StaffOfficial", "VolumeId");
            AddForeignKey("dbo.StaffOfficial", "VolumeId", "dbo.Volume", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StaffOfficial", "VolumeId", "dbo.Volume");
            DropIndex("dbo.StaffOfficial", new[] { "VolumeId" });
            AlterColumn("dbo.StaffOfficial", "VolumeId", c => c.String());
        }
    }
}
