using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using Attendance.Model;

namespace AtrakMobileApi.Helpers
{
    public static class PermissionHelper
    {
        // public static void ValidatePermission(string StaffId, DateTime PermissionStartDate, DateTime TotalHours)
        public static void ValidatePermission(string StaffId, DateTime PermissionStartDate, string Duration, string TimeFrom, string TimeTo, DateTime TotalHours)        
        {
            RAPermissionApplicationBusinessLogic BL = new RAPermissionApplicationBusinessLogic();

            //over lapping validation

            if (TotalHours.Hour == 0)
                throw new ApplicationException("KINDLY ENTER VALID PERMISSION HOURS");

            var str = BL.ValidateEligibility(StaffId, 
                PermissionStartDate, 
                Duration,
                TimeFrom,
                TimeTo,
                TotalHours);

            if (!str.ToUpper().StartsWith("OK"))
            {
                throw new ApplicationException(str);
            }
        }

        public static ClassesToSave GetClassesToSave(Permission_Dto perDto,
          //RequestApplication requestApplication, 
          UserClaims uc)
        {
            string BaseAddress = string.Empty;
            ClassesToSave CTS = new ClassesToSave();
            CommonBusinessLogic CB = new CommonBusinessLogic();
            //insert into Request Application Table.
            RequestApplication RA = new RequestApplication();
            RAPermissionApplicationBusinessLogic BL = new RAPermissionApplicationBusinessLogic();
            RA.Id = BL.GetUniqueId();
            RA.StaffId = perDto.StaffId;
            RA.PermissionType = perDto.PermissionTypeId;
            RA.LeaveStartDurationId = Convert.ToInt16(1);
            RA.StartDate = Convert.ToDateTime(perDto.PerStartDate.Add(perDto.FromTimeStart.TimeOfDay));
            RA.LeaveEndDurationId = Convert.ToInt16(1);
            RA.EndDate = Convert.ToDateTime(perDto.PerStartDate.Add(perDto.ToTimeEnd.TimeOfDay));

            //validate the date for paycycle
            //ValidateApplicationForPayDate(Convert.ToDateTime(RA.StartDate), Convert.ToDateTime(RA.EndDate));

            RA.TotalHours = Convert.ToDateTime(perDto.TotalHours);
            RA.ContactNumber = perDto.ContactNbr;
            RA.Remarks = perDto.PermissionOffReason;
            RA.ReasonId = 0;
            RA.IsCancelled = false;
            RA.IsApproved = false;
            RA.IsRejected = false;
            RA.ApplicationDate = DateTime.Now;
            RA.AppliedBy = uc.StaffId;
            RA.RequestApplicationType = "PO";
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
            AA.Approval2Owner = uc.ReviewerOwner; //.ReviewerId;
            AA.Approval2statusId = 1;
            AA.Approval2By = null;
            AA.Approval2On = null;
            AA.ParentType = "PO";
            AA.ForwardCounter = 1;
            AA.ApplicationDate = RA.ApplicationDate;

           
            CTS.RA = RA;
            CTS.AA = AA;
            CTS.ESL = null;

            return CTS;
        }
    }
}