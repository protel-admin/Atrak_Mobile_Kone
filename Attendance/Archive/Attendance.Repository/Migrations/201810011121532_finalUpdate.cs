namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finalUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AlternativePersonAssign", "ParentId", "dbo.RequestApplication");
            DropIndex("dbo.AlternativePersonAssign", new[] { "ParentId" });
            AlterColumn("dbo.AlternativePersonAssign", "ParentId", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AlternativePersonAssign", "ParentId", c => c.String(maxLength: 20));
            CreateIndex("dbo.AlternativePersonAssign", "ParentId");
            AddForeignKey("dbo.AlternativePersonAssign", "ParentId", "dbo.RequestApplication", "Id");
        }
    }
}
