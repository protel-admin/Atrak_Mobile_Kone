using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using Attendance.Model;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace AtrakMobileApi.Helpers
{
    public class LeaveApplicationHelper
    {
        public void FromDateShouldBeLessThanToDate(DateTime FromDate, DateTime ToDate)
        {
            if (FromDate > ToDate)
            {
                throw new Exception("Starting date of your application must be less than the Ending datey.");
            }
        }
        public void ValidateNewLeaveApplication(string StaffId,string LocationId,string SecurityGroupId, LeaveRequest_Dto leaveApplication)
        {
            RALeaveApplicationBusinessLogic BL = new RALeaveApplicationBusinessLogic();

            CommonBusinessLogic CB = new CommonBusinessLogic();
            var bl = new IndividualLeaveCreditDebitBusinessLogic();
             
                ValidateApplicationForPayDate(StaffId,Convert.ToDateTime(leaveApplication.StartDate), Convert.ToDateTime(leaveApplication.EndDate));
            FromDateShouldBeLessThanToDate(Convert.ToDateTime(leaveApplication.StartDate), Convert.ToDateTime(leaveApplication.EndDate));

                //Validate leave balance.
                if (leaveApplication.LeaveTypeId != "LV0036" && leaveApplication.LeaveTypeId != "LV0038" && leaveApplication.LeaveTypeId != "LV0039")
                {
                    BL.ValidateLeaveBalance(leaveApplication.StaffId, leaveApplication.LeaveTypeId, Convert.ToDecimal(leaveApplication.TotalDays));
                }

                string continuityMessage = string.Empty;
                continuityMessage = ValidateDurationContinuityForMultipleDaysRequest("Leave", leaveApplication.StartDate, Convert.ToInt32(leaveApplication.LeaveStartDurationId), leaveApplication.EndDate, Convert.ToInt32(leaveApplication.LeaveEndDurationId));
                if (!string.IsNullOrEmpty(continuityMessage))
                {
                    throw new Exception(continuityMessage);
                }
        }

        #region PayPeriodValidation
        //validation for pay cycle date range
        public void ValidateApplicationForPayDate(string StaffId,DateTime ApplicationStartDate, DateTime ApplicationEndDate)
        {
            string applicationRestriction = string.Empty;
            applicationRestriction = ConfigurationManager.AppSettings["EnableApplicationRestrictionForPayGeneration"].ToString().ToUpper().Trim();
            string message = "valid";
            if (applicationRestriction == "YES")
            {
                if (ApplicationStartDate.Date < DateTime.Now.Date)
                {
                    var cbl = new CommonBusinessLogic();
                    message = cbl.ValidateApplicationForPayDate(StaffId,ApplicationStartDate, ApplicationEndDate);
                    if (message != "VALID")
                    {
                        throw new Exception("Please select the date between the pay cycle or above");
                    }
                }
            }
        }
        #endregion

        public string ValidateDurationContinuityForMultipleDaysRequest(string ApplicationType, string FromDate,
          int StartDurationId, string ToDate, int EndDurationId)
        {
            string message = string.Empty;

            if (StartDurationId == 1 && EndDurationId == 3)
            {
                message = " Please select valid " + ApplicationType + " start & end duration";
            }
            else if (StartDurationId == 2 && EndDurationId == 1)
            {
                message = " Please select valid " + ApplicationType + " start & end duration";
            }
            else if (StartDurationId == 2 && EndDurationId == 2 && FromDate != ToDate)
            {
                message = " Please select valid " + ApplicationType + " start & end duration";
            }
            else if (StartDurationId == 2 && EndDurationId == 3)
            {
                message = " Please select valid " + ApplicationType + " start & end duration";
            }
            else if (StartDurationId == 3 && EndDurationId == 3 && FromDate != ToDate)
            {
                message = " Please select valid " + ApplicationType + " start & end duration";
            }
            return message;
        }

       public static ClassesToSave GetClassesToSave(LeaveRequest_Dto LeaveReqDto, UserClaims userClaims)
        {
            RALeaveApplicationBusinessLogic BL = new RALeaveApplicationBusinessLogic();
            ClassesToSave CTS = new ClassesToSave();
            //insert into Request Application Table.
            CommonBusinessLogic CB = new CommonBusinessLogic();
            RequestApplication RA = new RequestApplication();
            RA.Id = BL.GetUniqueId();
            RA.StaffId = LeaveReqDto.StaffId;
            RA.LeaveTypeId = LeaveReqDto.LeaveTypeId;
            RA.LeaveStartDurationId = Convert.ToInt16(LeaveReqDto.LeaveStartDurationId);
            RA.LeaveEndDurationId = Convert.ToInt16(LeaveReqDto.LeaveEndDurationId);

            //startDate.ToString("dd") + "-" + startDate.ToString("MMM") + "-" + startDate.ToString("yyyy");
           // var format = "dd-MMM-yyyy";
            var  startDate = Convert.ToDateTime(LeaveReqDto.StartDate);
            var endDate = Convert.ToDateTime(LeaveReqDto.EndDate);

            RA.StartDate = startDate; // Convert.ToDateTime(LeaveReqDto.StartDate);
            RA.EndDate = endDate; // Convert.ToDateTime(LeaveReqDto.EndDate);
          
            RA.TotalDays = (LeaveReqDto.TotalDays);
            RA.ContactNumber = LeaveReqDto.ContactNbr;
            RA.Remarks = LeaveReqDto.Reason;
            RA.ReasonId = 0;
            
            RA.IsCancelled = false;
            RA.IsApproved = false;
            RA.IsRejected = false;
            RA.ApplicationDate = DateTime.Now;
            RA.AppliedBy = userClaims.StaffId;
            RA.RequestApplicationType = "LA";
            RA.IsCancelApprovalRequired = false;
            RA.IsCancelApproved = false;
            RA.IsCancelRejected = false;

            // Insert Into Application Approval Table.
            ApplicationApproval AA = new ApplicationApproval();
            AA.Id = BL.GetUniqueId();
            AA.ParentId = RA.Id;
            AA.ApprovalStatusId = 1;
            AA.ApprovedBy = null;
            AA.ApprovedOn = null;
            AA.Comment = null;
            AA.ApprovalOwner = userClaims.ApprovalOwner;
            AA.Approval2Owner = userClaims.ReviewerOwner;
            AA.Approval2statusId = 1;
            AA.Approval2By = null;
            AA.Approval2On = null;
            AA.ParentType = "LA";
            AA.ForwardCounter = 1;
            AA.ApplicationDate = RA.ApplicationDate;
            // Insert into Email Send Log Table.
            
      
            CTS.RA = RA;
            CTS.AA = AA;
            CTS.ESL = null;
            CTS.APA = null;

            return CTS;
        }
    }
}