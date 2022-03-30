using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model
{
    public class PermissionOff
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
        public DateTime? TimeFrom { get; set; }
        [Required]
        public DateTime? TimeTo { get; set; }

        public DateTime? TotalHours { get; set; }

        public DateTime? PermissionDate { get; set; }

        [MaxLength(15)]
        public string PermissionType { get; set; }

        [MaxLength(200)]
        [Required]
        public string Reason { get; set; }
        [MaxLength(20)]
        public string ContactNumber { get; set; }

        public bool IsCancelled { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
    }
    public class PermissionOffList
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string PermissionDate { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string Reason { get; set; }
        public string ContactNumber { get; set; }
        public string IsCancelled { get; set; }
        public string PermissionType{get;set;}
        public string TotalHours { get; set; }
    }
}
