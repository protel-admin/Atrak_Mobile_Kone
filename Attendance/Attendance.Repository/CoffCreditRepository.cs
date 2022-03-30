using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class CoffCreditRepository :IDisposable
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

        public CoffCreditRepository()
        {
            context = new AttendanceManagementContext();

        }

        public List<CoffReqDates> GetAllOTDates(string Staffid, string FromDate, string ToDate)
        {
            var qrystr = new StringBuilder();
            qrystr.Clear();
            qrystr.Append("EXEC [DBO].[GETCOFFDATES] '" + Staffid + "', '" + FromDate + "', '" + ToDate + "'");
            var data = context.Database.SqlQuery<CoffReqDates>(qrystr.ToString()).ToList();
            if (data != null)
            {
                return data;
            }
            else
            {
                List<CoffReqDates> CRD = new List<CoffReqDates>();
                return CRD;
            }

        }

        public List<COffCreditRequest> GetAllCreditList(string staffid)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            if (staffid == "-")
            {
                qryStr.Append("select CR.Id as COffCreditId ,CR.Staffid as StaffId,DBO.FNGETSTAFFNAME(CR.STAFFID) AS FirstName,convert(varchar,CR.CoffReqFrom) as CoffReqFrom,convert(varchar,CR.TotalDays) as TotalDays ,CR.Reason as COffReason, (select Name from approvalstatus where id=aa.approvalstatusid) as ApprovalStatusName  from CoffReq CR inner join applicationapproval AA on CR.id=AA.parentid where AND ISCANCELLED = 0 order by CR.id desc");
            }
            else
            {
                qryStr.Append("select CR.Id as COffCreditId,CR.Staffid as StaffId,DBO.FNGETSTAFFNAME(CR.STAFFID) AS FirstName,convert(varchar,CR.CoffReqFrom ,106) as CoffReqFrom,convert(varchar,CR.TotalDays) as TotalDays,CR.Reason as COffReason, (select Name from approvalstatus where id=aa.approvalstatusid) as ApprovalStatusName  from CoffReq CR inner join applicationapproval AA on CR.id=AA.parentid where CR.staffid= '" + staffid + "' AND ISCANCELLED = 0 order by CR.id desc");

            }
            try
            {

                var lst =
                context.Database.SqlQuery<COffCreditRequest>(qryStr.ToString())
                        .Select(c => new COffCreditRequest()
                        {
                            COffCreditId = c.COffCreditId,
                            FirstName = c.FirstName,
                            StaffId = c.StaffId,
                            CoffReqFrom = c.CoffReqFrom,
                            COffReason = c.COffReason,
                            TotalDays = c.TotalDays,
                            ApprovalStatusName = c.ApprovalStatusName
                        }).ToList();

                if (lst == null)
                {
                    return new List<COffCreditRequest>();
                }
                else
                {
                    return lst;
                }

            }
            catch (Exception)
            {
                return new List<COffCreditRequest>();
            }
        }
        
        public string VaidateOnBehalfCompOffCredit(string StaffId, string WorkedDate)
        {
            string validationMessage = string.Empty;
            var queryString = new StringBuilder();
            try
            {

                queryString.Clear();
                queryString.Append("Select Top 1 StaffId from [CoffReq] where StaffId = '" + StaffId + "' AND Convert(Date,CoffReqFrom) = Convert(Date,'" + WorkedDate + "')");
                validationMessage = context.Database.SqlQuery<string>(queryString.ToString()).FirstOrDefault();

            }
            catch
            {
                validationMessage = "";
            }
            return validationMessage;
        }

        public void SaveOnBehalfCompOffCredit(ClassesToSave classesToSave, string loggedInUser, string userRole)
        {
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
            var repo = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    var qryStr = new StringBuilder();
                    string compOffCreditBy = string.Empty;
                    if (userRole.Equals("1"))
                    {
                        compOffCreditBy = "HR";
                    }
                    else if (userRole.Equals("6"))
                    {
                        compOffCreditBy = "TL";
                    }

                    //SAVE INTO APPLICATION APPROVAL TABLE
                    string ReportingManager = repo.GetReportingManager(classesToSave.RA.StaffId);
                    EmployeeLeaveAccount ela = new EmployeeLeaveAccount();
                    ela.StaffId = classesToSave.RA.StaffId;
                    ela.LeaveTypeId = "LV0005";
                    ela.TransactionFlag = 1;
                    ela.TransactionDate = classesToSave.RA.StartDate;
                    ela.LeaveCount = classesToSave.RA.TotalDays;
                    ela.Narration = "On Behalf Comp-Off credit by - " + loggedInUser + "";
                    ela.RefId = "";
                    ela.IsLapsed = false;
                    ela.FinancialYearStart = DateTime.Now;
                    ela.FinancialYearEnd = DateTime.Now;
                    ela.LeaveCreditDebitReasonId = 4;
                    ela.Year = DateTime.Now.Year;
                    ela.Month = DateTime.Now.Month;
                    ela.IsSystemAction = false;
                    ela.TransactionBy = loggedInUser;
                    context.EmployeeLeaveAccount.AddOrUpdate(ela);
                    context.ApplicationApproval.AddOrUpdate(classesToSave.AA);
                    context.RequestApplication.AddOrUpdate(classesToSave.RA);
                    context.SaveChanges();
                    Trans.Commit();

                    if (compOffCreditBy.Equals("HR"))
                    {
                        var ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
                        var ReportingManagerName = repo.GetStaffName(ReportingManager);
                        string onBehalfApplicantName = repo.GetStaffName(loggedInUser);
                        var StaffName = repo.GetStaffName(classesToSave.RA.StaffId);
                        var StaffEmailId = repo.GetEmailIdOfEmployee(classesToSave.RA.StaffId);
                        var staffid = classesToSave.RA.StaffId;
                        if (string.IsNullOrEmpty(ReportingManagerEmailId).Equals(true)) //if the reporting manager does not have an email id then...
                        {
                            //check if the staff has an email id.
                            if (string.IsNullOrEmpty(StaffEmailId).Equals(true)) //if the staff does not have an email id then...
                            {
                                //do not take any action.
                            }
                            else //if the staff has an email id then...
                            {
                                var EmailStr = string.Empty;
                                EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">&nbsp;&nbsp;Your Comp-Off account has been credited with " + classesToSave.RA.TotalDays + " day for the worked date " + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + onBehalfApplicantName + "&nbsp; ( " + loggedInUser + ")</p></body></html>";
                                repo.SendEmailMessage("", StaffEmailId, "", "", "C-off credit application of " + classesToSave.RA.StaffId + " - " + StaffName, EmailStr);
                            }
                        }
                        else // if the reporting manager has an email id then...
                        {
                            var EmailStr = string.Empty;
                            EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">&nbsp;&nbsp;Your team member " + StaffName + "&nbsp;(" + classesToSave.RA.StaffId + ") Comp-Off account has been credited with " + classesToSave.RA.TotalDays + " day for the worked date " + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + onBehalfApplicantName + " ( " + loggedInUser + ")</p></body></html>";
                            repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Comp-off credit application  of " + StaffName, EmailStr);

                            EmailStr = string.Empty;
                            EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">&nbsp;&nbsp;Your Comp-Off account has been credited with " + classesToSave.RA.TotalDays + " day for the worked date " + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + onBehalfApplicantName + " &nbsp;( " + loggedInUser + ")</p></body></html>";
                            repo.SendEmailMessage("", StaffEmailId, "", "", "Comp-off credit application of " + classesToSave.RA.StaffId + " - " + StaffName, EmailStr);
                        }
                    }
                    else if (compOffCreditBy.Equals("TL"))
                    {
                        var ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
                        var ReportingManagerName = repo.GetStaffName(ReportingManager);
                        string onBehalfApplicantName = repo.GetStaffName(loggedInUser);
                        var StaffName = repo.GetStaffName(classesToSave.RA.StaffId);
                        var StaffEmailId = repo.GetEmailIdOfEmployee(classesToSave.RA.StaffId);
                        var staffid = classesToSave.RA.StaffId;
                        if (string.IsNullOrEmpty(ReportingManagerEmailId).Equals(true)) //if the reporting manager does not have an email id then...
                        {
                            //check if the staff has an email id.
                            if (string.IsNullOrEmpty(StaffEmailId).Equals(true)) //if the staff does not have an email id then...
                            {
                                //do not take any action.
                            }
                            else //if the staff has an email id then...
                            {
                                var EmailStr = string.Empty;
                                EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Comp-Off account has been credited with " + classesToSave.RA.TotalDays + " day for the worked date " + Convert.ToDateTime(classesToSave.RA.StartDate).ToString("dd-MMM-yyyy") + "</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + onBehalfApplicantName + "&nbsp; (" + loggedInUser + ")</p></body></html>";
                                repo.SendEmailMessage("", StaffEmailId, "", "", "Comp-off credit application of " + classesToSave.RA.StaffId + " - " + StaffName, EmailStr);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw e;
                }
            }
        }
        public int GetGradeRank(string StaffId)
        {
            var query = new StringBuilder();
            query.Clear();
            query.Append("Select PeopleSoftCode from Grade Where Id in (Select GradeId from StaffOfficial where StaffId = '" + StaffId + "')");
            int gradeRank = Convert.ToInt16(context.Database.SqlQuery<string>(query.ToString()).FirstOrDefault());
            return gradeRank;
        }
        public string GetCoffReqPeriodRepository(string StaffId)
        {
            string Message = string.Empty;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("select Value from [Rule] R inner join RuleGroupTxn RGT on RGT.ruleid=R.id " +
                    " where R.IsActive=1 and name ='Lapsing Period for Coff' and rulegroupid=(select PolicyId from StaffOfficial where StaffId=@StaffId)");
                Message = context.Database.SqlQuery<string>(builder.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
            return Message;
        }
    }
}