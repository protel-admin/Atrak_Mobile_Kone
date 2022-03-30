using Attendance.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository
{
    public class MaintenanceOffRepository : IDisposable
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
        public MaintenanceOffRepository()
        {
            context = new AttendanceManagementContext();
        }

        public string GetFinalDate(string StaffId , string FromDate , string ToDate , int Flag)
        {
            SqlParameter[] sqlParameter = new SqlParameter[4];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            sqlParameter[1] = new SqlParameter("@FromDate", FromDate);
            sqlParameter[2] = new SqlParameter("@ToDate", ToDate);
            sqlParameter[3] = new SqlParameter("@Flag", Flag);

            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT DBO.fnGetFinalDay ( @StaffId,@FromDate,@ToDate@ ,@Flag) AS FINALDAY");

            var str = context.Database.SqlQuery<string>(QryStr.ToString(),sqlParameter).FirstOrDefault().ToString();
            return str;
        }

        //SELECT DBO.fnGetFinalDay ( '111853','30-SEP-2015' , '14-OCT-2015' , 0 )
        public string ValidateMaintenanceOff(string StaffId , string FromDate , string ToDate , bool IsFixed)
        {
            SqlParameter[] sqlParameter = new SqlParameter[4];
            sqlParameter[0] = new SqlParameter("@StaffId", StaffId);
            sqlParameter[1] = new SqlParameter("@FromDate", FromDate);
            sqlParameter[2] = new SqlParameter("@ToDate", ToDate);
           
            var QryStr = new StringBuilder();
            var a = 0 ;
            if (IsFixed == true)
            {
                a = 1;
            }
            sqlParameter[3] = new SqlParameter("@a", a);
            QryStr.Clear();
            QryStr.Append("SELECT DBO.fnValidateMOFFApplication(@StaffId,@FromDate,@ToDate,@a)");

            var str = (context.Database.SqlQuery<string>(QryStr.ToString(),sqlParameter).FirstOrDefault()).ToString();
            return str;
        }

        public List<MOApplicableYear> GetMOffApplicableYear(string id)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@id", id);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT CONVERT ( VARCHAR , MOYEAR ) AS MOYEAR FROM fnGetApplicableMOffYear(@id)");

            try
            {
                var lst = context.Database.SqlQuery<MOApplicableYear>(qryStr.ToString(),sqlParameter).Select(d => new MOApplicableYear()
                {
                    MOYear = d.MOYear,
                }).ToList();

                if(lst == null)
                {
                    return new List<MOApplicableYear>(); 
                }
                else
                {
                    return lst;
                }

            }
            catch(Exception)
            {
                return new List<MOApplicableYear>();
            }
        }

        public void CancelMOffApplication(string ApplicationId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@ApplicationId", ApplicationId);

            var QryStr = new StringBuilder();

            QryStr.Clear();
            QryStr.Append("SELECT COUNT(*) AS TOTALCOUNT FROM MaintenanceOff A INNER JOIN APPLICATIONAPPROVAL B ON A.ID = B.PARENTID WHERE A.id = @ApplicationId ");
            QryStr.Append("AND APPROVALSTATUSID IN ( 2 , 3 ) AND ");
            QryStr.Append("( CONVERT ( DATETIME , CONVERT ( VARCHAR , DATEFROM , 106 ) ) < CONVERT ( DATETIME , CONVERT ( VARCHAR , GETDATE() , 106 ) ) OR  ");
            QryStr.Append("CONVERT ( DATETIME , CONVERT ( VARCHAR , DATETO , 106 ) ) < CONVERT ( DATETIME , CONVERT ( VARCHAR , GETDATE() , 106 ) ) ) ");

            var res2 = context.Database.SqlQuery<int>(QryStr.ToString(),sqlParameter).FirstOrDefault();

            if (Convert.ToInt16(res2) > 0)
            {
                throw new Exception("Cannot cancel past application. To cancel past application it must neither be approved nor rejected.");
            }

            QryStr.Clear();
            QryStr.Append("SELECT CONVERT ( VARCHAR , ISCANCELLED ) AS ISCANCELLED FROM MaintenanceOff WHERE id = @ApplicationId");
            var res1 = context.Database.SqlQuery<string>(QryStr.ToString(),sqlParameter).FirstOrDefault();

            if (res1.Equals("1"))
            {
                throw new Exception("Cannot cancel a cancelled application.");
            }
            
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("UPDATE MAINTENANCEOFF SET ISCANCELLED = 1 WHERE ID = @ApplicationId");

            var lb = new CommonRepository();
            var str = lb.LeaveBalanceHandler(ApplicationId, "Cancel");
            if (str != "OK.")
            {
                throw new Exception(str);
            }
            context.Database.ExecuteSqlCommand(qryStr.ToString(),sqlParameter);
        }

        public bool CanMOffBeOpened()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT CONVERT ( BIT , COUNT(*) ) AS TOTALCOUNT FROM MOFFYEAR WHERE CONVERT ( DATETIME , CONVERT ( VARCHAR , GETDATE() , 106 ) )BETWEEN CONVERT ( DATETIME , CONVERT ( VARCHAR, MOFFSTARTDATE , 106 ) ) AND CONVERT ( DATETIME , CONVERT ( VARCHAR , MOFFENDDATE , 106 ) )");
            var CanOpen = false;
            try
            {
                CanOpen = Convert.ToBoolean(context.Database.SqlQuery<bool>(qryStr.ToString()).FirstOrDefault());
            }
            catch(Exception)
            {
                CanOpen = false;
            }

            return CanOpen;
        }

        public string GetFirstLetterGrade(string id)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@id", id);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT LEFT ( GRADENAME , 1 ) AS FIRSTLETTERGRADE FROM STAFFVIEW WHERE STAFFID = @id");
            var str = string.Empty;
            try
            {
                str = (context.Database.SqlQuery<string>(qryStr.ToString(),sqlParameter).FirstOrDefault()).ToString();
            }
            catch(Exception)
            {
                str = "E";
            }
            
            return str;
        }

        public List<MaintenanceOffList> GetAllMaintenanceOff(string staffid)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@staffid", staffid);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            if (staffid == "-")
            {
                qryStr.Append("select Id , StaffId , DateFrom , "+
                    "DateTo , Reason, ContactNumber , " +
                    "IsCancelled , ApplicationDate , IsFlexible " + 
                    "from maintenanceoff");
            }
            else
            {
                qryStr.Append("select MAINTENANCEOFFID AS Id , StaffId , FIRSTNAME AS StaffName , FROMDATE AS DateFrom , " +
                            "TODATE AS DateTo , MAINTENANCEOFFREASON AS Reason, isnull ( ContactNumber , '-' ) as ContactNumber , " +
                            "ISCANCELLED , ApplicationDate , ISFLEXIBLE , ApprovalStatusName , CONVERT ( VARCHAR , MOFFYEAR ) AS MOFFYEAR " +
                            "from VWMAINTENANCEOFFAPPROVAL where staffid = @staffid AND ISCANCELLED = 'NO'");
            }
            try
            {
                var lst =
               context.Database.SqlQuery<MaintenanceOffList>(qryStr.ToString(),sqlParameter)
                       .Select(c => new MaintenanceOffList()
                       {
                           Id = c.Id,
                           StaffId = c.StaffId,
                           StaffName = c.StaffName,
                           DateFrom = c.DateFrom,
                           DateTo = c.DateTo,
                           Reason = c.Reason,
                           ContactNumber = c.ContactNumber,
                           IsCancelled = c.IsCancelled,
                           IsFlexible = c.IsFlexible,
                           ApprovalStatusName = c.ApprovalStatusName,
                           MOffYear = c.MOffYear
                       }).ToList();

                if (lst == null)
                {
                    return new List<MaintenanceOffList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<MaintenanceOffList>();
            }
        }

        public void SaveMaintenanceOffInfo(MaintenanceOff co)
        {
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
                        SaveMaintenanceOffDetails(co);
                        ReportingManager = repo.GetReportingManager(co.StaffId);
                        if (string.IsNullOrEmpty(ReportingManager) == true)
                        {
                            ReportingManager = co.StaffId;
                            selfapproval = true;
                        }

                        repo.SaveIntoApplicationApproval(co.Id, "MO", co.StaffId, ReportingManager, selfapproval);
                    }
                    else
                    {
                        SaveMaintenanceOffDetails(co);
                    }



                    //##############################################################################################################
                    //CODE BLOCK TO SEND EMAIL INTIMATION TO THE REPORTING MANAGER AND AN ACKNOWLEDGEMENT TO THE SENDER WHO RAISED 
                    //  THE APPLICATION.
                    //##############################################################################################################
                    string BaseAddress = string.Empty;
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
                            EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your maintenance off application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.DateFrom).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.DateTo).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + co.Reason + "</td></tr></table></p><p>But an intimation email could not be sent to your reporting manager because of a missing email id.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                            //send intimation to the staff stating that his/her leave application has been acknowleged 
                            //function call to get the name of the staff and the reporting manager.
                            //  but the reporting manager does not have a email id so no intimation has been sent to him.
                            repo.SendEmailMessage("", StaffEmailId, "", "", "Maintenance off application of " + co.StaffId + " - " + StaffName, EmailStr);
                        }
                    }
                    else // if the reporting manager has an email id then...
                    {
                        var EmailStr = string.Empty;
                        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a maintenance off. Maintenance off details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.DateFrom).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.DateTo).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + co.Reason + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this application.</p><p><a href=\"" + BaseAddress + "MaintenanceOff/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + co.Id + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "MaintenanceOff/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + co.Id + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        // send intimation to the reporting manager about the leave application.
                        repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Maintenance off application of " + StaffName, EmailStr);

                        // send acknowledgement to the staff who raised the leave application.
                        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your maintenance off application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.DateFrom ).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(co.DateTo).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + co.Reason + "</td></tr></table></p><p>This application has been sent also to your reporting manager.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        repo.SendEmailMessage("", StaffEmailId, "", "", "Maintenance off application sent to " + ReportingManagerName, EmailStr);
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


        public void SaveMaintenanceOffDetails(MaintenanceOff co)
        {
            var lastid = string.Empty;
            if (string.IsNullOrEmpty(co.Id) == true)
            {
                var mr = new MasterRepository();
                lastid = mr.getmaxid("maintenanceoff", "Id", "MO", "", 10, ref lastid);
                co.Id = lastid;
            }

            context.MaintenanceOff.AddOrUpdate(co);
            context.SaveChanges();
        }

        public string GetLeaveBalRep(string staffid)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@staffid", staffid);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT WORKINGPATTERN FROM STAFFVIEW WHERE STAFFID = @staffid");

            var bal = (context.Database.SqlQuery<double>(qryStr.ToString(),sqlParameter).FirstOrDefault()).ToString();
            return bal;
        }
        public string GetApproval(string staffid)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@staffid", staffid);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select ass.Name from approvalstatus ass inner join applicationapproval ap on ap.ApprovalStatusId = ass.Id inner join staff st on  @staffid = ap.parentid ");

            var bal = (context.Database.SqlQuery<string>(qryStr.ToString(),sqlParameter).FirstOrDefault()).ToString();
            return bal;
        }
    }
}
