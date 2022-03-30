namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameColDeptIdToStaffIdInTeamHierachy : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TeamHierarchy", "DepartmentId", "dbo.Department");
            DropIndex("dbo.TeamHierarchy", new[] { "DepartmentId" });
            AddColumn("dbo.StaffOfficial", "Interimhike", c => c.Boolean(nullable: false));
            AddColumn("dbo.StaffOfficial", "Tenure", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.StaffOfficial", "DateOfRelieving", c => c.DateTime(storeType: "smalldatetime"));
            AddColumn("dbo.TeamHierarchy", "StaffId", c => c.String(nullable: false, maxLength: 10));
            CreateIndex("dbo.TeamHierarchy", "StaffId");
            AddForeignKey("dbo.TeamHierarchy", "StaffId", "dbo.Staff", "Id");
            DropColumn("dbo.TeamHierarchy", "DepartmentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TeamHierarchy", "DepartmentId", c => c.String(nullable: false, maxLength: 10));
            DropForeignKey("dbo.TeamHierarchy", "StaffId", "dbo.Staff");
            DropIndex("dbo.TeamHierarchy", new[] { "StaffId" });
            DropColumn("dbo.TeamHierarchy", "StaffId");
            DropColumn("dbo.StaffOfficial", "DateOfRelieving");
            DropColumn("dbo.StaffOfficial", "Tenure");
            DropColumn("dbo.StaffOfficial", "Interimhike");
            CreateIndex("dbo.TeamHierarchy", "DepartmentId");
            AddForeignKey("dbo.TeamHierarchy", "DepartmentId", "dbo.Department", "Id");
        }
    }
}
