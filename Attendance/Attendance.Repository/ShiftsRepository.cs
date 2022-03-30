using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using AttendanceManagement.Models;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository {
    public class ShiftsRepository : IDisposable
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
        private AttendanceManagementContext context;

        public ShiftsRepository()
        {
            context = new AttendanceManagementContext();
        }
        string message = "";
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
                            "isactive , CreatedOn , CreatedBy from shifts union  select  'LV0011'  as id , 'WEEKLY OFF' as name,'WO' as shortname,  '00:00' as starttime , '00:00' as endtime ,'' as gracelateby,'' as graceearlyby,'' as " +
                            "breakstarttime ,'' as breakendtime,'' as mindayhours,'' as minweekhours,'' as isactive,'' as CreatedOn,'' as CreatedBy  ");

            //qryStr.Append("Select 'LV0011'  as id , 'WO' as ShortName,'WEEKLY OFF' as Name , '00:00' as ShiftInTime , '00:00' as ShiftOutTime" +
            //            " union  Select Id , ShortName , convert ( varchar ( 5 ) , StartTime , 114 ) ShiftInTime ," +
            //             "convert ( varchar ( 5 ) , EndTime , 114 ) ShiftOutTime , Name from SHIFTS WHERE [IsActive]=1  order by Name asc");

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

        public void SaveShiftInformation(Shifts sh)
        {
            MasterRepository a = new MasterRepository();
            if (string.IsNullOrEmpty(sh.Id) == true)
            {
                var maxid = string.Empty;
                var lastid = string.Empty;
                maxid = a.getmaxid("Shifts", "Id", "SH", "", 6, ref lastid);
                sh.Id = maxid;
            }
            context.Shifts.AddOrUpdate(sh);
            context.SaveChanges();
        }
        public string SaveShiftExtensionDetails(ShiftExtensionAndDoubleShift se)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var repo = new CommonRepository();
                    string ReportingManager = string.Empty;
                    string BaseAddress = string.Empty;
                    var EmailStr = string.Empty;
                    string ReportingManagerEmailId = string.Empty;
                    string StaffEmailId = string.Empty;
                    string StaffName = string.Empty;
                    string ReportingManagerName = string.Empty;
                    string OBRMStaffId = string.Empty;
                    string OneAboveReportingManagerEmailId = string.Empty;
                    string OneAboveReportingManagerName = string.Empty;
                    string HRStaffid = string.Empty;
                    string HREmailId = string.Empty;
                    var joinstring = string.Empty;
                    var queryString1 = new StringBuilder();
                    var AAobj = new ApplicationApproval();
                    var maxid = string.Empty;
                    var lastid = string.Empty;
                    var aa = new ApplicationApproval();
                    var mr = new MasterRepository();
                    int shiftExtensionId = 0;
                    ReportingManager = repo.GetReportingManager(se.StaffId);
                    queryString1.Clear();
                    //queryString1.Append(" Update [ShiftExtensionAndDoubleShift] set IsActive = 0 Where  StaffId = '" + se.StaffId + "' AND " +
                    //    " Convert(Date,TxnDate) = Convert(Date,'" + se.TxnDate + "') AND IsActive = 1");
                    //context.Database.ExecuteSqlCommand(queryString1.ToString());
                    context.ShiftExtensionAndDoubleShift.Add(se);

                    queryString1.Clear();
                    try
                    {
                        queryString1.Append(" Select Max(Id) from ShiftExtensionAndDoubleShift");
                        shiftExtensionId = context.Database.SqlQuery<int>(queryString1.ToString()).FirstOrDefault();
                        shiftExtensionId = shiftExtensionId + 1;
                    }
                    catch
                    {
                        shiftExtensionId = 1;
                    }
                    maxid = mr.getmaxid("ApplicationApproval", "id", "AA", "", 10, ref lastid);
                    AAobj.Id = maxid;
                    AAobj.ParentId = shiftExtensionId.ToString();
                    AAobj.ParentType = "SE";
                    AAobj.ApplicationDate = DateTime.Now;
                    AAobj.ApprovalStatusId = 1;
                    AAobj.ForwardCounter = 1;
                    AAobj.Comment = "-";
                    AAobj.ApprovalOwner = ReportingManager;
                    AAobj.ApprovedBy = ReportingManager;
                    AAobj.ApprovedOn = DateTime.Now;
                    context.ApplicationApproval.Add(AAobj);

                    context.SaveChanges();

                    if (string.IsNullOrEmpty(ReportingManager).Equals(true))
                    {
                        throw new ApplicationException("Application cannot be submitted because reporting manager has not been configured.");
                    }
                    //get the emailid of the reporting manager.
                    ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
                    //get the emailid of the staff who raises the leave application.
                    StaffEmailId = repo.GetEmailIdOfEmployee(se.StaffId);
                    if (string.IsNullOrEmpty(StaffEmailId).Equals(true))
                    {
                        throw new ApplicationException("Application cannot be submitted because your mail id has not been configured.");
                    }
                    //get the name of the staff.
                    StaffName = repo.GetStaffName(se.StaffId);
                    //get the name of the reporting manager.
                    ReportingManagerName = repo.GetStaffName(ReportingManager);
                    //get the one above reporting manager email id
                    //var OBRMStaffId = repo.GetOneAboveManagerFromTeamHierarchy(law.StaffId);
                    OBRMStaffId = repo.GetReportingManager(ReportingManager);
                    OneAboveReportingManagerEmailId = repo.GetEmailIdOfEmployee(OBRMStaffId);
                    OneAboveReportingManagerName = repo.GetStaffName(OBRMStaffId);
                    HRStaffid = repo.GetHR();
                    HREmailId = repo.GetEmailIdOfEmployee(HRStaffid);
                    BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();


                    if (string.IsNullOrEmpty(OneAboveReportingManagerEmailId) == false)
                    {
                        joinstring = string.Concat(ReportingManagerEmailId, ",", OneAboveReportingManagerEmailId);
                    }

                    var staffid = se.StaffId;

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

                            EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your shift extension application has been submitted to your Reporting Manager (" + ReportingManagerName + ") for Approval.<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.DurationOfHoursExtension + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Before Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursBeforeShift + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">After Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursAfterShift + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>";
                            //send intimation to the staff stating that his/her leave application has been acknowleged 
                            //function call to get the name of the staff and the reporting manager.
                            //  but the reporting manager does not have a email id so no intimation has been sent to him.
                            repo.SendEmailMessage("", StaffEmailId, "", "", "Shift extension application of " + se.StaffId + " - " + StaffName, EmailStr);
                        }
                    }
                    else // if the reporting manager has an email id then...
                    {
                        string OneAboveManagerMailTriggerType = string.Empty;
                        string HRManagerMailTriggerTypeForLeaves = string.Empty;
                        string ShiftExtensionApprovalNotificationForOAM = string.Empty;
                        string OneAboveManagerMailTriggerTypeForShiftExtension = string.Empty;
                        try
                        {
                            OneAboveManagerMailTriggerType = ConfigurationManager.AppSettings["OneAboveManagerMailTriggerType"].ToString().ToUpper().Trim();
                            HRManagerMailTriggerTypeForLeaves = ConfigurationManager.AppSettings["HRManagerMailTriggerTypeForLeaves"].ToString().ToUpper().Trim();
                            ShiftExtensionApprovalNotificationForOAM = ConfigurationManager.AppSettings["ShiftExtensionApprovalNotificationForOAM"].ToString().ToUpper().Trim();
                            OneAboveManagerMailTriggerTypeForShiftExtension = ConfigurationManager.AppSettings["OneAboveManagerMailTriggerTypeForShiftExtension"].ToString().ToUpper().Trim();
                        }
                        catch
                        {
                            OneAboveManagerMailTriggerType = "cc";
                            HRManagerMailTriggerTypeForLeaves = "cc";
                            ShiftExtensionApprovalNotificationForOAM = "No";
                            OneAboveManagerMailTriggerTypeForShiftExtension = "CC";
                        }
                        if (ShiftExtensionApprovalNotificationForOAM.Equals("Yes"))
                        {
                            if (OneAboveManagerMailTriggerTypeForShiftExtension.Equals("CC"))
                            {
                                EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a shift extension. Shift extension details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(se.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr>   <tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.DurationOfHoursExtension + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Before Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursBeforeShift + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">After Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursAfterShift + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required for this application.</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">10.114.76.61:8011</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " &nbsp;(" + staffid + ")</p></body></html>";
                                // send intimation to the reporting manager about the shift extension application.
                                repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Shift extension application of " + StaffName, EmailStr);

                                EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + OneAboveReportingManagerName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a shift extension. Shift extension details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(se.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr>   <tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.DurationOfHoursExtension + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Before Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursBeforeShift + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">After Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursAfterShift + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " &nbsp;(" + staffid + ")</p></body></html>";
                                // Send CC to the one above manager about the shift extension application.
                                repo.SendEmailMessage(StaffEmailId, "", OneAboveReportingManagerEmailId, "", "Shift extension application of " + StaffName, EmailStr);

                                // send acknowledgement to the staff who raised the shift extension application.
                                EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your shift extension application has been submitted to your Reporting Manager (" + ReportingManagerName + ") for Approval.<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(se.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.DurationOfHoursExtension + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Before Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursBeforeShift + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">After Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursAfterShift + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>";
                                repo.SendEmailMessage("", StaffEmailId, "", "", "Shift extension application sent to " + ReportingManagerName, EmailStr);
                            }
                            else if (OneAboveManagerMailTriggerTypeForShiftExtension.Equals("TO"))
                            {
                                EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + OneAboveReportingManagerName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a shift extension. Shift extension details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(se.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr>   <tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.DurationOfHoursExtension + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Before Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursBeforeShift + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">After Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursAfterShift + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required for this application.</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">10.114.76.61:8011</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " &nbsp;(" + staffid + ")</p></body></html>";
                                // send intimation to the one above manager about the shift extension application.
                                repo.SendEmailMessage(StaffEmailId, OneAboveReportingManagerEmailId, "", "", "Shift extension application of " + StaffName, EmailStr);

                                EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a shift extension. Shift extension details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(se.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr>   <tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.DurationOfHoursExtension + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Before Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursBeforeShift + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">After Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursAfterShift + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " &nbsp;(" + staffid + ")</p></body></html>";
                                // Send CC to the reporting manager about the shift extension application.
                                repo.SendEmailMessage(StaffEmailId, "", ReportingManagerEmailId, "", "Shift extension application of " + StaffName, EmailStr);

                                // send acknowledgement to the staff who raised the shift extension application.
                                EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your shift extension application has been submitted to your Reporting Manager (" + ReportingManagerName + ") for Approval.<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(se.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.DurationOfHoursExtension + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Before Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursBeforeShift + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">After Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursAfterShift + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>";
                                repo.SendEmailMessage("", StaffEmailId, "", "", "Shift extension application sent to " + OneAboveReportingManagerName, EmailStr);
                            }
                        }

                        else
                        {
                            EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a shift extension. Shift extension details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(se.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.DurationOfHoursExtension + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Before Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursBeforeShift + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">After Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursAfterShift + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required for this application.</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">10.114.76.61:8011</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " &nbsp;(" + staffid + ")</p></body></html>";
                            // send intimation to the reporting manager about the shift extension application.
                            repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Shift extension application of " + StaffName, EmailStr);

                            // send acknowledgement to the staff who raised the shift extension application.
                            EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your shift extension application has been submitted to your Reporting Manager (" + ReportingManagerName + ") for Approval.<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(se.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Duration:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.DurationOfHoursExtension + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Before Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursBeforeShift + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">After Shift Hours:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + se.NoOfHoursAfterShift + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>";
                            repo.SendEmailMessage("", StaffEmailId, "", "", " Shift extension application sent to " + ReportingManagerName, EmailStr);
                        }
                    }
                    transaction.Commit();
                    message = "Success";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return message;
        }
    }
}
