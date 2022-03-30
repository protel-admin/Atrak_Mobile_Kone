namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropingtable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.AlternativePersonAssign");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AlternativePersonAssign",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.String(nullable: false, maxLength: 20),
                        StaffId = c.String(nullable: false, maxLength: 20),
                        AlternativeStaffId = c.String(nullable: false, maxLength: 20),
                        IsCancelled = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsRejected = c.Boolean(nullable: false),
                        IntimationMailSent = c.Boolean(nullable: false),
                        ConfirmationMailSent = c.Boolean(nullable: false),
                        CancellationMailSent = c.Boolean(nullable: false),
                        RejectMailSent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
