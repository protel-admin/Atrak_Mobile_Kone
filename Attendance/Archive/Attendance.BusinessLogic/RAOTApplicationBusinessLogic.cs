using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using System.Web.Mvc;
using System.Configuration;

namespace Attendance.BusinessLogic
{
    public class RAOTApplicationBusinessLogic
    {
        public List<RAOTRequestApplication> GetAppliedOverTimeRequestForMyTeam(string StaffId)
        {
            using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
            {
                return rAOTApplicationRepository.GetAppliedOverTimeRequestForMyTeam(StaffId);
            }
        }

        public List<PermissionType> GetOTTypes()
        {
            using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
            {
                return rAOTApplicationRepository.GetOTTypes();
            }
        }

        public List<SelectListItem> ConvertPermissionTypesToListItems(List<PermissionType> objListOfLeaveTypes)
        {
            List<SelectListItem> _ListOfLeaveTypes_ = new List<SelectListItem>();
            foreach (var l in objListOfLeaveTypes)
            {
                _ListOfLeaveTypes_.Add(new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                });
            }

            return _ListOfLeaveTypes_;
        }

        public string GetUniqueId()
        {
            using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
            {
                return rAOTApplicationRepository.GetUniqueId();
            }
        }

        public void SaveRequestApplication(ClassesToSave DataToSave)
        {
            using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
            {
                rAOTApplicationRepository.SaveRequestApplication(DataToSave);
            }
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
                        EmailSubject = "Request for OT application sent to "+ OTApprover,
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

        public void CancelApplication(string Id, string StaffId, string Approvalid,string CurrentUser)  
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            string StaffName = string.Empty;
            string ReviewerName = string.Empty;
            string ReportingManagerName = string.Empty;
            string EmailFrom = string.Empty;
            string EmailTo = string.Empty;
            string Reviewer = string.Empty;
            string OTReviwer = string.Empty;
            string OTApprover = string.Empty;
            string ApproverMailId = string.Empty;
            string ReviewerMailId = string.Empty;
            string ApproverName = string.Empty;

            ApplicationApproval AA = new ApplicationApproval();
            OTApplication OTA = new OTApplication();

            using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
            {
                AA = rAOTApplicationRepository.GetApplicationApproval(Id);
                OTA = rAOTApplicationRepository.GetOTApplicationDetails(Id);
            }
            ClassesToSave CTS = new ClassesToSave();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
           
            var ACT =new AttendanceControlTable();
            var AD = new AttendanceData();
            var StaffOfficial = new StaffOfficial();
            using (CommonRepository commonRepository = new CommonRepository())
            {
                Reviewer = commonRepository.GetStaffReviewer(StaffId);
                OTReviwer = commonRepository.GetOTReviewer(StaffId);
                OTApprover = commonRepository.GetOTApprover(StaffId);

            //ReviewerName = cm.GetStaffName(Reviewerid);
                ReportingManagerName = commonRepository.GetStaffName(AA.ApprovalOwner);
                EmailFrom = commonRepository.GetEmailFromAdd();
                EmailTo = commonRepository.GetEmailIdOfEmployee(OTA.StaffId);
                ApproverMailId = commonRepository.GetEmailIdOfEmployee(OTApprover);
                ApproverName = commonRepository.GetStaffName(Approvalid);
                ReviewerMailId = commonRepository.GetEmailIdOfEmployee(OTReviwer);
                    }
            //StaffName = cm.GetStaffName(Obj.StaffId);
            //var Obj = repo.GetRequestApplicationDetails(Id);

            //
            //Check whether the starting date of the leave application is below the current date.
            //var IsFutureDate = IsFromDateMoreOrEqualToCurrerntDate(Obj.StartDate, DateTime.Now);
            //
            //If the leave application date is future to the current date.
            //if (IsFutureDate == true)
            //{
            //Check if the leave application has been approved or not.
            if (AA.ApprovalStatusId == 1)    //If the leave application has not been approved. (i.e. in the pending state) then...
            {
                    //Check if the leave application has already been cancelled or not.
                    if (OTA.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                       //Cancel the leave application which is in pending state.
                       if (CurrentUser == OTReviwer)
                       {
                           OTA.IsCancelled = true;
                           OTA.CreatedOn = DateTime.Now;
                           OTA.CreatedBy = Approvalid;
                           AA.Id = Id;
                           AA.ApprovalStatusId = 4;

                           ESL.Add(new EmailSendLog
                           {
                               //From = cm.GetEmailIdOfEmployee(ReportingManagerId) ?? string.Empty,
                               From = EmailFrom,
                               To = EmailTo ?? "-",
                               CC = string.Empty,
                               BCC = string.Empty,
                               EmailSubject = "Requested OT application status",
                               EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your OT  application for the date " + Convert.ToDateTime(OTA.OTDate).ToString("dd-MMM-yyyy") + " has been Canceled..</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ApproverName  + " &nbsp;(" + Approvalid + ")</p></body></html>",
                               CreatedOn = DateTime.Now,
                               CreatedBy = CurrentUser,
                               IsSent = false,
                               SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                               IsError = false,
                               ErrorDescription = "-",
                               SentCounter = 0
                           });
                           ESL.Add(new EmailSendLog
                           {
                               //From = cm.GetEmailIdOfEmployee(ReportingManagerId) ?? string.Empty,
                               From = EmailFrom,
                               To = ApproverMailId ?? "-",
                               CC = string.Empty,
                               BCC = string.Empty,
                               EmailSubject = "Requested OT application status",
                               EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your OT  application for the date " + Convert.ToDateTime(OTA.OTDate).ToString("dd-MMM-yyyy") + " has been Canceled..</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ApproverName + " &nbsp;(" + Approvalid + ")</p></body></html>",
                               CreatedOn = DateTime.Now,
                               CreatedBy = CurrentUser,
                               IsSent = false,
                               SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                               IsError = false,
                               ErrorDescription = "-",
                               SentCounter = 0
                           });
                       }                    
                       else if (CurrentUser == OTReviwer)
                       {
                           OTA.IsCancelled = true;
                           OTA.CreatedOn = DateTime.Now;
                           OTA.CreatedBy = Approvalid;
                           AA.Id = Id;
                           AA.ApprovalStatusId = 4;
                           ESL.Add(new EmailSendLog
                           {
                               //From = cm.GetEmailIdOfEmployee(ReportingManagerId) ?? string.Empty,
                               From = EmailFrom,
                               To = EmailTo ?? "-",
                               CC = string.Empty,
                               BCC = string.Empty,
                               EmailSubject = "Requested OT application status",
                               EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your OT  application for the date " + Convert.ToDateTime(OTA.OTDate).ToString("dd-MMM-yyyy") + " has been Canceled..</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ApproverName + " &nbsp;(" + Approvalid + ")</p></body></html>",
                               CreatedOn = DateTime.Now,
                               CreatedBy = CurrentUser,
                               IsSent = false,
                               SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                               IsError = false,
                               ErrorDescription = "-",
                               SentCounter = 0
                           });
                           ESL.Add(new EmailSendLog
                           {
                               //From = cm.GetEmailIdOfEmployee(ReportingManagerId) ?? string.Empty,
                               From = EmailFrom,
                               To = ReviewerMailId ?? "-",
                               CC = string.Empty,
                               BCC = string.Empty,
                               EmailSubject = "Requested OT application status",
                               EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">A request OT  application for the date " + Convert.ToDateTime(OTA.OTDate).ToString("dd-MMM-yyyy") + " has been Canceled..</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ApproverName + " &nbsp;(" + Approvalid + ")</p></body></html>",
                               CreatedOn = DateTime.Now,
                               CreatedBy = CurrentUser,
                               IsSent = false,
                               SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                               IsError = false,
                               ErrorDescription = "-",
                               SentCounter = 0
                           });
                          
                       }                    
                   
                        //CTS.RA = Obj;
                        CTS.ESL = ESL;
                        CTS.OTA = OTA;
                        CTS.AA = AA;
                    using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
                    {
                        rAOTApplicationRepository.CancelApplication(CTS);
                    }
                    }
                    else   //If the leave application has already been cancelled then...
                    {
                        //throw exception (first of all the cancel link must not be visible.)
                        throw new Exception("You cannot cancel a OT request that is already been cancelled.");
                    }
            }
            else
            {
                if (AA.ApprovalStatusId == 2)
                {
                    OTA.IsCancelled = true;
                    OTA.CreatedOn = DateTime.Now;
                    OTA.CreatedBy = Approvalid;

                    AD.IsOTValid = false;
                    ACT.StaffId = StaffId;
                    ACT.FromDate = OTA.InTime;
                    ACT.ToDate = OTA.OutTime;
                    ACT.IsProcessed = false;
                    ACT.CreatedOn = DateTime.Now;
                    ACT.CreatedBy = CurrentUser;
                    ACT.ApplicationType = "ISCANCELLED";
                    ACT.ApplicationId = Id;

                    ESL.Add(new EmailSendLog
                    {
                        //From = cm.GetEmailIdOfEmployee(ReportingManagerId) ?? string.Empty,
                        From = EmailFrom,
                        To = EmailTo ?? "-",
                        CC = string.Empty,
                        BCC = string.Empty,
                        EmailSubject = "Requested OT Cancellation application status",
                        EmailBody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your Approved OT application for the date " + Convert.ToDateTime(OTA.OTDate).ToString("dd-MMM-yyyy") + " has been Canceled..</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ApproverName + " &nbsp;(" + Approvalid + ")</p></body></html>",
                        CreatedOn = DateTime.Now,
                        CreatedBy = CurrentUser,
                        IsSent = false,
                        SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                        IsError = false,
                        ErrorDescription = "-",
                        SentCounter = 0
                    });

                    CTS.ESL = ESL;
                    CTS.OTA = OTA;
                    CTS.ACT = ACT;
                    CTS.AD  = AD;
                    using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
                    {
                        rAOTApplicationRepository.CancelApplication(CTS);
                    }
                }
                else   //If the leave application has already been cancelled then...
                {
                    //throw exception (first of all the cancel link must not be visible.)
                    throw new Exception("You cannot cancel a OT request that is already been cancelled.");
                }
            }
                //else//If the leave application has been approved then...
                //{
                //    //Check if the leave application has already been cancelled or not.
                //    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                //    {
                //        //Cancel the leave application which is in approved state.
                //        Obj.IsCancelled = true;
                //        Obj.CancelledDate = DateTime.Now;

                //        //Credit back the leave balance that was deducted.
                //        //EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
                //        //ELA.StaffId = Obj.StaffId;
                //        //ELA.LeaveTypeId = Obj.LeaveTypeId;
                //        //ELA.TransactionFlag = 1;
                //        //ELA.TransactionDate = DateTime.Now;
                //        //ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays);
                //        //ELA.Narration = "Cancelled the pending leave application.";
                //        //ELA.RefId = Obj.Id;
                //        //
                //        CTS.RA = Obj;
                //        //CTS.ELA = ELA;
                //        repo.CancelApplication(CTS);
                //    }
                //    else //If the leave application has already been cancelled then...
                //    {
                //        //throw exception.
                //        throw new Exception("You cannot cancel a OT request that is already been cancelled.");
                //    }
                //}
            //}
            //else  //If the leave application is a past date then...
            //{
                //Check if the leave application has been approved or not.
                //if (Obj.IsApproved.Equals(false))    //If the leave application has not been approved. (i.e. in the pending state) then...
                //{
                //    //Check if the leave application has already been cancelled or not.
                //    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                //    {
                //        //Cancel the leave application which is in pending state.
                //        Obj.IsCancelled = true;
                //        Obj.CancelledDate = DateTime.Now;

                //        CTS.RA = Obj;
                //        repo.CancelApplication(CTS);
                //    }
                //    else //If the leave application has already been cancelled then...
                //    {
                //        //throw exception (first of all the cancel link must not be visible for the application that has already been cancelled.)
                //        throw new Exception("You cannot cancel a OT request that is already been cancelled.");
                //    }
                //}
                //else  //If the leave application has been approved then...
                //{
                //    //Check if the leave application has already been cancelled or not.
                //    if (Obj.IsCancelled.Equals(false))//If the leave application has not been cancelled then...
                //    {
                //        //throw exception informing the user that the leave application has to be cancelled by his supervisor.
                //        throw new Exception("You cannot cancel past OT request that have already been approved. It has to be cancelled by your supervisor.");
                //    }
                //    else  //If the leave application has already been cancelled then...
                //    {
                //        //throw exception.
                //        throw new Exception("You cannot cancel a OT request that is already been cancelled.");
                //    }
                //}
            //}
        }

        public string CancelApprovedApplication(string Id)
        {
            string staffId = string.Empty;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            string applicationId = string.Empty;
            string applicationType = string.Empty;
            string punchtype = string.Empty;
            //Get the leave application details based on the Id passed to this function as a parameter.
            RequestApplication Obj = new RequestApplication();
            ClassesToSave CTS = new ClassesToSave();

            using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
            {
                Obj = rAOTApplicationRepository.GetRequestApplicationDetails(Id);
            }
            //
            //Check whether the starting date of the leave application is below the current date.
            var IsFutureDate = IsFromDateMoreOrEqualToCurrerntDate(Obj.StartDate, DateTime.Now);
            //
            //If the leave application date is future to the current date.
            if (IsFutureDate == true)
            {
                //Check if the leave application has been approved or not.
                if (Obj.IsApproved.Equals(false))    //If the leave application has not been approved. (i.e. in the pending state) then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in pending state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                        CTS.RA = Obj;
                        using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
                        {
                            rAOTApplicationRepository.CancelApplication(CTS);
                        }
                    }
                    else   //If the leave application has already been cancelled then...
                    {
                        //throw exception (first of all the cancel link must not be visible.)
                        throw new Exception("You cannot cancel a permission request that is already been cancelled.");
                    }
                }
                else//If the leave application has been approved then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in approved state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;

                        CTS.RA = Obj;

                        using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
                        {
                            rAOTApplicationRepository.CancelApplication(CTS);
                        }
                    }
                    else //If the leave application has already been cancelled then...
                    {
                        //throw exception.
                        throw new Exception("You cannot cancel a permission request that is already been cancelled.");
                    }
                }
            }
            else  //If the leave application is a past date then...
            {
                //Check if the leave application has been approved or not.
                if (Obj.IsApproved.Equals(true))    //If the leave application has not been approved. (i.e. in the pending state) then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in pending state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;

                        CTS.RA = Obj;
                        using (RAOTApplicationRepository rAOTApplicationRepository = new RAOTApplicationRepository())
                        {
                            rAOTApplicationRepository.CancelApplication(CTS);
                        }

                        CommonRepository obj = new CommonRepository();

                        try
                        {
                            var data = obj.GetList(Obj.Id);
                            staffId = data.StaffId;
                            fromDate = data.FromDate;
                            toDate = data.ToDate;
                            applicationType = Obj.RequestApplicationType;
                            applicationId = Obj.Id;
                        }
                        catch (Exception err)
                        {
                            throw err;
                        }
                        if (fromDate < currentDate)
                        {
                            if (toDate >= currentDate)
                            {
                                toDate = DateTime.Now.AddDays(-1);
                            }
                            obj.LogIntoIntoAttendanceControlTable(staffId, fromDate, toDate, applicationType, applicationId);
                        }
                    }
                    else //If the leave application has already been cancelled then...
                    {
                        //throw exception (first of all the cancel link must not be visible for the application that has already been cancelled.)
                        throw new Exception("You cannot cancel a permission request that is already been cancelled.");
                    }
                }
            }

            return "OK";

        }


        private bool IsFromDateMoreOrEqualToCurrerntDate(DateTime? LeaveStartDate, DateTime? CurrentDate)
        {
            //TimeSpan TS1 = new TimeSpan();
            //TS1 = LeaveStartDate;


            if (LeaveStartDate.Value.Date < CurrentDate.Value.Date)
            {
                return false;
            }
            else if (LeaveStartDate.Value.Date >= CurrentDate.Value.Date)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
