using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using Newtonsoft.Json;

namespace Attendance.BusinessLogic
{
    public class OTApplicationBusinessLogic
    {
        //
        public string LoadOTApplications()
        {
            var repo = new OTApplicationRepository();
            var lst = repo.LoadOTApplications();
            var str = ConvertOTApplicationListToJSon(lst);
            return str;
        }

        public void SaveInformation(ClassesToSaveforOT ota, string SolidLine)
        {
            var repo = new OTApplicationRepository();
            repo.SaveInformation(ota, SolidLine);
        }

        public string SaveInformation(List<OTBulkUpload> OTP, string LogedInUser)
        {
            var repo = new OTApplicationRepository();
            return repo.SaveInformation(OTP, LogedInUser);
        }

        public void RejectApplication(string Id, string ReportingManagerId)
        {
            //Get the leave application details based on the id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();

            using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
            {
                Obj = rAOTApplicationRepository.GetRequestApplicationDetails(Id);
                AA = rAOTApplicationRepository.GetApplicationApproval(Id);

            }
            List<EmailSendLog> ESL = new List<EmailSendLog>();

            OTApplication OTDetails = new OTApplication();
            string StaffName = string.Empty;
            string ReportingManagerName = string.Empty;
            string ApproverOwner = string.Empty;
            string OTReviewer = string.Empty;
            string OTApprover = string.Empty;
            string EmailFrom = string.Empty;
            string EmailTo = string.Empty;

            using (CommonRepository commonRepository = new CommonRepository())
            {
                ReportingManagerName = commonRepository.GetStaffName(ReportingManagerId);
                OTDetails = commonRepository.GetOTDetails(Id);
                StaffName = commonRepository.GetStaffName(Obj.StaffId);
                ApproverOwner = commonRepository.GetApproverOwner(Id);
                OTReviewer = commonRepository.GetOTReviewer(Obj.StaffId);
                OTApprover = commonRepository.GetOTApprover(Obj.StaffId);
                EmailFrom = commonRepository.GetEmailFromAdd();
                EmailTo = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
            }

            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the leave application has been cancelled then...
            {
                //throw exception stating that the cancelled leave application cannot be rejected.
                throw new Exception("Cancelled permission request cannot be rejected.");
            }
            else if (Obj.IsApproved.Equals(true)) //if the leave application has been approved then...
            {
                //throw exception stating that the approved leave application cannot be rejected.
                throw new Exception("Approved permission request cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the leave application has been rejected then...
            {
                //throw exception stating that the rejected leave application cannot be rejected.
                throw new Exception("Rejected permission request cannot be rejected.");
            }
            else //if the leave application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                //reject the application.
                Obj.IsRejected = true;

                AA.ApprovalStatusId = 3;
                AA.ApprovedBy = ReportingManagerId;
                AA.ApprovedOn = DateTime.Now;
                AA.Comment = "OT REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";
                if (ReportingManagerId == OTReviewer)
                {
                    ESL.Add(new EmailSendLog   //mail to User
                    {
                        //From = "sjrobert@protellabs.com",
                        From = EmailFrom,
                        To = EmailTo ?? "-",
                        CC = "-",
                        BCC = "-",
                        EmailSubject = "Request for OT application Status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your request for OT application has been Rejected.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">OTDate:</td><td style=\"width:80%;\">" + OTDetails.OTDate + "</td></tr><tr><td style=\"width:20%;\">OTDuration:</td><td style=\"width:80%;\">" + OTDetails.OTDuration + "</td></tr><tr><td style=\"width:20%;\">Total Days:</td><td style=\"width:80%;\">" + OTDetails.OTReason + "</td></tr></table></p><p></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>",
                        CreatedOn = AA.ApplicationDate,
                        CreatedBy = ReportingManagerId,
                        IsSent = false,
                        SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                else if (ReportingManagerId == OTApprover)
                {
                    ESL.Add(new EmailSendLog   //mail to User
                    {
                        //From = "sjrobert@protellabs.com",
                        From = EmailFrom,
                        To = EmailTo ?? "-",
                        CC = "-",
                        BCC = "-",
                        EmailSubject = "Request for OT application Status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your request for OT application has been Rjected bu Your OT Approver.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">OTDate:</td><td style=\"width:80%;\">" + OTDetails.OTDate + "</td></tr><tr><td style=\"width:20%;\">OTDuration:</td><td style=\"width:80%;\">" + OTDetails.OTDuration + "</td></tr><tr><td style=\"width:20%;\">Total Days:</td><td style=\"width:80%;\">" + OTDetails.OTReason + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>",
                        CreatedOn = AA.ApplicationDate,
                        CreatedBy = ReportingManagerId,
                        IsSent = false,
                        SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }

                ClassesToSave CTS = new ClassesToSave();
                CTS.RA = Obj;
                CTS.AA = AA;
                CTS.ESL = ESL;
                using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
                {
                    rAOTApplicationRepository.RejectApplication(CTS);
                }
                //send rejected mail to the applicant.
            }
        }
        public void ApproveApplication(string Id, string ReportingManagerId)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ApplicationApproval AA = new ApplicationApproval();


            using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
            {
                Obj = rAOTApplicationRepository.GetRequestApplicationDetails(Id);
                AA = rAOTApplicationRepository.GetApplicationApproval(Id);
            }
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            OTApplication OTDetails = new OTApplication();
            string StaffName = string.Empty;
            string ReportingManagerName = string.Empty;
            string ApproverOwner = string.Empty;
            string OTReviewer = string.Empty;
            string OTApprover = string.Empty;
            string BaseAddress = string.Empty;
            string EmailFrom = string.Empty;
            string EmailTo = string.Empty;
            BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
            using (CommonRepository commonRepository = new CommonRepository())
            {
                OTDetails = commonRepository.GetOTDetails(Id);
                StaffName = commonRepository.GetStaffName(Obj.StaffId);
                ApproverOwner = commonRepository.GetApproverOwner(Id);
                OTReviewer = commonRepository.GetOTReviewer(Obj.StaffId);
                OTApprover = commonRepository.GetOTApprover(Obj.StaffId);
                ReportingManagerName = commonRepository.GetStaffName(ReportingManagerId);
                EmailFrom = commonRepository.GetEmailFromAdd();
                EmailTo = commonRepository.GetEmailIdOfEmployee(Obj.StaffId);
            }

            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true)) //if the leave application has been cancelled then...
            {
                //throw exception that a cancelled leave application cannot be approved.
                throw new Exception("Cannot approve a cancelled permission application. Apply for a new leave.");
            }
            else if (Obj.IsApproved.Equals(true)) //if application has already been approved then...
            {
                //throw exception stating that an already approved application cannot be reapproved.
                throw new Exception("Cannot approve already approved permission request.");
            }
            else if (Obj.IsRejected.Equals(true))
            {
                //throw exception stating that an already rejected application cannot be approved.
                throw new Exception("Cannot approve already rejected permission request.");
            }
            else
            {
                ////Get the leave balance based on the employee and the leave type.
                ////var LeaveBalance = repo.GetLeaveBalance(Obj.StaffId, Obj.LeaveTypeId);
                ////Check if the leave balance is more than the total days of leave requested.
                //if (LeaveBalance >= Convert.ToDecimal(Obj.TotalDays)) //if the leave balance is more then...
                //{
                //approve the application.
                Obj.IsApproved = true;

                //update the reporting manager in application Approval.
                //
                //
                AA.ApprovalStatusId = 2;
                AA.ApprovedBy = ReportingManagerId;
                AA.ApprovedOn = DateTime.Now;
                AA.Comment = "APPROVED THE OT REQUEST.";

                //deduct leave balance from employee leave account table.
                //EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
                //ELA.StaffId = Obj.StaffId;
                //ELA.LeaveTypeId = Obj.LeaveTypeId;
                //ELA.TransactionFlag = 2;
                //ELA.TransactionDate = DateTime.Now;
                //ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays) * -1;
                //ELA.Narration = "Approved the leave application.";
                //ELA.RefId = Obj.Id;
                if (ReportingManagerId == OTReviewer)
                {
                    ESL.Add(new EmailSendLog   //mail to User
                    {
                        //From = "sjrobert@protellabs.com",
                        From = EmailFrom,
                        To = EmailTo ?? "-",
                        CC = "-",
                        BCC = "-",
                        EmailSubject = "Request for OT application Status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your request for OT application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">OTDate:</td><td style=\"width:80%;\">" + OTDetails.OTDate + "</td></tr><tr><td style=\"width:20%;\">OTDuration:</td><td style=\"width:80%;\">" + OTDetails.OTDuration + "</td></tr><tr><td style=\"width:20%;\">Total Days:</td><td style=\"width:80%;\">" + OTDetails.OTReason + "</td></tr></table></p><p>This application has been approved send to OTapprover.</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">Click Here</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>",
                        CreatedOn = AA.ApplicationDate,
                        CreatedBy = ReportingManagerId,
                        IsSent = false,
                        SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                    ESL.Add(new EmailSendLog  //mail to OTreviewer
                    {
                        //From = "sjrobert@protellabs.com",
                        From = EmailFrom,
                        To = EmailTo ?? "-",
                        CC = "-",
                        BCC = "-",
                        EmailSubject = "Request for OT application sent to " + OTApprover,
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your request for OT application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">OTDate:</td><td style=\"width:80%;\">" + OTDetails.OTDate + "</td></tr><tr><td style=\"width:20%;\">OTDuration:</td><td style=\"width:80%;\">" + OTDetails.OTDuration + "</td></tr><tr><td style=\"width:20%;\">Total Days:</td><td style=\"width:80%;\">" + OTDetails.OTReason + "</td></tr></table></p><p>OT application has been reviewed,Need your intervention for approving the application .</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">Click Here</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>",
                        CreatedOn = AA.ApplicationDate,
                        CreatedBy = ReportingManagerId,
                        IsSent = false,
                        SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }
                else if (ReportingManagerId == OTApprover)
                {
                    ESL.Add(new EmailSendLog   //mail to User
                    {
                        //From = "sjrobert@protellabs.com",
                        From = EmailFrom,
                        To = EmailTo ?? "-",
                        CC = "-",
                        BCC = "-",
                        EmailSubject = "Request for OT application Status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your request for OT application has been Approved.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">OTDate:</td><td style=\"width:80%;\">" + OTDetails.OTDate + "</td></tr><tr><td style=\"width:20%;\">OTDuration:</td><td style=\"width:80%;\">" + OTDetails.OTDuration + "</td></tr><tr><td style=\"width:20%;\">Total Days:</td><td style=\"width:80%;\">" + OTDetails.OTReason + "</td></tr></table></p><p>This application has been Approved By OTapprover.</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">Click Here</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>",
                        CreatedOn = AA.ApplicationDate,
                        CreatedBy = ReportingManagerId,
                        IsSent = false,
                        SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });
                }

                ClassesToSave CTS = new ClassesToSave();
                CTS.RA = Obj;
                //CTS.ELA = ELA;
                CTS.ESL = ESL;
                CTS.AA = AA;
                using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
                {
                    rAOTApplicationRepository.ApproveApplication(CTS);
                }

                //Get the leave balance once again based on the employee and the leave type.
                //var PostApprovalLeaveBalance = repo.GetLeaveBalance(Obj.StaffId, Obj.LeaveTypeId);
                //check if the leave balance is less than 0 or not.
                //if (LeaveBalance < 0)//if the leave balance is less than 0 then...
                //{
                //    //Reject the application
                //    Obj.IsApproved = false;
                //    Obj.IsRejected = true;

                //    AA.ApprovalStatusId = 3;
                //    AA.ApprovedBy = ReportingManagerId;
                //    AA.ApprovedOn = DateTime.Now;
                //    AA.Comment = "LEAVE REQUEST REJECTED DUE TO INSUFFICIENT LEAVE BALANCE.";

                //    //recredit back the total days debited.
                //    ELA.StaffId = Obj.StaffId;
                //    ELA.LeaveTypeId = Obj.LeaveTypeId;
                //    ELA.TransactionFlag = 1;
                //    ELA.TransactionDate = DateTime.Now;
                //    ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays);
                //    ELA.Narration = "APPROVAL REVERSED DUE TO INSUFFICIENT BALANCE.";
                //    ELA.RefId = Obj.Id;

                //    CTS.RA = Obj;
                //    CTS.ELA = ELA;
                //    CTS.AA = AA;
                //    repo.RejectApplication(CTS);

                //    //send sorry email to the applicant.
                //    //
                //    //
                //}
                //}
                //else
                //{
                //    throw new Exception("The applicant does not meet the defined time limit to approve the permission request.");
                //}
            }
        }
        public string ConvertOTApplicationListToJSon(List<OTApplicationList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var d in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new OTApplicationList()
                {
                    Id = d.Id,
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    OTDate = d.OTDate,
                    OTTime = d.OTTime,
                    OTDuration = d.OTDuration,
                    OTReason = d.OTReason,
                    StatusId = d.StatusId,
                    Status = d.Status,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy
                }));
                jsontemp.Append(",");
            }
            jsonstring = jsontemp.ToString();

            if (string.IsNullOrEmpty(jsonstring) == false)
            {
                if (jsonstring.EndsWith(",") == true)
                {
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
            }
            return "[" + jsonstring + "]";

        }
        /*Self*/
        public string CancelApprovedApplication(string Id, string StaffId, string currentuser, DateTime OTDate)
        {
            OverTimeRepository Repo = new OverTimeRepository();
            return Repo.CancelApprovedApplication(Id, currentuser, StaffId, OTDate);
        }
        /*Self*/
        public List<OTRequestApplicationModel> OverTimeRequestApplication(string StaffId, string AppliedBy)
        {
            OverTimeRepository Repo = new OverTimeRepository();
            var Obj = Repo.OTRequestApplication(StaffId, AppliedBy);
            return Obj;
        }
    }

}
