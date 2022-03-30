using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class HierarchyRepository : IDisposable
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
        public HierarchyRepository()
        {
            context = new AttendanceManagementContext();
        }

        public string SaveInformation(string Approver, string stafflist, string ApprovalLevel)
        {
            var qryStr = new StringBuilder();
            try
            {
                //if (ApprovalLevel != null && ApprovalLevel != "")
                //{
                //    ApprovalLevel = ApprovalLevel.Substring(0, ApprovalLevel.Length - 1);

                //}            
                var SplitStafflist = stafflist.Split(',');
                for (int i = 0; i < SplitStafflist.Length; i++)
                {
                    string Stafflist = SplitStafflist[i];
                    if (ApprovalLevel != null && ApprovalLevel != "" && ApprovalLevel != "0")
                    {
                        var SplitApprovalLevellist = ApprovalLevel.Split(',');
                        string AppLst = SplitApprovalLevellist[i];
                        if (AppLst == "1")
                        {
                            qryStr.Clear();
                            qryStr.Append("update [StaffOfficial] set ReportingManager = @Approver, Approver2 = @Approver " +
                                "where StaffId in (" + Stafflist + ")");
                            context.Database.ExecuteSqlCommand(qryStr.ToString(),new SqlParameter("@Approver", Approver));
                        }
                        else if (AppLst == "2")
                        {
                            qryStr.Clear();
                            qryStr.Append("update [StaffOfficial] set ReportingManager = @Approver " +
                                "where StaffId in (" + Stafflist + ")");
                            context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@Approver", Approver));
                        }
                    }
                    else
                    {
                        qryStr.Clear();
                        qryStr.Append("update [StaffOfficial] set ReportingManager = @Approver " +
                            "where StaffId in (" + Stafflist + ")");
                        context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@Approver", Approver));
                    }
                }
                return "OK";
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void SaveOTApproverInformation(string ReportingManager, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            stafflist = stafflist.Substring(0, stafflist.Length - 1);
            qryStr.Append("update [StaffOfficial] set OTReportingManager = @ReportingManager where StaffId in (" + stafflist + ")");
            context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@ReportingManager", ReportingManager));
        }

        public void SaveReviewerInformation(string ReportingManager, string stafflist)//Update Approver1 ID
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            //stafflist = stafflist.Substring(0, stafflist.Length - 1);
            qryStr.Append("update [StaffOfficial] set Approver2 = @ReportingManager where StaffId in (" + stafflist + ")");
            context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@ReportingManager", ReportingManager));
        }

        public void SaveReviewerInformationforAP1(string Reviewer, string stafflist) //Update Approver1/approver2 as Same
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            //stafflist = stafflist.Substring(0, stafflist.Length - 1);
            qryStr.Append("update [StaffOfficial] set Approver2 = @Reviewer, ReportingManager = @Reviewer where StaffId in (" + stafflist + ")");
            context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@Reviewer", Reviewer));
        }

        public void SaveOTReviewerInformation(string ReportingManager, string stafflist)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            stafflist = stafflist.Substring(0, stafflist.Length - 1);
            qryStr.Append("update [StaffOfficial] set OTReviewer = '" + ReportingManager + "' where StaffId in (" + stafflist + ")");
            context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@ReportingManager", ReportingManager));
        }

        public List<ReportingList> GetTeam()
        {
            //GET THE LIST OF TEAM WHICH DOES NOT HAVE THE REPORTING MANAGER.
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select StaffId , FirstName as Name ,isnull ( repmgrfirstname , '-' ) as ReportingManagerName " +
                "from staffview where reportingmgrid is null");

            try
            {
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString()).Select(d => new ReportingList()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    ReportingManagerName = d.ReportingManagerName
                }).ToList();

                if (lst == null)
                {
                    return new List<ReportingList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ReportingList>();
            }
        }

        public List<ReportingList> GetReviewBlankTeam()
        {
            //GET THE LIST OF TEAM WHICH DOES NOT HAVE THE REPORTING MANAGER.
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select StaffId , FirstName as Name , " +
                "isnull ( ReviewerName , '-' ) as ReviewerName, ApproverLevel " +
                "from staffview where Approver2 is null or Approver2 ='' ");
            try
            {
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString()).Select(d => new ReportingList()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    ReportingManagerName = d.ReviewerName,
                    ApproverLevel = d.ApproverLevel
                }).ToList();

                if (lst == null)
                {
                    return new List<ReportingList>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<ReportingList>();
            }
        }

        public List<ReportingList> GetApproverBlankTeam()
        {
            //GET THE LIST OF TEAM WHICH DOES NOT HAVE THE REPORTING MANAGER.
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select StaffId , FirstName as Name , " +
                "isnull ( REPMGRFIRSTNAME , '-' ) as ReportingManagerName " +
                "from staffview where REPORTINGMGRID is null or REPORTINGMGRID = ''");

            try
            {
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString()).Select(d => new ReportingList()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    ReportingManagerName = d.ReportingManagerName
                }).ToList();

                if (lst == null)
                {
                    return new List<ReportingList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ReportingList>();
            }
        }


        public List<ReportingList> GetReviewerTeam(string id)
        {
            //GET THE LIST OF TEAM REPORTING TO THE SAID MANAGER.
            var qryStr = new StringBuilder();
            qryStr.Clear();
            if (id != "")
            {
                qryStr.Append(" select StaffId , FirstName as Name, " +
                    " isnull ( ReviewerName , '-' ) as ReviewerName , ApproverLevel" +
                    " from staffview where Approver2 = @id");
            }
            else
            {
                qryStr.Append(" select StaffId , FirstName as Name, " +
                " isnull ( ReviewerName , '-' ) as ReviewerName , ApproverLevel " +
                " from staffview where Approver2 is null");

            }

            try
            {
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString(),new SqlParameter("@id", id)).Select(d => new ReportingList()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    ReportingManagerName = d.ReviewerName,
                    ApproverLevel = d.ApproverLevel
                }).ToList();

                if (lst == null)
                {
                    return new List<ReportingList>();
                }
                else
                {
                    return lst;
                }
            }
            catch 
            {
                return new List<ReportingList>();
            }
        }

        public List<ReportingList> GetOTReviewerTeam(string id)
        {
            //GET THE LIST OF TEAM REPORTING TO THE SAID MANAGER.
            var qryStr = new StringBuilder();
            qryStr.Clear();
            if (id != "")
            {
                qryStr.Append("select StaffId , FirstName as Name, " +
                    "isnull ( OTReviewer , '-' ) as ReportingManagerName " +
                    "from staffview where OTReviewer = @id");
            }
            else
            {
                qryStr.Append("select StaffId , FirstName as Name, " +
                "isnull ( OTReviewer , '-' ) as ReportingManagerName " +
                "from staffview where OTReviewer is null");

            }

            try
            {
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString(), new SqlParameter("@id", id)).Select(d => new ReportingList()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    ReportingManagerName = d.ReportingManagerName
                }).ToList();

                if (lst == null)
                {
                    return new List<ReportingList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ReportingList>();
            }
        }

        public List<ReportingList> GetOTApproverTeam(string id)
        {
            //GET THE LIST OF TEAM REPORTING TO THE SAID MANAGER.
            var qryStr = new StringBuilder();
            qryStr.Clear();
            if (id != "")
            {
                qryStr.Append("select StaffId , FirstName as Name, " +
                    "isnull ( OTReportingManager , '-' ) as ReportingManagerName " +
                    "from staffview where OTReportingManager = @id");
            }
            else
            {
                qryStr.Append("select StaffId , FirstName as Name, " +
                "isnull ( OTReportingManager , '-' ) as ReportingManagerName " +
                "from staffview where OTReportingManager is null");

            }

            try
            {
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString(), new SqlParameter("@id", id)).Select(d => new ReportingList()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    ReportingManagerName = d.ReportingManagerName
                }).ToList();

                if (lst == null)
                {
                    return new List<ReportingList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ReportingList>();
            }
        }


        public List<ReportingList> GetTeam(string id)
        {
            //GET THE LIST OF TEAM REPORTING TO THE SAID MANAGER.
            var qryStr = new StringBuilder();
            qryStr.Clear();
            if (id != "")
            {
                qryStr.Append("select StaffId , FirstName as Name, " +
                    "isnull ( REPMGRFIRSTNAME , '-' ) as ReportingManagerName,ApproverLevel " +
                    "from staffview where REPORTINGMGRID = @id");
            }
            else
            {
                qryStr.Append("select StaffId , FirstName as Name, " +
                "isnull ( REPORTINGMGRID , '-' ) as ReportingManagerName " +
                "from staffview where (REPORTINGMGRID is null or REPORTINGMGRID = '') ");

            }

            try
            {
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString(), new SqlParameter("@id", id)).Select(d => new ReportingList()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    ReportingManagerName = d.ReportingManagerName,
                    ApproverLevel = d.ApproverLevel
                }).ToList();

                if (lst == null)
                {
                    return new List<ReportingList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ReportingList>();
            }
        }
    }
}
