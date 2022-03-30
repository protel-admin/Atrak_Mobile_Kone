using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Data.Entity.Migrations;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository
{
    public class RHRepository : IDisposable
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

        public RHRepository()
        {
            context = new AttendanceManagementContext();
        }

        public void CancelRH(string ApplicationId)
        {


            var QryStr = new StringBuilder();

            QryStr.Clear();
            QryStr.Append("SELECT COUNT(*) AS TOTALCOUNT FROM RHAPPLICATION A INNER JOIN APPLICATIONAPPROVAL B ON A.ID = B.PARENTID WHERE A.id = '" + ApplicationId + "' ");
            QryStr.Append("AND APPROVALSTATUSID IN ( 2 , 3 ) AND ");
            QryStr.Append("( CONVERT ( DATETIME , CONVERT ( VARCHAR , LEAVESTARTDATE , 106 ) ) < CONVERT ( DATETIME , CONVERT ( VARCHAR , GETDATE() , 106 ) ) OR  ");
            QryStr.Append("CONVERT ( DATETIME , CONVERT ( VARCHAR , LEAVEENDDATE , 106 ) ) < CONVERT ( DATETIME , CONVERT ( VARCHAR , GETDATE() , 106 ) ) ) ");

            var res2 = context.Database.SqlQuery<int>(QryStr.ToString()).FirstOrDefault();

            if (Convert.ToInt16(res2) > 0)
            {
                throw new Exception("Cannot cancel past application. To cancel past application it must neither be approved nor rejected.");
            }

            QryStr.Clear();
            QryStr.Append("SELECT CONVERT ( VARCHAR , ISCANCELLED ) AS ISCANCELLED FROM RHAPPLICATION WHERE id = @ApplicationId");
            var res1 = context.Database.SqlQuery<string>(QryStr.ToString(),new SqlParameter("@ApplicationId", ApplicationId)).FirstOrDefault();

            if (res1.Equals("1"))
            {
                throw new Exception("Cannot cancel a cancelled application.");
            } 
            
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("UPDATE RHAPPLICATION SET ISCANCELLED = 1 WHERE ID = @ApplicationId");
            context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@ApplicationId", ApplicationId));
        }

        public void SaveRHApplication(RHApplication rha)
        {
            var ReportingManager = string.Empty;
            var selfapproval = false;
            var repo = new CommonRepository();
            string BaseAddress = string.Empty;
            string AppName = string.Empty;

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    //check if the id was initially generated.
                    if (string.IsNullOrEmpty(rha.Id) == true) //if not then...
                    {
                        SaveRHInformation(rha);
                        ReportingManager = repo.GetReportingManager(rha.StaffId);
                        if (string.IsNullOrEmpty(ReportingManager) == true)
                        {
                            ReportingManager = rha.StaffId;
                            selfapproval = true;
                        }

                        repo.SaveIntoApplicationApproval(rha.Id, "RH", rha.StaffId, ReportingManager, selfapproval);

                    }
                    else
                    {
                        SaveRHInformation(rha);
                    }


                    //##############################################################################################################
                    //CODE BLOCK TO SEND EMAIL INTIMATION TO THE REPORTING MANAGER AND AN ACKNOWLEDGEMENT TO THE SENDER WHO RAISED 
                    //  THE APPLICATION.
                    //##############################################################################################################
                    //////////rha.RestrictedHolidays = context.Database.SqlQuery<RestrictedHolidays>("select * from RestrictedHolidays where id = " + rha.RHId).Select(d => new RestrictedHolidays()
                    //////////{
                    //////////    Id = d.Id,
                    //////////    Name = d.Name,
                    //////////    RHDate =d.RHDate,
                    //////////    RHYear= d.RHYear,
                    //////////    CompanyId = d.CompanyId, 
                    //////////    LeaveId =d.LeaveId
                    //////////}).FirstOrDefault();

                    try
                    {
                        //try to get the server ip from the web.config file.
                        BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                        //check if the server ip address has been given or not.
                        if (string.IsNullOrEmpty(BaseAddress) == true) //if the server ip address has not been given then...
                            //throw exception.
                            throw new Exception("BaseAddress parameter is blank in web.config file.");
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    RestrictedHolidays RH = null;

                    SqlParameter[] sqlParameter = new SqlParameter[1];
                    sqlParameter[0] = new SqlParameter("@id", Convert.ToString(rha.RHId));
                    try
                    {
                        rha.RestrictedHolidays = context.Database.SqlQuery<RestrictedHolidays>("SELECT * FROM RESTRICTEDHOLIDAYS WHERE Id = @id",sqlParameter).FirstOrDefault();
                    }
                    catch(Exception)
                    {
                        throw;
                    }
                    

                    //get the emailid of the reporting manager.
                    var ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
                    //get the emailid of the staff who raises the leave application.
                    var StaffEmailId = repo.GetEmailIdOfEmployee(rha.StaffId);
                    //get the name of the staff.
                    var StaffName = repo.GetStaffName(rha.StaffId);
                    //get the name of the reporting manager.
                    var ReportingManagerName = repo.GetStaffName(ReportingManager);

                    //check if the reporting manager has an email id.
                    if (string.IsNullOrEmpty(ReportingManagerEmailId) == true) //if the reporting manager does not have an email id then...
                    {
                        //check if the staff has an email id.
                        if (string.IsNullOrEmpty(StaffEmailId) == true) //if the staff does not have an email id then...
                        {
                            //do not take any action.
                        }
                        else //if the staff has an email id then...
                        {
                            var EmailStr = string.Empty;
                            EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your restricted holiday application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">RH Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(rha.RestrictedHolidays.RHDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">RH Reason:</td><td style=\"width:80%;\">" + rha.RestrictedHolidays.Name + "</td></tr></table></p><p>But an intimation email could not be sent to your reporting manager because of a missing email id.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                            //send intimation to the staff stating that his/her leave application has been acknowleged 
                            //function call to get the name of the staff and the reporting manager.
                            //  but the reporting manager does not have a email id so no intimation has been sent to him.
                            repo.SendEmailMessage("", StaffEmailId, "", "", "restricted holiday application of " + rha.StaffId + " - " + StaffName, EmailStr);
                        }
                    }
                    else // if the reporting manager has an email id then...
                    {
                        var EmailStr = string.Empty;
                        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a restricted holiday. Restricted holiday details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">RH Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(rha.RestrictedHolidays.RHDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">RH Reason:</td><td style=\"width:80%;\">" + rha.RestrictedHolidays.Name + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this application.</p><p><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + rha.Id + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + rha.Id + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        // send intimation to the reporting manager about the restricted holiday application.
                        repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "restricted holiday application of " + StaffName, EmailStr);

                        // send acknowledgement to the staff who raised the leave application.
                        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your restricted holiday application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">RH Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(rha.RestrictedHolidays.RHDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">RH Reason:</td><td style=\"width:80%;\">" + rha.RestrictedHolidays.Name+ "</td></tr></table></p><p>This application has been sent also to your reporting manager.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        repo.SendEmailMessage("", StaffEmailId, "", "", "restricted holiday application sent to " + ReportingManagerName, EmailStr);
                    }
                    //##############################################################################################################
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public void SaveRHInformation(RHApplication rha)
        {
            var lastid = string.Empty;
            if (string.IsNullOrEmpty(rha.Id) == true)
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid("RHAPPLICATION", "id", "RH", "", 10, ref lastid);
                rha.Id = lastid;
            }

            context.RHApplication.AddOrUpdate(rha);
            context.SaveChanges();
        }

        public List<RHHistory> GetRHApplicationHistory(string StaffId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT CONVERT ( VARCHAR , A.ID ) AS RHApplicationId , " +
                " CONVERT ( VARCHAR , A.RHID ) AS RHID , A.StaffId , " +
                "DBO.FNGETSTAFFNAME(A.STAFFID) AS StaffName , B.NAME AS HolidayName, " +
                "REPLACE ( CONVERT ( VARCHAR , RHDATE , 106 ) , ' ' , '-' ) AS RHDate , " +
                "CONVERT ( VARCHAR , RHYEAR ) AS RHYear, B.LeaveId, " +
                "CONVERT ( VARCHAR , C.APPROVALSTATUSID ) AS ApprovalStatusId , CASE WHEN ISCANCELLED = 0 THEN D.NAME ELSE 'CANCELLED' END AS ApprovalStatusName " +
                "FROM RHAPPLICATION A INNER JOIN RESTRICTEDHOLIDAYS B ON A.RHID = B.ID " +
                "INNER JOIN APPLICATIONAPPROVAL C ON C.PARENTID = A.ID " + 
                "INNER JOIN APPROVALSTATUS D ON C.APPROVALSTATUSID = D.ID WHERE ISCANCELLED = 0 AND A.STAFFID = @StaffId");

            try
            {
                var lst = context.Database.SqlQuery<RHHistory>(QryStr.ToString(),new SqlParameter("@StaffId", StaffId)).Select(d => new RHHistory()
                {
                    RHApplicationId = d.RHApplicationId,
                    HolidayName = d.HolidayName,
                    RHDate = d.RHDate,
                    RHYear = d.RHYear,
                    RHID = d.RHID,
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    LeaveId = d.LeaveId,
                    ApprovalStatusId = d.ApprovalStatusId,
                    ApprovalStatusName = d.ApprovalStatusName
                }).ToList();

                return lst;

            }
            catch(Exception)
            {
                throw;
            }
        }

        public string ValidateRHApplication(string StaffId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT DBO.fnValidateRHApplication(@StaffId)");

            var ret = context.Database.SqlQuery<string>(QryStr.ToString(),new SqlParameter("@StaffId",StaffId)).FirstOrDefault();

            if(ret != "OK")
            {
                throw new Exception(ret);
            }

            return ret;
        }

        public List<RestrictedHolidayList> GetRestrictedHolidays(string CompanyId, string StaffId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@CompanyId", CompanyId);
            sqlParameter[1] = new SqlParameter("@StaffId", StaffId);
            

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT CONVERT ( VARCHAR , ID ) AS Id, Name , REPLACE ( CONVERT ( VARCHAR , RHDATE , 106 ) , ' ' , '-' ) AS RHDate , CONVERT ( VARCHAR , RHYEAR ) as RHYear ,CompanyId , LeaveId FROM RESTRICTEDHOLIDAYS WHERE RHYEAR = YEAR ( GETDATE() ) AND COMPANYID = @CompanyId AND ID NOT IN (SELECT RHID FROM RHAPPLICATION WHERE ISCANCELLED = 0 AND STAFFID = @StaffId)");

            try
            {
                var lst = context.Database.SqlQuery<RestrictedHolidayList>(qryStr.ToString(),sqlParameter).Select(d => new RestrictedHolidayList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    RHDate = d.RHDate,
                    RHYear = d.RHYear,
                    CompanyId = d.CompanyId,
                    LeaveId = d.LeaveId,
                    ImportDate = d.ImportDate,
                    ImportedBy = d.ImportedBy
                }).ToList();

                if(lst == null)
                {
                    return new List<RestrictedHolidayList>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<RestrictedHolidayList>();
            }
        }
    }
}
