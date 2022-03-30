namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changing_data_type : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AlternativePersonAssign");
            AlterColumn("dbo.AlternativePersonAssign", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.AlternativePersonAssign", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AlternativePersonAssign");
            AlterColumn("dbo.AlternativePersonAssign", "Id", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.AlternativePersonAssign", "Id");
        }
    }
}
