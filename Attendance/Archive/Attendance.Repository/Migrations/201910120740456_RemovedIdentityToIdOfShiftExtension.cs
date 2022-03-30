namespace Attendance.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedIdentityToIdOfShiftExtension : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ShiftExtensionAndDoubleShift");
            AlterColumn("dbo.ShiftExtensionAndDoubleShift", "Id", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.ShiftExtensionAndDoubleShift", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ShiftExtensionAndDoubleShift");
            AlterColumn("dbo.ShiftExtensionAndDoubleShift", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ShiftExtensionAndDoubleShift", "Id");
        }
    }
}
