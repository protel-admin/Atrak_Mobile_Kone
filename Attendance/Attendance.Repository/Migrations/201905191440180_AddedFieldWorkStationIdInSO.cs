namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldWorkStationIdInSO : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffOfficial", "WorkStationId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffOfficial", "WorkStationId");
        }
    }
}
