using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository
{
    public class LaterOffRepository : IDisposable
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

        public AttendanceManagementContext context = null;
        public LaterOffRepository()
        {
            context = new AttendanceManagementContext();
        }

        public void CancelLaterOff(string ApplicationId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@ApplicationId", ApplicationId);
            var QryStr = new StringBuilder();

            QryStr.Clear();
            QryStr.Append("SELECT COUNT(*) AS TOTALCOUNT FROM LATEROFF A INNER JOIN APPLICATIONAPPROVAL B ON A.ID = B.PARENTID WHERE A.id = @ApplicationId ");
            QryStr.Append("AND APPROVALSTATUSID IN ( 2 , 3 ) AND ");
            QryStr.Append("( CONVERT ( DATETIME , CONVERT ( VARCHAR , LEAVESTARTDATE , 106 ) ) < CONVERT ( DATETIME , CONVERT ( VARCHAR , GETDATE() , 106 ) ) OR  ");
            QryStr.Append("CONVERT ( DATETIME , CONVERT ( VARCHAR , LEAVEENDDATE , 106 ) ) < CONVERT ( DATETIME , CONVERT ( VARCHAR , GETDATE() , 106 ) ) ) ");

            var res2 = context.Database.SqlQuery<int>(QryStr.ToString(),sqlParameter).FirstOrDefault();

            if (Convert.ToInt16(res2) > 0)
            {
                throw new Exception("Cannot cancel past application. To cancel past application it must neither be approved nor rejected.");
            }

            QryStr.Clear();
            QryStr.Append("SELECT CONVERT ( VARCHAR , ISCANCELLED ) AS ISCANCELLED FROM LATEROFF WHERE id = @ApplicationId");
            var res1 = context.Database.SqlQuery<string>(QryStr.ToString(),sqlParameter).FirstOrDefault();

            if (res1.Equals("1"))
            {
                throw new Exception("Cannot cancel a cancelled application.");
            }            
            
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("UPDATE LATEROFF SET ISCANCELLED = 1 WHERE ID = @ApplicationId");
            context.Database.ExecuteSqlCommand(qryStr.ToString(),sqlParameter);
        }

        public List<LaterOffList> GetAllLaterOff(string staffid)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@staffid", staffid);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            if (staffid == "-")
            {
                qryStr.Append("select Id , StaffId , LaterOffReqDate , " +
                                "LaterOffAvailDate , Reason " +
                                "from lateroff");
            }
            else
            {
                qryStr.Append("select Id , StaffId , dbo.fnGetStaffName(StaffId) as StaffName , LaterOffReqDate , " +
                                 "LaterOffAvailDate , Reason " +
                                 "from lateroff where staffid = @staffid AND ISCANCELLED = 0 ");
            }
            try
            {
                var lst =
                context.Database.SqlQuery<LaterOffList>(qryStr.ToString(),sqlParameter)
                        .Select(c => new LaterOffList()
                        {
                            Id = c.Id,
                            StaffId = c.StaffId,
                            StaffName = c.StaffName,
                            LaterOffReqDate = c.LaterOffReqDate,
                            LaterOffAvailDate = c.LaterOffAvailDate,
                            Reason = c.Reason
                        }).ToList();

                if (lst == null)
                {
                    return new List<LaterOffList>();
                }
                else
                {
                    return lst;
                }

            }
            catch (Exception)
            {
                return new List<LaterOffList>();
            }
        }

        public void SaveLaterOffInfo(LaterOff co)
        {
            var BaseAddress = string.Empty;
            var ReportingManager = string.Empty;
            var selfapproval = false;
            var repo = new CommonRepository();

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    //check if the id was initially generated.
                    if (string.IsNullOrEmpty(co.Id) == true) //if not then...
                    {
                        SaveLaterOffDetails(co);
                        ReportingManager = repo.GetReportingManager(co.StaffId);
                        if (string.IsNullOrEmpty(ReportingManager) == true)
                        {
                            ReportingManager = co.StaffId;
                            selfapproval = true;
                        }

                        repo.SaveIntoApplicationApproval(co.Id, "LO", co.StaffId, ReportingManager, selfapproval);
                    }
                    else
                    {
                        SaveLaterOffDetails(co);
                    }


                    //##############################################################################################################
                    //CODE BLOCK TO SEND EMAIL INTIMATION TO THE REPORTING MANAGER AND AN ACKNOWLEDGEMENT TO THE SENDER WHO RAISED 
                    //  THE APPLICATION.
                    //##############################################################################################################

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

                    //get the emailid of the reporting manager.
                    var ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
                    //get the emailid of the staff who raises the leave application.
                    var StaffEmailId = repo.GetEmailIdOfEmployee(co.StaffId);
                    //get the name of the staff.
                    var StaffName = repo.GetStaffName(co.StaffId);
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
                            EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Later Off application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">Req Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.LaterOffReqDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Avail Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.LaterOffAvailDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + co.Reason + "</td></tr></table></p><p>But an intimation email could not be sent to your reporting manager because of a missing email id.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                            //send intimation to the staff stating that his/her Later Off application has been acknowleged 
                            //function call to get the name of the staff and the reporting manager.
                            //  but the reporting manager does not have a email id so no intimation has been sent to him.
                            repo.SendEmailMessage("", StaffEmailId, "", "", "Later Off application of " + co.StaffId + " - " + StaffName, EmailStr);
                        }
                    }
                    else // if the reporting manager has an email id then...
                    {
                        var EmailStr = string.Empty;
                        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a Later Off. Later Off details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">Req Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.LaterOffReqDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Avail Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.LaterOffAvailDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + co.Reason + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this application.</p><p><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + co.Id + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + co.Id + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        // send intimation to the reporting manager about the Later Off application.
                        repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Later Off application of " + StaffName, EmailStr);

                        // send acknowledgement to the staff who raised the leave application.
                        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Later Off application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">Req Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.LaterOffReqDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Avail Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.LaterOffAvailDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + co.Reason + "</td></tr></table></p><p>This application has been sent also to your reporting manager.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        repo.SendEmailMessage("", StaffEmailId, "", "", "Later Off application sent to " + ReportingManagerName, EmailStr);
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

        public string ValidateApplication(string StaffId, string FromDate, string ToDate)
        {
            SqlParameter[] sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            sqlParameter[1] = new SqlParameter("@FromDate", FromDate);
            sqlParameter[2] = new SqlParameter("@ToDate", ToDate);
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select dbo.fnValidateLaterOffApplication(@StaffId,@FromDate,@ToDate)");

            try
            {
                var str = context.Database.SqlQuery<string>(QryStr.ToString(),sqlParameter).FirstOrDefault();
                return str;
            }
            catch (Exception err)
            {
                return "ERROR!" + err.Message;
            }
        }
            
        public void SaveLaterOffDetails(LaterOff co)
        {
            var lastid = string.Empty;
            if (string.IsNullOrEmpty(co.Id) == true)
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid("lateroff", "Id", "LO", "", 10, ref lastid);
                co.Id = lastid;
            }

            context.LaterOff.AddOrUpdate(co);
            context.SaveChanges();
        }

        public List<ValidLaterOffDates> GetValidLaterOffDates(string StaffId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select replace ( convert ( varchar , ActionDate , 106 ) , ' ' , '-' ) as LaterOffReqDate , Validity ");
            QryStr.Append("from lateroffdate where getdate() between convert ( datetime , convert ( varchar , actiondate , 106 ) ) and ");
            QryStr.Append("convert ( datetime , convert( varchar , dateadd ( M  , Validity , actiondate ) , 106 ) ) ");
            QryStr.Append("and companyid = ( select companyid from staffofficial where staffid = @StaffId )");
            QryStr.Append("and CONVERT ( DATETIME , CONVERT ( VARCHAR , actiondate , 106 ) ) not in ( select CONVERT ( DATETIME , CONVERT ( VARCHAR , LaterOffReqDate , 106 ) ) ");
            QryStr.Append("from LaterOff where staffid = @StaffId  and IsCancelled = 0)");

            try
            {
                var lst = context.Database.SqlQuery<ValidLaterOffDates>(QryStr.ToString(),sqlParameter).Select(d => new ValidLaterOffDates()
                {
                    LaterOffReqDate = d.LaterOffReqDate,
                    Validity = d.Validity
                }).ToList();

                if(lst == null)
                {
                    return new List<ValidLaterOffDates>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<ValidLaterOffDates>();
            }
        }
    }
}
