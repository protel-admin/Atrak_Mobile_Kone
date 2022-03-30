namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterColumnRequestApplication15102020 : DbMigration
    {

        public override void Up()
        {
            AlterColumn("dbo.RequestApplication", "RequestApplicationType", c => c.String(nullable: false, maxLength: 5));
        }

        public override void Down()
        {
            AlterColumn("dbo.RequestApplication", "RequestApplicationType", c => c.String(nullable: false, maxLength: 2));
        }


    }
}
