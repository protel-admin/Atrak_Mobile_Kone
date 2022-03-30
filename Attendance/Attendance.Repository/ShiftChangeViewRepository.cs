using Attendance.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class ShiftChangeViewRepository : IDisposable
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
        public ShiftChangeViewRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<DropDownStrModel> GetShiftShortNameRepository()
        {
            List<DropDownStrModel> lst = new List<DropDownStrModel>();
            try
            {
                lst = context.Shifts.Where(condition => condition.IsActive == true).Select(select => new DropDownStrModel()
                {
                    Name = select.ShortName,
                    Id = select.Name
                }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }

        public List<Dates> AllStaffShiftCalendar(string StaffId, string Fromdate, string Todate)
        {
            SqlParameter[] Param = new SqlParameter[3];
            Param[0] = new SqlParameter("@staffid", StaffId);
            Param[1] = new SqlParameter("@fromdate", Fromdate);
            Param[2] = new SqlParameter("@todate", Todate);
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("EXEC [DBO].[ShiftCalendarNew] @staffid, @fromdate,@todate");

            try
            {
                var data = context.Database.SqlQuery<Dates>(QryStr.ToString(), Param).ToList();


                if (data == null)
                {
                    return new List<Dates>();
                }
                else
                {
                    return data;

                }
            }

            catch (Exception err)
            {
                throw err;

            }

        }



        public List<StaffList> LoadReportingmangerwisestaff(string Staffid)
        {
            SqlParameter[] Param = new SqlParameter[1];
            Param[0] = new SqlParameter("@staffid", Staffid);
            var qryStr = new StringBuilder();
            var vrepo = new StaffListRepository();

            qryStr.Clear();
            qryStr.Append("select staffid , DBO.FNGETSTAFFNAME(STAFFID) as staffname , DBO.FNGETMASTERNAME(STAFFID , 'DP') as Department from staffview WHERE StatusId = 1 AND REPORTINGMGRID= '"+Staffid+"'");

            var lst = vrepo.GetAllStaffLists(qryStr.ToString());

            return lst;
        }

        public List<StaffList> LoadDepartmentwise(string Departmentid, string staffid)
        {
            SqlParameter[] Param = new SqlParameter[2];
            Param[0] = new SqlParameter("@Departmentid", Departmentid);
            Param[1] = new SqlParameter("@staffid", staffid);
            var qryStr = new StringBuilder();
            var vrepo = new StaffListRepository();

            qryStr.Clear();
            qryStr.Append("select staffid , DBO.FNGETSTAFFNAME(STAFFID) as staffname , DBO.FNGETMASTERNAME(STAFFID , 'DP') as Department " +
                "from staffview WHERE  StatusId = 1 AND Deptid='"+Departmentid+"'");

            var lst = vrepo.GetAllStaffLists(qryStr.ToString());

            return lst;
        }

        public List<ShiftviewList> LoadShifts()
        {

            var qryStr = new StringBuilder();
            var vrepo = new StaffListRepository();

            qryStr.Clear();
            qryStr.Append("select id as Shiftid,Shortname as Shiftshortname,convert(varchar(5), starttime, 108) as Shiftstarttime,convert(varchar(5), endtime , 108) as Shiftendtime from shifts");


            try
            {
                var lstGrp =
                    context.Database.SqlQuery<ShiftviewList>(qryStr.ToString())
                        .Select(d => new ShiftviewList()
                        {
                            //Id = d.Id,
                            Shiftid = d.Shiftid,
                            Shiftshortname = d.Shiftshortname,
                            Shiftstarttime = d.Shiftstarttime,
                            Shiftendtime = d.Shiftendtime,

                        }).ToList();

                if (lstGrp == null)
                {
                    return new List<ShiftviewList>();
                }
                else
                {
                    return lstGrp;
                }

            }
            catch (Exception)
            {
                return new List<ShiftviewList>();
                throw;
            }

        }

        public String UpdateAttendanceshift(string O_Staffid, string O_Shiftdate, string N_Shiftid, string N_Shiftshortname)
        {
            SqlParameter[] Param = new SqlParameter[4];
            Param[0] = new SqlParameter("@O_Staffid", O_Staffid);
            Param[1] = new SqlParameter("@O_Shiftdate", O_Shiftdate);
            Param[2] = new SqlParameter("@N_Shiftid", N_Shiftid);
            Param[3] = new SqlParameter("@N_Shiftshortname", N_Shiftshortname);
            var qryStr = new StringBuilder();
            var qrystr1 = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("EXEC [SAVESHIFTCHANGEVIEWDETAILS]   @O_Staffid,@O_Shiftdate,@N_Shiftid,@N_Shiftshortname");
            context.Database.ExecuteSqlCommand(qryStr.ToString());
            return "OK";

        }

        public List<DepartmentList> GetDepartmentList(string staffid)
        {
             SqlParameter[] Param = new SqlParameter[1];
            Param[0] = new SqlParameter("@staffid", staffid);
            var qryStr = new StringBuilder();
            qryStr.Append("select '0'  as id , '-- Select Department --' as name  union select id , name from department A join StaffOfficial B on A.id=B.DepartmentId where B.staffid=@staffid");
            var lst = context.Database.SqlQuery<DepartmentList>(qryStr.ToString(), Param).Select(d => new DepartmentList()
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();

            if (lst.Count == 0)
            {
                return new List<DepartmentList>();
            }
            else
            {
                return lst;
            }
        }

        public List<ShiftView> GetAllShifts()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select id , name , shortname , " +
                            "left ( convert ( varchar , starttime , 114 ) , 8 ) as starttime , " +
                            "left ( convert ( varchar , endtime , 114 ) , 8 ) as endtime , " +
                            "left ( convert ( varchar , gracelateby , 114 ) , 8 ) as gracelateby , " +
                            "left ( convert ( varchar , graceearlyby , 114 ) , 8 ) as graceearlyby , " +
                            "left ( convert ( varchar , breakstarttime , 114 ) , 8 ) as breakstarttime, " +
                            "left ( convert ( varchar , breakendtime, 114 ) , 8 ) as breakendtime, " +
                            "convert ( varchar , mindayhours ) as  mindayhours, convert ( varchar , minweekhours ) as minweekhours , " +
                            "case when isactive = 1 then 'Yes' else 'No' end as " +
                            "isactive , CreatedOn , CreatedBy from shifts order by convert ( varchar ( 8 ) , StartTime , 114 ),convert ( varchar ( 8 ) , EndTime , 114 )  Asc");
            var lstSh = context.Database.SqlQuery<ShiftView>(qryStr.ToString()).Select(d => new ShiftView()
            {
                Id = d.Id,
                Name = d.Name,
                ShortName = d.ShortName,
                StartTime = d.StartTime,
                EndTime = d.EndTime,
                GraceLateBy = d.GraceLateBy,
                GraceEarlyBY = d.GraceEarlyBY,
                BreakStartTime = d.BreakStartTime,
                BreakEndTime = d.BreakEndTime,
                MinDayHours = d.MinDayHours,
                MinWeekHours = d.MinWeekHours,
                IsActive = d.IsActive,
                CreatedOn = d.CreatedOn,
                CreatedBy = d.CreatedBy
            }).ToList();

            if (lstSh.Count == 0)
            {
                return new List<ShiftView>();
            }
            else
            {
                return lstSh;
            }
        }

        public void UpdateAssignShifts(string staffid, string fromdate, string todate, string stafflist, string createdBy)
        {
            SqlParameter[] Param = new SqlParameter[5];
            Param[0] = new SqlParameter("@staffid", staffid);
            Param[1] = new SqlParameter("@fromdate", fromdate);
            Param[2] = new SqlParameter("@todate", todate);
            Param[3] = new SqlParameter("@stafflist", stafflist);
            Param[4] = new SqlParameter("@createdBy", createdBy);
            var qryStr = new StringBuilder();
            var qrystr1 = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("EXEC [AssignShifts]   @staffid,@fromdate,@todate,@stafflist,@createdBy");
            context.Database.ExecuteSqlCommand(qryStr.ToString());



        }

        public string UpdateAttendanceshiftGrid(List<ShiftChangeDetailViewModel> model, string UserId, string CreatedBy)
        {
            SqlParameter[] Param = new SqlParameter[2];
            Param[0] = new SqlParameter("@UserId", UserId);
            Param[1] = new SqlParameter("@CreatedBy", CreatedBy);
            string Message = string.Empty;
            var qryStr = new StringBuilder();
            var qrystr1 = new StringBuilder();
            using (var Trns = context.Database.BeginTransaction())
            {
                try
                {
                    
                    foreach (var dt in model)
                    {
                        if (dt.Date.Where(i => i.Updated == true).Count() > 0)
                        {
                            foreach (var list in dt.Date.Where(i => i.Updated == true).ToList())
                            {
                                string Staffid = list.Staffid;
                                string Shiftid = list.Shiftid;
                                string ShiftShortname = list.Shiftshortname;
                                var Shiftdate = list.ShiftDate;
                                qryStr.Clear();
                              
                                qryStr.Append("EXEC [SAVESHIFTCHANGEVIEWDETAILS]  @Staffid, @Shiftdate ,@Shiftid, @ShiftShortname, @CreatedBy");
                                context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@Staffid", Staffid),
                                    new SqlParameter("@Shiftdate", Shiftdate), new SqlParameter("@Shiftid", Shiftid),
                                    new SqlParameter("@ShiftShortname", ShiftShortname), new SqlParameter("@CreatedBy", CreatedBy));
                            }
                        }
                    }
                    Trns.Commit();
                    Message = "OK";
                    return Message;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}



