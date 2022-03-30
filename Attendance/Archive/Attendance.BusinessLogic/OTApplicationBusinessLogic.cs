using System;
using System.Collections.Generic;
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
            using (OTApplicationRepository oTApplicationRepository = new OTApplicationRepository())
            {
                return ConvertOTApplicationListToJSon(oTApplicationRepository.LoadOTApplications());
            }
        }

        public string SaveInformation(List<OTBulkUpload> OTP,string LogedInUser)
        {
            using (OTApplicationRepository oTApplicationRepository = new OTApplicationRepository())
            {
                return oTApplicationRepository.SaveInformation(OTP, LogedInUser);
            }
        }

        public void SaveInformation(ClassesToSaveforOT ota, string SolidLine)
        {
            using (OTApplicationRepository oTApplicationRepository = new OTApplicationRepository())
            {
                 oTApplicationRepository.SaveInformation(ota, SolidLine);
            }
        }

        //public string GetReviewer()
        //{
        //    var repo = new RAOnDutyApplicationRepository();
        //    return repo.GetUniqueId();
        //}

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

        public void ApproveApplication(string Id, string ReportingManagerId)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            OTApplication oTApplication = new OTApplication();
            ApplicationApproval applicationApproval = new ApplicationApproval();

            using (OTApplicationRepository oTApplicationRepository = new OTApplicationRepository())
            {
                oTApplication = oTApplicationRepository.GetOTApplicationDetails(Id);
            
                applicationApproval = oTApplicationRepository.GetApplicationApproval(Id);
            }
            
            
            EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            //first send acknowledgement email to the user.
            string ReportingManagerName = string.Empty;
            string ReviewerName = string.Empty;
            string StaffName = string.Empty;
            string LocationId = string.Empty;
            string ApproverOwner = string.Empty;
            string ReviewerOwner = string.Empty;
            string EmailFrom = string.Empty;
            string EmailTo = string.Empty;
            string CCAddress = string.Empty;
            string ReviewerMail = string.Empty;
            string StaffEmail = string.Empty;
            string ApproverMail = string.Empty;

            using (CommonRepository commonRepository = new CommonRepository())
            {
                LocationId = commonRepository.GetLocationId(oTApplication.StaffId);
                ApproverOwner = commonRepository.GetApproverOwner(Id);
                ReviewerOwner = commonRepository.GetReviewerOwner(Id);
                ReportingManagerName = commonRepository.GetStaffName(ApproverOwner);
                ReviewerName = commonRepository.GetStaffName(ReviewerOwner);
                StaffName = commonRepository.GetStaffName(oTApplication.StaffId);
                EmailFrom = commonRepository.GetEmailFromAdd();
                EmailTo = commonRepository.GetEmailIdOfEmployee(oTApplication.StaffId);
                CCAddress = commonRepository.GetCCAddress("OVERTIME", LocationId);
                ReviewerMail = commonRepository.GetEmailIdOfEmployee(ReviewerOwner);
                StaffEmail = commonRepository.GetEmailIdOfEmployee(oTApplication.StaffId);
                ApproverMail = commonRepository.GetEmailIdOfEmployee(ApproverOwner);
            }
             ClassesToSaveforOT CTS = new ClassesToSaveforOT();


             if (ApproverOwner == ReportingManagerId && ReviewerOwner == ReportingManagerId)
             {

                 applicationApproval.ApprovalStatusId = 2;
                 applicationApproval.ApprovedBy = ReportingManagerId;
                 applicationApproval.ApprovedOn = DateTime.Now;
                 applicationApproval.ReviewerstatusId = 2;
                 applicationApproval.ReviewedBy = ReportingManagerId;
                 applicationApproval.ReviewedOn = DateTime.Now;
                applicationApproval.Comment = "APPROVED THE OT REQUEST.";

                 var mailbody = string.Empty;
                 {
                     mailbody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your OverTime application on " + Convert.ToDateTime(oTApplication.OTDate).ToString("dd-MMM-yyyy") + "  for " + oTApplication.OTTime + "  has been approved.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ReportingManagerName + " &nbsp;(" + ReportingManagerId + ")</p></body></html>";
                 }

                 ESL.Add(new EmailSendLog
                 {
                     //From = cm.GetEmailIdOfEmployee(ReportingManagerId) ?? string.Empty,
                     From = EmailFrom,
                     To = EmailTo ?? "-",
                     CC = CCAddress,
                     BCC = string.Empty,
                     EmailSubject = "Requested OverTime application status",
                     EmailBody = mailbody,
                     CreatedOn = DateTime.Now,
                     CreatedBy = ReportingManagerId,
                     IsSent = false,
                     SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                     IsError = false,
                     ErrorDescription = "-",
                     SentCounter = 0
                 });

                 //if (LocationId == "LO0005")
                 //{
                 //    int OTDuration = int.Parse(Obj.OTTime);

                 //    ELA.StaffId = Obj.StaffId;
                 //    ELA.LeaveTypeId = "LV0005";
                 //    ELA.TransactionFlag = 1;
                 //    ELA.TransactionDate = DateTime.Now;
                 //    if (OTDuration >= 4 && OTDuration <= 7)
                 //    {
                 //        ELA.LeaveCount = Convert.ToDecimal(0.50);
                 //    }
                 //    else if (OTDuration > 7)
                 //    {
                 //        ELA.LeaveCount = Convert.ToDecimal(1);
                 //    }
                 //    ELA.Narration = "OVERTIME - APPLICATION CREDIT BY ";
                 //    ELA.LeaveCreditDebitReasonId = 28;
                 //}


                 CTS.AA = applicationApproval;
                 CTS.ESL = ESL;
                 CTS.ELA = ELA;
                 CTS.OTP = oTApplication;
                using (OTApplicationRepository oTApplicationRepository = new OTApplicationRepository())
                {
                    oTApplicationRepository.ApproveApplication(CTS, ReportingManagerId, oTApplication.StaffId);
                }

             }

            if (ApproverOwner == ReportingManagerId && ReviewerOwner != ReportingManagerId)
            {

                applicationApproval.ApprovalStatusId = 2;
                applicationApproval.ApprovedBy = ReportingManagerId;
                applicationApproval.ApprovedOn = DateTime.Now;
                applicationApproval.Comment = "APPROVED THE OT REQUEST.";

                var mailbody = string.Empty;
                {
                    mailbody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your OverTime application on " + Convert.ToDateTime(oTApplication.OTDate).ToString("dd-MMM-yyyy") + "  for " + oTApplication.OTTime + "  has been approved.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ReportingManagerName + " &nbsp;(" + ReportingManagerId + ")</p></body></html>";
                }

                ESL.Add(new EmailSendLog
                {
                    //From = cm.GetEmailIdOfEmployee(ReportingManagerId) ?? string.Empty,
                    From = EmailFrom,
                    To = EmailTo ?? "-",
                    CC = CCAddress,
                    BCC = string.Empty,
                    EmailSubject = "Requested OverTime application status",
                    EmailBody = mailbody,
                    CreatedOn = DateTime.Now,
                    CreatedBy = ReportingManagerId,
                    IsSent = false,
                    SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                    IsError = false,
                    ErrorDescription = "-",
                    SentCounter = 0
                });

               // if(LocationId == "LO0005")
               // {
               //         int OTDuration = int.Parse(Obj.OTTime);

               //         ELA.StaffId = Obj.StaffId;
               //         ELA.LeaveTypeId = "LV0005";
               //         ELA.TransactionFlag = 1;
               //         ELA.TransactionDate = DateTime.Now;
               //         if (OTDuration >= 4 && OTDuration <= 7)
               //         {
               //             ELA.LeaveCount = Convert.ToDecimal(0.50);
               //         }
               //         else if(OTDuration > 7)
               //         {
               //             ELA.LeaveCount = Convert.ToDecimal(1);
               //         }
               //         ELA.Narration = "OVERTIME - APPLICATION CREDIT BY ";
               //         ELA.LeaveCreditDebitReasonId = 28;
               //}

               
                CTS.AA = applicationApproval;
                CTS.ESL = ESL;
                CTS.ELA = ELA;
                CTS.OTP = oTApplication;
                using (OTApplicationRepository oTApplicationRepository = new OTApplicationRepository())
                {
                    oTApplicationRepository.ApproveApplication(CTS, ReportingManagerId, oTApplication.StaffId);

            }
            }

            if (ReviewerOwner == ReportingManagerId && ApproverOwner != ReportingManagerId)
            {


                applicationApproval.ReviewerstatusId = 2;
                applicationApproval.ReviewedBy = ReportingManagerId;
                applicationApproval.ReviewedOn = DateTime.Now;
                applicationApproval.Comment = "REVIEWED THE OT DUTY REQUEST.";

                var mailbody = string.Empty;
                {
                    mailbody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your OverTime application on " + Convert.ToDateTime(oTApplication.OTDate).ToString("dd-MMM-yyyy") + "  for " + oTApplication.OTTime + "  has been Reviewed and sent for an Approval.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ReviewerName + " &nbsp;(" + ReviewerOwner + ")</p></body></html>";
                }

                ESL.Add(new EmailSendLog
                {

                
                    From = ReviewerMail ?? string.Empty,
                    To = StaffEmail ?? "-",
                    CC = string.Empty,
                    BCC = string.Empty,
                    EmailSubject = "Requested OverTime application status",
                    EmailBody = mailbody,
                    CreatedOn = DateTime.Now,
                    CreatedBy = ReportingManagerId,
                    IsSent = false,
                    SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                    IsError = false,
                    ErrorDescription = "-",
                    SentCounter = 0
                });

                var mailbodyA = string.Empty;
                {
                    mailbodyA = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ApproverOwner + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + "  has applied for an OverTime application on " + Convert.ToDateTime(oTApplication.OTDate).ToString("dd-MMM-yyyy") + " for " + oTApplication.OTTime + "  has been Reviewed, Need your Intervention for approval.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ReviewerName + " &nbsp;(" + ReviewerOwner + ")</p></body></html>";
                }


                ESL.Add(new EmailSendLog
                {
                    From = ReviewerMail ?? string.Empty,
                    To = ApproverMail ?? "-",
                    CC = string.Empty,
                    BCC = string.Empty,
                    EmailSubject = "Requested leave application status",
                    EmailBody = mailbodyA,
                    CreatedOn = DateTime.Now,
                    CreatedBy = ReportingManagerId,
                    IsSent = false,
                    SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                    IsError = false,
                    ErrorDescription = "-",
                    SentCounter = 0
                });

                //ClassesToSaveforOT CTS = new ClassesToSaveforOT();
                CTS.AA = applicationApproval;
                CTS.ESL = ESL;
                CTS.ELA = ELA;
                CTS.OTP = oTApplication;
                using (OTApplicationRepository oTApplicationRepository = new OTApplicationRepository())
                {
                    oTApplicationRepository.ApproveApplication(CTS, ReportingManagerId, oTApplication.StaffId);
                }
            }

        }


        public void RejectApplication(string Id, string ReportingManagerId)
        {
            //Get the leave application details based on the id passed to this function as a parameter.
            
            OTApplication oTApplication = new OTApplication();
            using (OTApplicationRepository oTApplicationRepository = new OTApplicationRepository())
            {
                oTApplication = oTApplicationRepository.GetOTApplicationDetails(Id);
            }
            ApplicationApproval applicationApproval = new ApplicationApproval();

            using (OTApplicationRepository oTApplicationRepository = new OTApplicationRepository())
            {
                applicationApproval = oTApplicationRepository.GetApplicationApproval(Id);
            }
           
            List<EmailSendLog> ESL = new List<EmailSendLog>();
            //first send acknowledgement email to the user.
            string ReportingManagerName = string.Empty;
            string StaffName = string.Empty;
            string LocationId = string.Empty;
            string ReviewerName = string.Empty;
            string ApproverOwner = string.Empty;
            string ReviewerOwner = string.Empty;
            string EmailFrom = string.Empty;
            string EmailTo = string.Empty;
            string CCAddress = string.Empty;
            string ReviewerMail = string.Empty;
            string StaffEmail = string.Empty;
            string ApproverMail = string.Empty;

            using (CommonRepository commonRepository = new CommonRepository())
            {
                LocationId = commonRepository.GetLocationId(oTApplication.StaffId);
                ApproverOwner = commonRepository.GetApproverOwner(Id);
                ReviewerOwner = commonRepository.GetReviewerOwner(Id);
                ReportingManagerName = commonRepository.GetStaffName(ApproverOwner);
                ReviewerName = commonRepository.GetStaffName(ReviewerOwner);
                StaffName = commonRepository.GetStaffName(oTApplication.StaffId);
                EmailFrom = commonRepository.GetEmailFromAdd();
                EmailTo = commonRepository.GetEmailIdOfEmployee(oTApplication.StaffId);
                CCAddress = commonRepository.GetCCAddress("OVERTIME", LocationId);
                ReviewerMail = commonRepository.GetEmailIdOfEmployee(ReviewerOwner);
                StaffEmail = commonRepository.GetEmailIdOfEmployee(oTApplication.StaffId);
                ApproverMail = commonRepository.GetEmailIdOfEmployee(ApproverOwner);
            }
            ClassesToSave CTS = new ClassesToSave();

            if (ApproverOwner == ReportingManagerId && ReviewerOwner == ReportingManagerId)
            {
                applicationApproval.ApprovalStatusId = 3;
                applicationApproval.ApprovedBy = ReportingManagerId;
                applicationApproval.ApprovedOn = DateTime.Now;
                applicationApproval.Comment = "OVER TIME REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";

                var mailbody = string.Empty;
                mailbody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your OverTime application on " + Convert.ToDateTime(oTApplication.OTDate).ToString("dd-MMM-yyyy") + " for the duration of " + oTApplication.OTTime + "  has been rejected by the Approver.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ReportingManagerName + " &nbsp;(" + ReportingManagerId + ")</p></body></html>";
                //email from user
                ESL.Add(new EmailSendLog
                {
                    //From = cm.GetEmailIdOfEmployee(ReportingManagerId) ?? string.Empty,
                    From = EmailFrom,
                    To = EmailTo ?? "-",
                    CC = CCAddress,
                    BCC = string.Empty,
                    EmailSubject = "Requested leave application status",
                    EmailBody = mailbody,
                    CreatedOn = DateTime.Now,
                    CreatedBy = ReportingManagerId,
                    IsSent = false,
                    SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                    IsError = false,
                    ErrorDescription = "-",
                    SentCounter = 0
                });


                CTS.AA = applicationApproval;
                CTS.ESL = ESL;
                using (OTApplicationRepository oTApplicationRepository = new OTApplicationRepository())
                {
                    oTApplicationRepository.RejectApplication(CTS, oTApplication.StaffId);
                }
            }

           else if(ApproverOwner ==ReportingManagerId && ReviewerOwner!=ReportingManagerId) 
           {
                applicationApproval.ApprovalStatusId = 3;
                applicationApproval.ApprovedBy = ReportingManagerId;
                applicationApproval.ApprovedOn = DateTime.Now;
                applicationApproval.Comment = "OVER TIME REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";

                var mailbody = string.Empty;                
                 mailbody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your OverTime application on " + Convert.ToDateTime(oTApplication.OTDate).ToString("dd-MMM-yyyy") + " for the duration of " + oTApplication.OTTime + "  has been rejected by the Approver.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ReportingManagerName + " &nbsp;(" + ReportingManagerId + ")</p></body></html>";
                //email from user
                ESL.Add(new EmailSendLog
                {
                    //From = cm.GetEmailIdOfEmployee(ReportingManagerId) ?? string.Empty,
                    From = EmailFrom,
                    To = StaffEmail ?? "-",
                    CC = CCAddress,
                    BCC = string.Empty,
                    EmailSubject = "Requested leave application status",
                    EmailBody = mailbody,
                    CreatedOn = DateTime.Now,
                    CreatedBy = ReportingManagerId,
                    IsSent = false,
                    SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                    IsError = false,
                    ErrorDescription = "-",
                    SentCounter = 0
                });

                
                CTS.AA = applicationApproval;
                CTS.ESL = ESL;
                using (OTApplicationRepository oTApplicationRepository = new OTApplicationRepository())
                {
                    oTApplicationRepository.RejectApplication(CTS, oTApplication.StaffId);
                }
           }

            else if(ReviewerOwner ==ReportingManagerId && ApproverOwner!=ReportingManagerId) 
           {
                applicationApproval.ReviewerstatusId = 3;
                applicationApproval.ReviewedBy = ReportingManagerId;
                applicationApproval.ReviewedOn = DateTime.Now;
                applicationApproval.Comment = "ON DUTY REQUEST HAS BEEN REJECTED BY THE REVIEWER.";

                var mailbody = string.Empty;
                
                    mailbody = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br/><br>Greetings<br/></p><p style=\"font-family:tahoma; font-size:9pt;\">Your On Duty application on " + Convert.ToDateTime(oTApplication.OTDate).ToString("dd-MMM-yyyy") + " for the duration of" + oTApplication.OTTime + "  has been rejected by the Reviewer.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ReportingManagerName + " &nbsp;(" + ReportingManagerId + ")</p></body></html>";

                //email to user
                ESL.Add(new EmailSendLog
                {
                    //From = cm.GetEmailIdOfEmployee(ReportingManagerId) ?? string.Empty,
                    From = EmailFrom,
                    To = StaffEmail ?? "-",
                    CC = string.Empty,
                    BCC = string.Empty,
                    EmailSubject = "Requested leave application status",
                    EmailBody = mailbody,
                    CreatedOn = DateTime.Now,
                    CreatedBy = ReportingManagerId,
                    IsSent = false,
                    SentOn = Convert.ToDateTime("1900-01-01 00:00:00.000"),
                    IsError = false,
                    ErrorDescription = "-",
                    SentCounter = 0
                });
            }
                //ClassesToSave CTS = new ClassesToSave();
                CTS.AA = applicationApproval;
                CTS.ESL = ESL;
            using (OTApplicationRepository oTApplicationRepository = new OTApplicationRepository())
            {
                oTApplicationRepository.RejectApplication(CTS, oTApplication.StaffId);
            }
           }
                
            }
        }
        