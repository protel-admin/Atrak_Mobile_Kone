using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    public class OD_Dto
    {

        public string ApplicationType { get; set; }
        public int SingleDayLeaveStartDurationId { get; set; }
        public string StaffId { get; set; }
        public string Id { get; set; }
        public string Duration { get; set; }
        public string ODDate { get; set; }
        public int LeaveStartDurationId { get; set; }
        public string ODStartDate { get; set; }
        public string ODStartTime { get; set; }
        public int LeaveEndDurationId { get; set; }
        public string ODEndDate { get; set; }
        public string ODEndTime { get; set; }
        public string Remarks { get; set; }
        public string TotalHours { get; set; }
        public string TotalDays { get; set; }
        public string ContactNumber { get; set; }
     //   public string ApprovalStatus { get; set; }
        
    }
}