namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNewTableEmailForwardConfig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailForwardingconfig",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScreenID = c.String(nullable: false),
                        Fromaddress = c.String(nullable: false),
                        Toaddress = c.String(nullable: false),
                        CCaddress = c.String(nullable: false),
                        CreatedOn = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(),
                        ModifiedOn = c.DateTime(storeType: "smalldatetime"),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailForwardingconfig");
        }
    }
}
