namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldLocationIdfromRuleGroupTxn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RuleGroupTxn", "LocationID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RuleGroupTxn", "LocationID");
        }
    }
}
