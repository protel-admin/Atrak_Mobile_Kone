using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Attendance.Model;

namespace AtrakMobileApi.Models
{
    public class ODApplication
    {
        public string User_Id { get; set; }

        public string StaffId { get; set; }
        //
        public string Duration { get; set; }
        //
        public string ODStartDate { get; set; }
        //
        public string ODDate { get; set; }
        //
        public string ODEndDate { get; set; }
        //
        public string ODStartTime { get; set; }
        //
        public string ODEndTime { get; set; }
        //
        public string PermissionEndTime { get; set; }
        //
        public string TotalHours { get; set; }
        //
        public string Remarks { get; set; }
        //
        public string ContactNumber { get; set; }
        //
        public string PermissionTypeId { get; set; }
        //
        public string PermissionType { get; set; }
        //
        public int LeaveStartDurationId { get; set; }
        //
        public int LeaveEndDurationId { get; set; }
        //
        public string TotalDays { get; set; }
    }
}