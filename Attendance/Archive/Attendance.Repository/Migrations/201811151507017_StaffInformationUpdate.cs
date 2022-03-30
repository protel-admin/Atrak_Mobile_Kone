namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StaffInformationUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationApproval", "ReviewerstatusId", c => c.Int(nullable: false));
            AddColumn("dbo.ApplicationApproval", "ReviewedBy", c => c.String(maxLength: 20));
            AddColumn("dbo.ApplicationApproval", "ReviewedOn", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.ApplicationApproval", "ReviewerOwner", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.StaffOfficial", "MedicalClaimNumber", c => c.String(maxLength: 15));
            AddColumn("dbo.StaffOfficial", "Reviewer", c => c.String(maxLength: 20));
            AddColumn("dbo.StaffOfficial", "OTReviewer", c => c.String(maxLength: 20));
            AddColumn("dbo.StaffOfficial", "OTReportingManager", c => c.String(maxLength: 20));
            AddColumn("dbo.StaffPersonal", "PresentAddress", c => c.String(maxLength: 200));
            AddColumn("dbo.StaffPersonal", "EmergencyContactNumber", c => c.String(maxLength: 15));
            AddColumn("dbo.StaffPersonal", "EmergencyContactPerson", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StaffPersonal", "EmergencyContactPerson");
            DropColumn("dbo.StaffPersonal", "EmergencyContactNumber");
            DropColumn("dbo.StaffPersonal", "PresentAddress");
            DropColumn("dbo.StaffOfficial", "OTReportingManager");
            DropColumn("dbo.StaffOfficial", "OTReviewer");
            DropColumn("dbo.StaffOfficial", "Reviewer");
            DropColumn("dbo.StaffOfficial", "MedicalClaimNumber");
            DropColumn("dbo.ApplicationApproval", "ReviewerOwner");
            DropColumn("dbo.ApplicationApproval", "ReviewedOn");
            DropColumn("dbo.ApplicationApproval", "ReviewedBy");
            DropColumn("dbo.ApplicationApproval", "ReviewerstatusId");
        }
    }
}
