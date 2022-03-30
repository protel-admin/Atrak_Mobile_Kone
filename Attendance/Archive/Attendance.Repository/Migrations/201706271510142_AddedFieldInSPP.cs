namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldInSPP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShiftPostingPattern", "CreatedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.ShiftPostingPattern", "CreatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.ShiftPostingPattern", "ModifiedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.ShiftPostingPattern", "ModifiedBy", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShiftPostingPattern", "ModifiedBy");
            DropColumn("dbo.ShiftPostingPattern", "ModifiedOn");
            DropColumn("dbo.ShiftPostingPattern", "CreatedBy");
            DropColumn("dbo.ShiftPostingPattern", "CreatedOn");
        }
    }
}
