namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedReqFieldfromEmailSendLog : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EmailSendLog", "From", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmailSendLog", "From", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
