using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Attendance.Model
{
    public class MaintenanceOff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(10)]
        [Required]
        public string Id { get; set; }
        [MaxLength(20)]
        [Required]
        public string StaffId { get; set; }
        [Required]
        public DateTime? DateFrom { get; set; }
        [Required]
        public DateTime? DateTo { get; set; }
        [MaxLength(200)]
        [Required]
        public string Reason { get; set; }

        [MaxLength(20)]
        public string ContactNumber { get; set; }

        public bool IsCancelled { get; set; }

        public DateTime? ApplicationDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsFlexible { get; set; }
        public int MOffYear { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
    }

    public class MaintenanceOffList
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Reason { get; set; }
        public string ContactNumber { get; set; }
        public string Leaves { get; set; }
        public string IsCancelled { get; set; }
        public string IsFlexible { get; set; }
        public string ApprovalStatusName { get; set; }
        public string MOffYear { get; set; }
    }

}
