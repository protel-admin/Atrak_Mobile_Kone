namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldInShiftPattern : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShiftPattern", "CreatedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.ShiftPattern", "CreatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.ShiftPattern", "ModifiedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.ShiftPattern", "ModifiedBy", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShiftPattern", "ModifiedBy");
            DropColumn("dbo.ShiftPattern", "ModifiedOn");
            DropColumn("dbo.ShiftPattern", "CreatedBy");
            DropColumn("dbo.ShiftPattern", "CreatedOn");
        }
    }
}
