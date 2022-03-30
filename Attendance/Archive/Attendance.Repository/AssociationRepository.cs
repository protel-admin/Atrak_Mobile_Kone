using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using System.Data.Entity.Migrations;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository
{
    public class AssociationRepository : IDisposable
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
    
        AttendanceManagementContext context = null;
        public AssociationRepository()
        {
            context = new AttendanceManagementContext();
        }

        public void SaveInformation(string id, string combination, string gender, string workingdaypattern, string priority, string parentid, string parenttype, string isactive)
        {
            Association asc = new Association();

            if(string.IsNullOrEmpty(id) == false)
                asc.Id = Convert.ToInt16(id);

            asc.Combination = combination;
            asc.Gender = gender;
            if (string.IsNullOrEmpty(workingdaypattern) == true)
                workingdaypattern = "-";
            asc.WorkingDayPattern = workingdaypattern;
            asc.Priority = Convert.ToInt16(priority);
            asc.ParentId = parentid;
            asc.ParentType = parenttype;

            if(isactive == "Yes")
                asc.IsActive = true;
            else if(isactive == "No")
                asc.IsActive = false;

            context.Association.AddOrUpdate(asc);
            context.SaveChanges();
        }

        public List<AssociationList> LoadPolicies( string ParentType )
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@ParentType", ParentType);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id, Combination, ParentId, ParentType, Priority, WorkingDayPattern, Gender, case when IsActive = 1 then 'Yes' else 'No' end as IsActive from association where ParentType=@ParentType");

            try
            {
                var lst = context.Database.SqlQuery<AssociationList>( qryStr.ToString(),sqlParameter).Select( d => new AssociationList()
                {
                    Id = d.Id,
                    Combination = d.Combination,
                    ParentId = d.ParentId,
                    ParentType = d.ParentType,
                    Priority = d.Priority,
                    WorkingDayPattern = d.WorkingDayPattern,
                    Gender = d.Gender,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<AssociationList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<AssociationList>();
            }
        }

        public List<LeaveGroupList> LoadLeaveGroup()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Convert( varchar, Id ) as Id , Name from leavegroup where isactive = 1");

            try
            {
                var lst = context.Database.SqlQuery<LeaveGroupList>(qryStr.ToString()).Select(d => new LeaveGroupList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if(lst == null)
                {
                    return new List<LeaveGroupList>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<LeaveGroupList>();
            }
        }

        public List<HolidayZoneList> LoadHolidayZone()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Convert( varchar, Id ) as Id , Name from HolidayZone where isactive = 1");

            try
            {
                var lst = context.Database.SqlQuery<HolidayZoneList>(qryStr.ToString()).Select(d => new HolidayZoneList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<HolidayZoneList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<HolidayZoneList>();
            }
        }

        public List<WorkingDayPatternList> LoadWorkingDayPattern()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id, PsCode, WorkingPattern, IsActive from WorkingDayPattern where isactive = 1");

            try
            {
                var lst = context.Database.SqlQuery<WorkingDayPatternList>(qryStr.ToString()).Select(d => new WorkingDayPatternList()
                {
                    Id = d.Id,
                    PsCode = d.PsCode,
                    WorkingPattern = d.WorkingPattern,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<WorkingDayPatternList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<WorkingDayPatternList>();
            }
        }

        public List<WeeklyOffList> LoadWeeklyOff()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Convert( varchar, Id ) as Id , Name from WeeklyOffs where isactive = 1");

            try
            {
                var lst = context.Database.SqlQuery<WeeklyOffList>(qryStr.ToString()).Select(d => new WeeklyOffList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<WeeklyOffList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<WeeklyOffList>();
            }
        }

        public List<RuleGroupList> LoadPolicy()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Convert( varchar, Id ) as Id , Name from RuleGroup where isactive = 1");

            try
            {
                var lst = context.Database.SqlQuery<RuleGroupList>(qryStr.ToString()).Select(d => new RuleGroupList()
                {
                    id = d.id,
                    name = d.name
                }).ToList();

                if (lst == null)
                {
                    return new List<RuleGroupList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<RuleGroupList>();
            }
        }

        public List<CompanyList> LoadCompany()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Convert( varchar, Id ) as Id , Name from company where isactive = 1");

            try
            {
                var lst = context.Database.SqlQuery<CompanyList>(qryStr.ToString()).Select(d => new CompanyList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<CompanyList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<CompanyList>();
            }
        }

        public List<BranchList> LoadBranch()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Convert( varchar, Id ) as Id , Name from branch where isactive = 1");

            try
            {
                var lst = context.Database.SqlQuery<BranchList>(qryStr.ToString()).Select(d => new BranchList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<BranchList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<BranchList>();
            }
        }

        public List<DepartmentList> LoadDepartment()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Convert( varchar, Id ) as Id , Name from department where isactive = 1");

            try
            {
                var lst = context.Database.SqlQuery<DepartmentList>(qryStr.ToString()).Select(d => new DepartmentList()
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
            catch (Exception)
            {
                return new List<DepartmentList>();
            }
        }

        public List<DivisionList> LoadDivision()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Convert( varchar, Id ) as Id , Name from division where isactive = 1");

            try
            {
                var lst = context.Database.SqlQuery<DivisionList>(qryStr.ToString()).Select(d => new DivisionList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<DivisionList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<DivisionList>();
            }
        }

        public List<DesignationList> LoadDesignation()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Convert( varchar, Id ) as Id , Name from designation where isactive = 1");

            try
            {
                var lst = context.Database.SqlQuery<DesignationList>(qryStr.ToString()).Select(d => new DesignationList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<DesignationList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<DesignationList>();
            }
        }

        public List<GradeList> LoadGrade()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Convert( varchar, Id ) as Id , Name from grade where isactive = 1");

            try
            {
                var lst = context.Database.SqlQuery<GradeList>(qryStr.ToString()).Select(d => new GradeList()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<GradeList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<GradeList>();
            }
        }
    }
}
