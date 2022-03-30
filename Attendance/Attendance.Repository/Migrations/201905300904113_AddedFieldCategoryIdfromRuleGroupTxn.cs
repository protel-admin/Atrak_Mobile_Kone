namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldCategoryIdfromRuleGroupTxn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RuleGroupTxn", "CategoryId", c => c.String(maxLength: 10));
            CreateIndex("dbo.RuleGroupTxn", "CategoryId");
            AddForeignKey("dbo.RuleGroupTxn", "CategoryId", "dbo.Category", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RuleGroupTxn", "CategoryId", "dbo.Category");
            DropIndex("dbo.RuleGroupTxn", new[] { "CategoryId" });
            DropColumn("dbo.RuleGroupTxn", "CategoryId");
        }
    }
}
