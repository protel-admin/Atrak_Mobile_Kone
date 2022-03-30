namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSomeFieldFromRuleGroupTxn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RuleGroupTxn", "CreatedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.RuleGroupTxn", "CreatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.RuleGroupTxn", "ModifiedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.RuleGroupTxn", "ModifiedBy", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RuleGroupTxn", "ModifiedBy");
            DropColumn("dbo.RuleGroupTxn", "ModifiedOn");
            DropColumn("dbo.RuleGroupTxn", "CreatedBy");
            DropColumn("dbo.RuleGroupTxn", "CreatedOn");
        }
    }
}
