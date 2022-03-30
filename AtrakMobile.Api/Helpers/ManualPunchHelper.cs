using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using Attendance.Model;

namespace AtrakMobileApi.Helpers
{
    public class ManualPunchHelper
    {
        static RAManualPunchApplicationBusinessLogic BL = new RAManualPunchApplicationBusinessLogic();
        public static void ValidatePunchInput(ManualPunch_Dto punchDto)
        {

            if (string.IsNullOrEmpty(punchDto.PunchType).Equals(true))
            {
                throw new ApplicationException("Please select the Punch Type");
            }
           
            if (punchDto.PunchType == "In")
            {
                if (punchDto.ManualPunchStartDateTime == null)
                {
                   throw new ApplicationException("Please select the In Punch");
                }
            }
            if (punchDto.PunchType == "Out")
            {
                if (punchDto.ManualPunchEndDateTime == null)
                {
                   
                    throw new ApplicationException("Please select the OutPunch");
                }
            }
            if (punchDto.PunchType == "InOut")
            {
                if (punchDto.ManualPunchStartDateTime == null && punchDto.ManualPunchEndDateTime == null)
                {
                     
                    throw new ApplicationException("Please select InPunch and OutPunch");
                }
            }
           
        }

        public static void ValidateDuplication(ManualPunch_Dto punchDto)
        {
            //RAManualPunchApplicationBusinessLogic BL = new RAManualPunchApplicationBusinessLogic();

            
            if (punchDto.PunchType == "In")
            {
                var StaffId1 = BL.ValidateExistanceManualPunch(punchDto.StaffId, punchDto.MPStartDateTime.ToString("yyyy-MM-dd hh:mm:ss"));
                if (!string.IsNullOrEmpty(StaffId1))
                {
                    throw new Exception("You have already applied ManualPunch for the Existing Date");
                }
            }

            if (punchDto.PunchType == "Out")
            {
                var StaffId1 = BL.ValidateExistanceManualPunch(punchDto.StaffId, punchDto.MPEndDateTime.ToString("yyyy-MM-dd hh:mm:ss"));
                if (!string.IsNullOrEmpty(StaffId1))
                {
                    throw new Exception("You have already applied ManualPunch for the Existing Date");
                }
            }
            if (punchDto.PunchType == "InOut")
            {
                var StaffId1 = BL.ValidateExistanceManualPunch(punchDto.StaffId, punchDto.MPStartDateTime.ToString("yyyy-MM-dd hh:mm:ss"));
                var StaffId2 = BL.ValidateExistanceManualPunch(punchDto.StaffId, punchDto.MPEndDateTime.ToString("yyyy-MM-dd hh:mm:ss"));
                if (!string.IsNullOrEmpty(StaffId1))
                {
                    throw new Exception("You have already applied ManualPunch for the Existing Date");
                }
            }
        }

        public static void ValidateSameDurationWhenSameDate(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                //BL.MustBeSameDurationWhenSameDate(StartDate, EndDate);
            }
            catch(ApplicationException ex)
            {
                throw ex;
            }
                  
        }


        public static ClassesToSave GetClassesToSave(ManualPunch_Dto punchDto, UserClaims uc)
        {
            ClassesToSave CTS = new ClassesToSave();
            CommonBusinessLogic CB = new CommonBusinessLogic();
            var BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
            //insert into Request Application Table.
            RequestApplication RA = new RequestApplication();
            RA.Id = BL.GetUniqueId();
            RA.StaffId = punchDto.StaffId;

            RA.LeaveStartDurationId = Convert.ToInt16(1);
            if (punchDto.ManualPunchStartDateTime != null && punchDto.ManualPunchEndDateTime != null)
            {
                RA.StartDate = punchDto.MPStartDateTime;
                RA.EndDate = punchDto.MPEndDateTime;
            }
            else if (punchDto.ManualPunchStartDateTime != null && punchDto.ManualPunchEndDateTime == null)
            {
                RA.StartDate = punchDto.MPStartDateTime;
                RA.EndDate = punchDto.MPStartDateTime;
            }
            else if (punchDto.ManualPunchEndDateTime != null && punchDto.ManualPunchStartDateTime == null)
            {
                RA.StartDate = punchDto.MPEndDateTime;
                RA.EndDate = punchDto.MPEndDateTime;
            }
            else
            {
                throw new ApplicationException("Both start and end dates cannot be null");
            }
            //--
            RA.PunchType = punchDto.PunchType;
            RA.LeaveEndDurationId = 1;
            //validate the date for paycycle
            //ValidateApplicationForPayDate(Convert.ToDateTime(RA.StartDate), Convert.ToDateTime(RA.EndDate));

            RA.ContactNumber = punchDto.ContactNumber;
            RA.Remarks = punchDto.Remarks;
            RA.ReasonId = 0;
            RA.IsCancelled = false;
            RA.IsApproved = false;
            RA.IsRejected = false;
            RA.ApplicationDate = DateTime.Now;
            RA.AppliedBy = uc.StaffId; // loggedstaffid;
            RA.RequestApplicationType = "MP";
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
            AA.ApprovalOwner = uc.ApprovalOwner; //.ApproverId;
            AA.Approval2Owner = uc.ReviewerOwner; // model.ReviewerId;
            AA.Approval2statusId = 1;
            AA.Approval2By = null;
            AA.Approval2On = null;
            AA.ParentType = "MP";
            AA.ForwardCounter = 1;
            AA.ApplicationDate = RA.ApplicationDate;

          
            CTS.RA = RA;
            CTS.AA = AA;
            CTS.ESL = null;
            return CTS;
        }


    }
}
