namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTableShiftPostingPattern : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShiftPostingPattern",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PatternId = c.Int(nullable: false),
                        Sunday = c.String(),
                        Monday = c.String(),
                        Tuesday = c.String(),
                        Wednesday = c.String(),
                        Thursday = c.String(),
                        Friday = c.String(),
                        Saturday = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ShiftPostingPattern");
        }
    }
}
