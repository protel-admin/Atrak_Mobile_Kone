namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldInShifts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shifts", "CreatedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.Shifts", "CreatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.Shifts", "ModifiedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.Shifts", "ModifiedBy", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shifts", "ModifiedBy");
            DropColumn("dbo.Shifts", "ModifiedOn");
            DropColumn("dbo.Shifts", "CreatedBy");
            DropColumn("dbo.Shifts", "CreatedOn");
        }
    }
}
