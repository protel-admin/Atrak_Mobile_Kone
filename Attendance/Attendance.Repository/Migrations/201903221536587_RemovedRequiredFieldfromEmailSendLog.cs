namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRequiredFieldfromEmailSendLog : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EmailSendLog", "CC", c => c.String(maxLength: 1000));
            AlterColumn("dbo.EmailSendLog", "BCC", c => c.String(maxLength: 1000));
            AlterColumn("dbo.EmailSendLog", "ErrorDescription", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmailSendLog", "ErrorDescription", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.EmailSendLog", "BCC", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.EmailSendLog", "CC", c => c.String(nullable: false, maxLength: 1000));
        }
    }
}
