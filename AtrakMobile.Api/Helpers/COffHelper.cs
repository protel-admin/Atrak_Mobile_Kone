using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using Attendance.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Helpers
{
    public static class COffHelper
    {
        public static void ValidateWorkedDay(decimal TotalDays)
        {
            if (TotalDays == 0)
            {
                throw new ApplicationException("Please Select Valid Worked date");
            }

        }
        public static void ValidateDurationContinuityForMultipleDaysRequest(COffCreditRequest_Dto COffCrReqDto)
        {
            string continuityMessage = string.Empty;
            continuityMessage = ValidateDurationMultipleDaysRequest("Coff", COffCrReqDto.WorkedDate, 0, COffCrReqDto.WorkedDate, 0);
            //model.CoffEndDate, model.LeaveEndDurationId);
            if (!string.IsNullOrEmpty(continuityMessage))
            {
                throw new ApplicationException(continuityMessage);
            }
        }

        public static void ValidateCoffCreditApplication(COffCreditRequest_Dto COffCrReqDto)
        {
            RACoffCreditApplicationBusinessLogic BL = new RACoffCreditApplicationBusinessLogic();
            BL.ValidateCoffCreditApplication(COffCrReqDto.StaffId, COffCrReqDto.WorkedDate.ToString("dd-MMM-yyyy"), COffCrReqDto.WorkedDate.ToString("dd-MMM-yyyy"));
        }

        public static int GetCOffReqPeriod(string StaffId, string LocationId)
        {
            RACoffCreditApplicationBusinessLogic BL = new RACoffCreditApplicationBusinessLogic();
            //<>            return BL.GetCoffReqPeriodBusinessLogic(StaffId);
            return BL.GetCompOffLapsePeriod(LocationId, StaffId);


        }

        static string ValidateDurationMultipleDaysRequest(string ApplicationType, DateTime FromDate, int StartDurationId, DateTime ToDate, int EndDurationId)
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



        public static ClassesToSave GetClassesToSave(COffCreditRequest_Dto COffCrReqDto, UserClaims uc)
        {
            RACoffCreditApplicationBusinessLogic BL = new RACoffCreditApplicationBusinessLogic();
            CommonBusinessLogic CB = new CommonBusinessLogic();
            ClassesToSave CTS = new ClassesToSave();
            string BaseAddress = string.Empty;
            BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();

            //insert into Request Application Table.
            RequestApplication RA = new RequestApplication();
            // Changes Made by aarthi on 28/2/2020 for CompOff Availing
            int CoffPeriodCount = COffHelper.GetCOffReqPeriod(COffCrReqDto.StaffId, uc.LocationId); // BL.GetCoffReqPeriodBusinessLogic(COffCrReqDto.StaffId);
            RA.Id = BL.GetUniqueId();
            RA.StaffId = COffCrReqDto.StaffId;
            RA.LeaveStartDurationId = 0;// COffCrReqDto.LeaveStartDurationId;
            RA.LeaveEndDurationId = 0;// COffCrReqDto.LeaveEndDurationId;
            RA.StartDate = COffCrReqDto.WorkedDate; // Convert.ToDateTime(COffCrReqDto.COffStartDate);
            RA.EndDate = COffCrReqDto.WorkedDate;//Convert.ToDateTime(COffCrReqDto.COffEndDate);

            RA.TotalDays = COffCrReqDto.TotalDays;
            RA.ContactNumber = COffCrReqDto.ContactNumber;
            RA.Remarks = COffCrReqDto.Remarks;
            RA.LeaveTypeId = "LV0005";
            RA.ReasonId = 0;
            RA.IsCancelled = false;
            RA.IsApproved = false;
            RA.IsRejected = false;
            RA.ApplicationDate = DateTime.Now;
            RA.AppliedBy = uc.StaffId;
            RA.RequestApplicationType = "CR";
            RA.IsCancelApprovalRequired = false;
            RA.IsCancelApproved = false;
            RA.IsCancelRejected = false;
            // Changes Made by aarthi on 28/2/2020 for CompOff Availing
            RA.WorkedDate = COffCrReqDto.WorkedDate; // Convert.ToDateTime(model.CoffStartDate);
            RA.ExpiryDate = COffCrReqDto.WorkedDate.AddDays(Convert.ToInt32(CoffPeriodCount));// Convert.ToDateTime(model.CoffStartDate).AddDays(Convert.ToInt32(CoffPeriodCount));

            // Insert Into Application Approval Table.
            ApplicationApproval AA = new ApplicationApproval();
            AA.Id = BL.GetUniqueId();
            AA.ParentId = RA.Id;
            AA.ApprovalStatusId = 1;
            AA.Approval2statusId = 1;
            AA.ApprovedBy = null;
            AA.ApprovedOn = null;
            AA.Approval2By = null;
            AA.Approval2On = null;
            AA.Comment = null;
            AA.ApprovalOwner = uc.ApprovalOwner; // model.ApproverId;
            AA.Approval2Owner = uc.ReviewerOwner; // model.ReviewerId;
            AA.ParentType = "CR";
            AA.ForwardCounter = 1;
            AA.ApplicationDate = RA.ApplicationDate;


            //Changes made by Aarthi on 07/03/2020
            //BL.ValidateCoffCreditApplication(model.StaffId, model.CoffStartDate, model.CoffStartDate, "CR");
            BL.ValidateCoffCreditApplication(COffCrReqDto.StaffId, COffCrReqDto.WorkedDate.ToString("dd-MMM-yyyy"), COffCrReqDto.WorkedDate.ToString("dd-MMM-yyyy"));
            //Rajesh do we need this validation ??
            COffHelper.ValidateDurationContinuityForMultipleDaysRequest(COffCrReqDto);



            CTS.RA = RA;
            CTS.AA = AA;
            CTS.ESL = null;

            return CTS;
        }
    }
}
