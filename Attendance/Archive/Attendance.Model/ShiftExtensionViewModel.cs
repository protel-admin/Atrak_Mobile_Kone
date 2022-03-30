using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManagement.Models
{
    public class ShiftExtensionViewModel
    {

        public int Id { get; set; }

        [MaxLength(30)]
        public string StaffId { get; set; }

        public string StaffName { get; set; }

        public string StaffDepartment { get; set; }

        [Display(Name = "Transaction Date:")]
        public DateTime TxnDate { get; set; }

        [MaxLength(30)]
        [Display(Name = "Type of Shift Extension:")]
        public string ShiftExtensionType { get; set; }

        [MaxLength(30)]
        [Display(Name = "Duration of Hours:")]
        public string DurationOfHoursExtension { get; set; }

        [Display(Name = "Before Shift Hours:")]
        public int NoOfHoursBeforeShift { get; set; }

        [Display(Name = "After Shift Hours:")]
        public int NoOfHoursAfterShift { get; set; }

        [Display(Name = "Select Shift1:")]
        public string Shift1 { get; set; }

        [Display(Name = "Select Shift2:")]
        public string Shift2 { get; set; }

        [Display(Name = "Select Shift3:")]
        public string Shift3 { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public List<SelectListItem> Shifts { get; set; }

    }
}