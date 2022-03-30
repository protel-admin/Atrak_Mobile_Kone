namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFieldUsedAsGSInShiftPattern : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShiftPattern", "UsedAsGeneralShift", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShiftPattern", "UsedAsGeneralShift");
        }
    }
}
