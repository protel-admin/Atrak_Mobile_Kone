namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnSAPShiftNameInShiftTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shifts", "SAPShiftName", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shifts", "SAPShiftName");
        }
    }
}
