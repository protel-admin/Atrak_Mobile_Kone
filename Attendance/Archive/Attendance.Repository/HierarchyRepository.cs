using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

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

        public void SaveInformation(string ReportingManager, string stafflist)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@ReportingManager", ReportingManager);
            stafflist = stafflist.Substring(0, stafflist.Length - 1);
            sqlParameter[1] = new SqlParameter("@stafflist", stafflist);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            
            qryStr.Append("update [StaffOfficial] set ReportingManager = @ReportingManager  where StaffId in (" +stafflist +")");
            context.Database.ExecuteSqlCommand(qryStr.ToString(),sqlParameter);
        }

        public void SaveOTApproverInformation(string ReportingManager, string stafflist)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@ReportingManager", ReportingManager);
            stafflist = stafflist.Substring(0, stafflist.Length - 1);
            sqlParameter[1] = new SqlParameter("@stafflist", stafflist);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            
            qryStr.Append("update [StaffOfficial] set OTReportingManager = @ReportingManager  where StaffId in (" + stafflist + ")");
            context.Database.ExecuteSqlCommand(qryStr.ToString(),sqlParameter);
        }

        public void SaveReviewerInformation(string ReportingManager, string stafflist)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@ReportingManager", ReportingManager);
            stafflist = stafflist.Substring(0, stafflist.Length - 1);
            sqlParameter[1] = new SqlParameter("@stafflist", stafflist);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("update [StaffOfficial] set Reviewer = @ReportingManager  where StaffId in (" + stafflist + ")");
            context.Database.ExecuteSqlCommand(qryStr.ToString(),sqlParameter);
        }

        public void SaveReviewerInformationforAP1(string ReportingManager, string stafflist)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@ReportingManager", ReportingManager);
           
            sqlParameter[1] = new SqlParameter("@stafflist", stafflist);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            //stafflist = stafflist.Substring(0, stafflist.Length - 1);
            qryStr.Append("update [StaffOfficial] set Reviewer = @ReportingManager, ReportingManager = @ReportingManager where StaffId in ("+ stafflist +")");
            context.Database.ExecuteSqlCommand(qryStr.ToString(),sqlParameter);
        }

        public void SaveOTReviewerInformation(string ReportingManager, string stafflist)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@ReportingManager", ReportingManager);
            
            var qryStr = new StringBuilder();
            qryStr.Clear();
            stafflist = stafflist.Substring(0, stafflist.Length - 1);
            sqlParameter[1] = new SqlParameter("@stafflist", stafflist);

            qryStr.Append("update [StaffOfficial] set OTReviewer = @ReportingManager  where StaffId in (" + stafflist + ")");
            context.Database.ExecuteSqlCommand(qryStr.ToString(),sqlParameter);
        }

        public List<ReportingList> GetTeam()
        {
            //GET THE LIST OF TEAM WHICH DOES NOT HAVE THE REPORTING MANAGER.
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select StaffId , FirstName as Name , "+
                "isnull ( repmgrfirstname , '-' ) as ReportingManagerName "+
                "from staffview where reportingmgrid is null");

            try{
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString()).Select(d => new ReportingList(){
                    StaffId = d.StaffId,
                    Name = d.Name,
                    ReportingManagerName = d.ReportingManagerName
                }).ToList();

                if(lst == null){
                    return new List<ReportingList>();}
                else{
                    return lst;}
            }
            catch(Exception){
                return new List<ReportingList>();}
        }

        public List<ReportingList> GetReviewBlankTeam()
        {
            //GET THE LIST OF TEAM WHICH DOES NOT HAVE THE REPORTING MANAGER.
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select StaffId , FirstName as Name , "+
                "isnull ( repmgrfirstname , '-' ) as ReportingManagerName,ApproverLevel " +
                "from staffview where Reviewer is null");

            try{
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString()).Select(d => new ReportingList(){
                    StaffId = d.StaffId,
                    Name = d.Name,
                    ReportingManagerName = d.ReportingManagerName,
                    ApproverLevel = d.ApproverLevel
                }).ToList();

                if(lst == null){
                    return new List<ReportingList>();}
                else{
                    return lst;}
            }
            catch(Exception){
                return new List<ReportingList>();}
        }

        public List<ReportingList> GetApproverBlankTeam()
        {
            //GET THE LIST OF TEAM WHICH DOES NOT HAVE THE REPORTING MANAGER.
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select StaffId , FirstName as Name , "+
                "isnull ( repmgrfirstname , '-' ) as ReportingManagerName "+
                "from staffview where reportingmgrid is null");

            try{
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString()).Select(d => new ReportingList(){
                    StaffId = d.StaffId,
                    Name = d.Name,
                    ReportingManagerName = d.ReportingManagerName
                }).ToList();

                if(lst == null){
                    return new List<ReportingList>();}
                else{
                    return lst;}
            }
            catch(Exception){
                return new List<ReportingList>();}
        }


        public List<ReportingList> GetReviewerTeam(string id)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@id", id);

            //GET THE LIST OF TEAM REPORTING TO THE SAID MANAGER.
            var qryStr = new StringBuilder();
            qryStr.Clear();
        
            qryStr.Append("select StaffId , FirstName as Name,isnull ( Reviewer , '-' ) as ReportingManagerName from staffview ");
           
            if (id != "")
            {
                qryStr.Append("where reviewer = @id");
            }
            else
            {
                qryStr.Append(" where reviewer is null");
            }


            try{
                
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString(), sqlParameter).Select(d => new ReportingList(){
                    StaffId = d.StaffId,
                    Name = d.Name,
                    ReportingManagerName = d.ReportingManagerName
                }).ToList();

                if (lst == null){
                    return new List<ReportingList>();}
                else{
                    return lst;}}
            catch (Exception){
                return new List<ReportingList>();}
        }

        public List<ReportingList> GetOTReviewerTeam(string id)
        {
            //GET THE LIST OF TEAM REPORTING TO THE SAID MANAGER.
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@id", id);
            
            var qryStr = new StringBuilder();
            qryStr.Clear();
           



                qryStr.Append("select StaffId , FirstName as Name, isnull ( OTReviewer , '-' ) as ReportingManagerName from staffview ");

            if (id != "")
            {
                qryStr.Append(" where OTReviewer = @id");
            }
            else
            {
                qryStr.Append(" where OTReviewer is null");
            }


            try
            {
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString(),sqlParameter).Select(d => new ReportingList()
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
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@id", id);
            
            var qryStr = new StringBuilder();
            qryStr.Clear();
            
                qryStr.Append("select StaffId , FirstName as Name, isnull ( OTReportingManager , '-' ) as ReportingManagerName from staffview ");
            if (id != "")
            {
                qryStr.Append(" where OTReportingManager = @id");
            }
            else
            {
                qryStr.Append(" where OTReportingManager is null");
            }

            try
            {
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString(),sqlParameter).Select(d => new ReportingList()
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
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@id", id);
            var qryStr = new StringBuilder();
            qryStr.Clear();
           
                qryStr.Append("select StaffId , FirstName as Name,isnull ( repmgrfirstname , '-' ) as ReportingManagerName from staffview ");
            if (id != "")
            {
                qryStr.Append("   where reportingmgrid = @id ");
            }

            else
            {
                qryStr.Append(" where reportingmgrid is null");
            }

            try
            {
                var lst = context.Database.SqlQuery<ReportingList>(qryStr.ToString(),sqlParameter).Select(d => new ReportingList()
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
    }
}
