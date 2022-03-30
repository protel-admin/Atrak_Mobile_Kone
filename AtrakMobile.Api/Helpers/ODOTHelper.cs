using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using Attendance.Repository;
using Attendance.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Helpers
{
    public static class ODBTHelper
    {

        public static void ValidateApplication(OD_Dto odDto)
        {
            RAOnDutyApplicationBusinessLogic BL = new RAOnDutyApplicationBusinessLogic();
            CommonRepository CR = new CommonRepository();
            //over lapping validation
            if (odDto.Duration == "SINGLEDAY")
            {
                if (odDto.ODDate == "" || odDto.ODDate == null)  //validate required field
                {
                    throw new ApplicationException(odDto.ApplicationType + " Date required ");
                }
                else if (odDto.ODStartTime == "" || odDto.ODStartTime == null)
                {
                    throw new ApplicationException(odDto.ApplicationType + " From time required ");
                }
                else if (odDto.ODEndTime == "" || odDto.ODEndTime == null)
                {
                    throw new ApplicationException(odDto.ApplicationType + " To time required ");
                }
                else if (odDto.TotalHours == "" || odDto.TotalHours == null || odDto.TotalHours == "00:00")
                {
                    throw new ApplicationException(" Total hours should not '0' or empty. ");
                }
                var separateval = odDto.TotalHours.Split(':');
                var TotalHours = new TimeSpan(Convert.ToInt32(separateval[0]), Convert.ToInt32(separateval[1]), 0);


            }
            if (odDto.Duration == "MULTIPLEDAY")
            {
                //validate Required Fields
                if (odDto.ODStartDate == null || odDto.ODStartDate == "")
                {
                    throw new ApplicationException(" Please choose the " + odDto.ApplicationType + " start date. ");
                }
                else if (odDto.ODEndDate == null || odDto.ODEndDate == "")
                {
                    throw new ApplicationException(" Please choose the " + odDto.ApplicationType + " end date. ");
                }
                else if (odDto.TotalDays == "0" || odDto.TotalDays == "0.0" || odDto.TotalDays == "0.00")
                {
                    throw new ApplicationException(" Total hours should not '0' or empty. ");
                }
                else if (odDto.ODStartDate == odDto.ODEndDate)
                {
                    //    throw new Exception("Startdate and Enddate should not be same in MultipleDays");
                    throw new Exception("Please select valid " + odDto.ApplicationType + " start & end duration");
                    // Message = " Please select valid " + model.ApplicationType + " start & end duration";
                }
                //validate Leave Durations
                string continuityMessage = string.Empty;
                continuityMessage = ValidateDurationContinuityForMultipleDaysRequest("On Duty", odDto.ODStartDate, odDto.LeaveStartDurationId,
                   odDto.ODEndDate, odDto.LeaveEndDurationId);
                if (!string.IsNullOrEmpty(continuityMessage))
                {
                    throw new ApplicationException(continuityMessage);
                }
                // BL.ValidateBeforeSave(model.StaffId, model.ODStartDate, model.ODEndDate, model.Duration);
                
                //BL.ValidateApplicationOverlaping(odDto.StaffId, odDto.ODStartDate, odDto.LeaveStartDurationId, odDto.ODEndDate, odDto.LeaveStartDurationId);
                CR.ValidateApplicationOverlaping(odDto.StaffId, Convert.ToDateTime(odDto.ODStartDate), odDto.LeaveStartDurationId, Convert.ToDateTime(odDto.ODEndDate), odDto.LeaveStartDurationId);

            }
            else
            {
                DateTime fd = Convert.ToDateTime(odDto.ODDate + " " + odDto.ODStartTime);
                DateTime td = Convert.ToDateTime(odDto.ODDate + " " + odDto.ODEndTime);
                //  BL.ValidateBeforeSave(model.StaffId, fd.ToString(), td.ToString(), model.Duration);
                //BL.ValidateApplicationOverlaping(odDto.StaffId, odDto.ODDate, odDto.LeaveStartDurationId, odDto.ODDate, odDto.LeaveEndDurationId);
                CR.ValidateApplicationOverlaping(odDto.StaffId, Convert.ToDateTime(odDto.ODDate), odDto.LeaveStartDurationId, Convert.ToDateTime(odDto.ODDate), odDto.LeaveEndDurationId);
            }
        }

        internal static ClassesToSave GetClassesToSave(OD_Dto odDto, UserClaims uc)
        {
            ClassesToSave CTS = new ClassesToSave();
            string BaseAddress = string.Empty;
            BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
            RAOnDutyApplicationBusinessLogic BL = new RAOnDutyApplicationBusinessLogic();
            CommonBusinessLogic CB = new CommonBusinessLogic();
            //insert into Request Application Table.
            RequestApplication RA = new RequestApplication();
            RA.Id = BL.GetUniqueId();
            RA.StaffId = odDto.StaffId;
            if (odDto.ApplicationType == "OD")
            {
                RA.LeaveTypeId = "LV0012";
            }
            else if (odDto.ApplicationType == "BT")
            {
                RA.LeaveTypeId = "LV0040";
            }
            else if (odDto.ApplicationType == "WFH")
            {
                RA.LeaveTypeId = "LV0042";
            }
            //RA.PermissionType = model.PermissionTypeId;
            RA.ODDuration = odDto.Duration;
            RA.LeaveStartDurationId = odDto.LeaveStartDurationId;
            RA.LeaveEndDurationId = odDto.LeaveEndDurationId;
            if (odDto.Duration == "SINGLEDAY")
            {
                RA.LeaveStartDurationId = odDto.SingleDayLeaveStartDurationId;
                RA.LeaveEndDurationId = odDto.SingleDayLeaveStartDurationId;
            }
            else
            {
                RA.LeaveStartDurationId = odDto.LeaveStartDurationId;
                RA.LeaveEndDurationId = odDto.LeaveEndDurationId;
            }
            if (odDto.Duration == "SINGLEDAY")
            {
                RA.StartDate = Convert.ToDateTime(odDto.ODDate + " " + odDto.ODStartTime);
                RA.EndDate = Convert.ToDateTime(odDto.ODDate + " " + odDto.ODEndTime);
                RA.TotalHours = Convert.ToDateTime(odDto.TotalHours);
            }
            if (odDto.Duration == "MULTIPLEDAY")
            {
                RA.StartDate = Convert.ToDateTime(odDto.ODStartDate);
                RA.EndDate = Convert.ToDateTime(odDto.ODEndDate);
                RA.TotalDays = Convert.ToDecimal(odDto.TotalDays);
            }

            //validate the date for paycycle
            //ValidateApplicationForPayDate(Convert.ToDateTime(RA.StartDate), Convert.ToDateTime(RA.EndDate));

            RA.ContactNumber = odDto.ContactNumber;
            RA.Remarks = odDto.Remarks;
            RA.ReasonId = 0;
            RA.IsCancelled = false;
            RA.IsApproved = false;
            RA.IsRejected = false;
            RA.ApplicationDate = DateTime.Now;
            RA.AppliedBy = uc.StaffId; // loggedstaffid;
            RA.RequestApplicationType = odDto.ApplicationType;
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
            AA.ApprovalOwner = uc.ApprovalOwner; // model.ApproverId;
            AA.ParentType = odDto.ApplicationType;
            AA.ForwardCounter = 1;
            AA.ApplicationDate = RA.ApplicationDate;
            AA.Approval2Owner = uc.ReviewerOwner; //.ReviewerId;
            AA.Approval2On = null;
            AA.Approval2By = null;
            AA.Approval2statusId = 1;

           
            CTS.RA = RA;
            CTS.AA = AA;
            CTS.ESL = null;

            return CTS;
        }

        private static string  ValidateDurationContinuityForMultipleDaysRequest(string ApplicationType, string FromDate,
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


    }
}