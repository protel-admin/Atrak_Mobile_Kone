// <auto-generated />
namespace Attendance.Repository.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class RenameColDeptIdToStaffIdInTeamHierachy : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(RenameColDeptIdToStaffIdInTeamHierachy));
        
        string IMigrationMetadata.Id
        {
            get { return "201708231342526_RenameColDeptIdToStaffIdInTeamHierachy"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
