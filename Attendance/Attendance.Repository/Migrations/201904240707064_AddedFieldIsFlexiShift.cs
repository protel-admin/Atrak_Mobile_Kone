namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldIsFlexiShift : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StaffOfficial", "IsFlexi", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffOfficial", "IsFlexi");
        }
    }
}
