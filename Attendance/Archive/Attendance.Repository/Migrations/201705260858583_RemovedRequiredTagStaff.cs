namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRequiredTagStaff : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Staff", "PeopleSoftCode", c => c.String(maxLength: 10));
            AlterColumn("dbo.Staff", "CardCode", c => c.String(maxLength: 8));
            AlterColumn("dbo.Staff", "ShortName", c => c.String(maxLength: 20));
            AlterColumn("dbo.Staff", "Gender", c => c.String(maxLength: 1));
            AlterColumn("dbo.StaffEducation", "CourseName", c => c.String(maxLength: 50));
            AlterColumn("dbo.StaffEducation", "University", c => c.String(maxLength: 50));
            AlterColumn("dbo.StaffEducation", "Grade", c => c.String(maxLength: 5));
            AlterColumn("dbo.StaffFamily", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.StaffOfficial", "DateOfJoining", c => c.DateTime(storeType: "smalldatetime"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StaffOfficial", "DateOfJoining", c => c.DateTime(nullable: false, storeType: "smalldatetime"));
            AlterColumn("dbo.StaffFamily", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.StaffEducation", "Grade", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.StaffEducation", "University", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.StaffEducation", "CourseName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Staff", "Gender", c => c.String(nullable: false, maxLength: 1));
            AlterColumn("dbo.Staff", "ShortName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Staff", "CardCode", c => c.String(nullable: false, maxLength: 8));
            AlterColumn("dbo.Staff", "PeopleSoftCode", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
