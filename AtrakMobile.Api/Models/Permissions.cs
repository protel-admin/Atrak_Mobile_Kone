using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    public class Permissions
    {
        public string User_Id { get; set; }

        public string StaffId { get; set; }
        //
        public string PermissionDate { get; set; }
        //
        public string PermissionStartTime { get; set; }
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

    }
}