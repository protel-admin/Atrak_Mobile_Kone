namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLocationColumninTableEmailForwardConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmailForwardingconfig", "LocationId", c => c.String(maxLength: 10));
            CreateIndex("dbo.EmailForwardingconfig", "LocationId");
            AddForeignKey("dbo.EmailForwardingconfig", "LocationId", "dbo.Location", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmailForwardingconfig", "LocationId", "dbo.Location");
            DropIndex("dbo.EmailForwardingconfig", new[] { "LocationId" });
            DropColumn("dbo.EmailForwardingconfig", "LocationId");
        }
    }
}
