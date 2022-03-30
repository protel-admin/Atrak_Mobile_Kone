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
    public class LeaveApplication
    {
        public string User_Id { get; set; }

        public string StaffId { get; set; }
        //
        public string LeaveStartDurationId { get; set; }
        //
        public string LeaveStartDate { get; set; }
        //
        public string LeaveEndDurationId { get; set; }
        //
        public string LeaveEndDate { get; set; }
        //
        public decimal TotalDays { get; set; }
        //
        public string Remarks { get; set; }
        //
        public string ContactNumber { get; set; }
        //
        public string LeaveTypeId { get; set; }
        

    }
}