using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model
{

    public class CompensatoryOff
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
        public DateTime? COffReqDate { get; set; }
        [Required]
        public DateTime? COffAvailDate { get; set; }
        [MaxLength(200)]
        [Required]
        public string Reason { get; set; }

        public bool IsCancelled { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
    }

    public class COffList
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public DateTime? COffReqDate { get; set; }
        public DateTime? COffAvailDate { get; set; }
        public string Reason { get; set; }
        public string IsCancelled { get; set; }
    }
}
