namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLateComingNewTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.LateComing", newName: "LateComingNew");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.LateComingNew", newName: "LateComing");
        }
    }
}
