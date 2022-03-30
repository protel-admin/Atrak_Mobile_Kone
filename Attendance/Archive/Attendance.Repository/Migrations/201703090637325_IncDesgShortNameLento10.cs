namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncDesgShortNameLento10 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Designation", "ShortName", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Designation", "ShortName", c => c.String(maxLength: 5));
        }
    }
}
