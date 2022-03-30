namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInDTypeOfCreatedOn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StaffEditRequest", "Createdon", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StaffEditRequest", "Createdon", c => c.String(nullable: false));
        }
    }
}
