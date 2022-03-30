namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTable_MobileSwipeTransactions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MobileSwipeTransactions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StaffId = c.String(nullable: false, maxLength: 50),
                        SwipeMode = c.String(nullable: false, maxLength: 20),
                        SwipeDateTime = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        Longitude = c.String(maxLength: 20),
                        Lattitude = c.String(maxLength: 20),
                        CreatedOn = c.DateTime(storeType: "smalldatetime"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MobileSwipeTransactions");
        }
    }
}
