namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnabledIdentity : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ShiftPostingPattern");
            AlterColumn("dbo.ShiftPostingPattern", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ShiftPostingPattern", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ShiftPostingPattern");
            AlterColumn("dbo.ShiftPostingPattern", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ShiftPostingPattern", "Id");
        }
    }
}
