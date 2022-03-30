namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAdditionalFandAddFValueTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdditionalField",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScreenName = c.String(nullable: false),
                        ColumnName = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        Access = c.String(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        Createdby = c.String(nullable: false),
                        ModifiedOn = c.DateTime(storeType: "smalldatetime"),
                        Modifiedby = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdditionalFieldValue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Staffid = c.String(nullable: false),
                        AddfId = c.Int(nullable: false),
                        ActualValue = c.String(),
                        ModifiedOn = c.DateTime(storeType: "smalldatetime"),
                        Modifiedby = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AdditionalField", t => t.AddfId)
                .Index(t => t.AddfId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdditionalFieldValue", "AddfId", "dbo.AdditionalField");
            DropIndex("dbo.AdditionalFieldValue", new[] { "AddfId" });
            DropTable("dbo.AdditionalFieldValue");
            DropTable("dbo.AdditionalField");
        }
    }
}
