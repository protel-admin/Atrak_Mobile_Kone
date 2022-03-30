using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository {
    public class StaffDrillDownRepository : IDisposable
    {
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }
            disposed = true;
        }
        AttendanceManagementContext context;
        public StaffDrillDownRepository()
        {
            context = new AttendanceManagementContext();
        }
        public List<DropDownModel> GetStaffStatusRepository()
        {
            List<DropDownModel> lst = new List<DropDownModel>();
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Id,Name from StaffStatus where IsActive=1 Union All Select '0' as Id,'ALL' as Name");
                lst = context.Database.SqlQuery<DropDownModel>(builder.ToString()).ToList();
                return lst;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<FilterList> LoadFilterList(string companyid , string branchid , string departmentid , 
                        string divisionid , string designationid , string gradeid , 
                        string categoryid , string costcentreid , string locationid,string volumeid,
                        string shortname)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("spFilterMasters");

            try
            {
                var lst = context.Database.SqlQuery<FilterList>("spFilterMasters @p0 , @p1 , @p2,@p3 , @p4 , @p5,@p6 , @p7 , @p8,@p9", companyid, branchid, departmentid, divisionid, designationid, gradeid, categoryid, costcentreid, locationid,volumeid).Where(d=>d.Type.Equals(shortname)).ToList();
                
                if (lst == null)
                {
                    return new List<FilterList>();
                }
                else
                {
                    return lst;
                }
            }
            catch
            {
                return new List<FilterList>();
            }
        }

        public List<DepartmentList> LoadCompanyWiseDepartment(string CompanyId)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT ID , NAME FROM DEPARTMENT WHERE ID IN "+
                "( SELECT DISTINCT DEPTID FROM STAFFVIEW WHERE COMPANYID = @CompanyId) Order By Name");

            try
            {
                var lst = context.Database.SqlQuery<DepartmentList>(qryStr.ToString(),new SqlParameter("@CompanyId", CompanyId)).Select(d => new DepartmentList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<DepartmentList>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<DepartmentList>();
            }
        }

        public List<CompanyList> GetCompanyList( )
        {
            var qryStr = new StringBuilder();
            qryStr.Append("select '0'  as id , '-- Select Company --' as Name union select id , SHORTname as Name from company");
            var lst = context.Database.SqlQuery<CompanyList>(qryStr.ToString()).Select(d => new CompanyList()
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();

            if (lst.Count == 0){
                return new List<CompanyList>();}
            else{
                return lst;}
        }

        public List<BranchList> GetBranchList( ) {
            var qryStr = new StringBuilder ( );
            qryStr.Append ( "select '0'  as id , '-- Select Branch --' as name  union select id , name from branch" );
            var lst = context.Database.SqlQuery<BranchList> ( qryStr.ToString ( ) ).Select ( d => new BranchList ( ) {
                Id = d.Id ,
                Name = d.Name
            } ).ToList ( );

            if ( lst.Count == 0 ) {
                return new List<BranchList> ( );
            } else {
                return lst;
            }
        }

        public List<DepartmentList> GetDepartmentList( ) {
            var qryStr = new StringBuilder ( );
            qryStr.Append ( "select '0'  as id , '-- Select Department --' as name  union select id , name from Department Order By Name Asc" );
            var lst = context.Database.SqlQuery<DepartmentList> ( qryStr.ToString ( ) ).Select ( d => new DepartmentList ( ) {
                Id = d.Id ,
                Name = d.Name
            } ).ToList ( );

            if ( lst.Count == 0 ) {
                return new List<DepartmentList> ( );
            } else {
                return lst;
            }
        }

        public List<DivisionList> GetDivisionList( ) {
            var qryStr = new StringBuilder ( );
            qryStr.Append ( "select '0'  as id , '-- Select Division --' as name  union select id , name from Division Order By Name Asc" );
            var lst = context.Database.SqlQuery<DivisionList> ( qryStr.ToString ( ) ).Select ( d => new DivisionList ( ) {
                Id = d.Id ,
                Name = d.Name
            } ).ToList ( );

            if ( lst.Count == 0 ) {
                return new List<DivisionList> ( );
            } else {
                return lst;
            }
        }

        public List<DesignationList> GetDsignationList( ) {
            var qryStr = new StringBuilder ( );
            qryStr.Append ( "select '0'  as id , '-- Select Designation --' as name  union select id , name from Designation Order By Name Asc" );
            var lst = context.Database.SqlQuery<DesignationList> ( qryStr.ToString ( ) ).Select ( d => new DesignationList ( ) {
                Id = d.Id ,
                Name = d.Name
            } ).ToList ( );

            if ( lst.Count == 0 ) {
                return new List<DesignationList> ( );
            } else {
                return lst;
            }
        }

        public List<GradeList> GetGradeList( ) {
            var qryStr = new StringBuilder ( );
            qryStr.Append ( "select '0'  as id , '-- Select Grade --' as name  union select id , name from Garde order by Name Asc" );
            var lst = context.Database.SqlQuery<GradeList> ( qryStr.ToString ( ) ).Select ( d => new GradeList ( ) {
                Id = d.Id ,
                Name = d.Name
            } ).ToList ( );

            if ( lst.Count == 0 ) {
                return new List<GradeList> ( );
            } else {
                return lst;
            }
        }

        public List<CategoryList> GetCategoryList()
        {
            var qryStr = new StringBuilder();
            qryStr.Append("select '0'  as id , '-- Select Category --' as name  union select id , name from Category Order By Name Asc");
            var lst = context.Database.SqlQuery<CategoryList>(qryStr.ToString()).Select(d => new CategoryList()
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();

            if (lst.Count == 0)
            {
                return new List<CategoryList>();
            }
            else
            {
                return lst;
            }
        }

        public List<CostCentreList> GetCostCentreList()
        {
            var qryStr = new StringBuilder();
            qryStr.Append("select '0'  as id , '-- Select Cost Centre --' as name  union select id , name from CostCentre");
            var lst = context.Database.SqlQuery<CostCentreList>(qryStr.ToString()).Select(d => new CostCentreList()
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();

            if (lst.Count == 0)
            {
                return new List<CostCentreList>();
            }
            else
            {
                return lst;
            }
        }

        public List<LocationList> GetLocationList()
        {
            var qryStr = new StringBuilder();
            qryStr.Append(" ");
            var lst = context.Database.SqlQuery<LocationList>(qryStr.ToString()).Select(d => new LocationList()
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();

            if (lst.Count == 0)
            {
                return new List<LocationList>();
            }
            else
            {
                return lst;
            }
        }

        public List<VolumeList> GetVolumeList()
        {
            var qryStr = new StringBuilder();
            qryStr.Append("select '0'  as id , '-- Select Volume --' as name  union select id , name from Volume");
            var lst = context.Database.SqlQuery<VolumeList>(qryStr.ToString()).Select(d => new VolumeList()
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();

            if (lst.Count == 0)
            {
                return new List<VolumeList>();
            }
            else
            {
                return lst;
            }
        }

        public List<EmployeeGroupView> GetEmployeeGroupList()
        {
            var qryStr = new StringBuilder();
            qryStr.Append("select '0'  as id , '-- Select Employee Group --' as name  union select Id , Name from vwemployeegroup a "+
                "inner join EmployeeGroup b on a.employeegroupid = b.id");
            var lst = context.Database.SqlQuery<EmployeeGroupView>(qryStr.ToString()).Select(d => new EmployeeGroupView()
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();

            if (lst.Count == 0)
            {
                return new List<EmployeeGroupView>();
            }
            else
            {
                return lst;
            }
        }

        public List<ShiftView> GetShiftList()
        {
            var qryStr = new StringBuilder();
            qryStr.Append("Select '0'  as id , '-- Select Shift --' as name  union select id , name from Shifts ");
            var lst = context.Database.SqlQuery<ShiftView>(qryStr.ToString()).Select(d => new ShiftView()
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();

            if (lst.Count == 0)
            {
                return new List<ShiftView>();
            }
            else
            {
                return lst;
            }
        }
    }
}
