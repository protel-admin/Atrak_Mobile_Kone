namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RuleGroup_LocationId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RuleGroupTxn", "LocationID", c => c.String(maxLength: 10));
            CreateIndex("dbo.RuleGroupTxn", "LocationID");
            AddForeignKey("dbo.RuleGroupTxn", "LocationID", "dbo.Location", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RuleGroupTxn", "LocationID", "dbo.Location");
            DropIndex("dbo.RuleGroupTxn", new[] { "LocationID" });
            DropColumn("dbo.RuleGroupTxn", "LocationID");
        }
    }
}
