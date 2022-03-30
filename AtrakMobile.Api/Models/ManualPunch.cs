using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    public class ManualPunch
    {
        public string User_Id { get; set; }

        public string StaffId { get; set; }
        //
        public string ManualPunchStartDateTime { get; set; }
        //
        public string ManualPunchEndDateTime { get; set; }
        //
        public string Remarks { get; set; }
        //
        public string ContactNumber { get; set; }
        //
        public string PunchType { get; set; }
    }
}