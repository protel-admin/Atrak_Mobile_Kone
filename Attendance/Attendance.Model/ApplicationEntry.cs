using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class ApplicationEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string StaffId { get; set; }

        [Required]
        [MaxLength(10)]
        public string ApplicationId { get; set; }

        [Required]
        [MaxLength(5)]
        public string Reason { get; set; }

        [Required]
        public DateTime? FromDate { get; set; }

        [Required]
        public DateTime? ToDate { get; set; }

        [Required]
        [MaxLength(200)]
        public string Remarks { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? CancelledOn { get; set; }

        public string CancelledBy { get; set; }

        public string TotalDays { get; set; }

        public bool IsCancelled { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
    }

    public class ApplicationEntryList
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string GradeName { get; set; }
        public string ApplicationId { get; set; }
        public string Reason { get; set; }
        public int? LeaveStartDurationId { get; set; }
        public string FromDate { get; set; }
        public int? LeaveEndDurationId { get; set; }
        public string ToDate { get; set; }
        public string Total { get; set; }
        public string LeaveType { get; set; }
        public string TotalHours { get; set; }
        public string PermissionDate { get; set; }
        public string PermissionType { get; set; }
        public string Remarks { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CancelledOn { get; set; }
        public string CancelledBy { get; set; }
        public string TotalDays { get; set; }
        public string ApplicationEntryId { get; set; }
        public string LeaveTypeId { get; set; }
        public List<ApplicationEntryList> lstAEL { get; set; }
    }

    public class LeaveTypeList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<LeaveTypeList> LeaveTypeList1 { get; set; }
    }
}