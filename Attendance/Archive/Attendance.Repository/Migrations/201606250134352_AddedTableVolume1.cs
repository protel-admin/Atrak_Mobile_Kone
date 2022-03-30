namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTableVolume1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Volume",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 10),
                        PeopleSoftCode = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false, maxLength: 50),
                        ShortName = c.String(nullable: false, maxLength: 5),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(storeType: "smalldatetime"),
                        CreatedBy = c.String(),
                        MOdifiedOn = c.DateTime(storeType: "smalldatetime"),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Volume");
        }
    }
}
