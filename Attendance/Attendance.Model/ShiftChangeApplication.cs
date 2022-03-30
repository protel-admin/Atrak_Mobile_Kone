using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class ShiftChangeApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(10)]
        [Required]
        public string Id{get;set;}
        [MaxLength(50)]
        [Required]
        public string StaffId{get;set;}

        [Required]
        public DateTime ? FromDate{get;set;}
        [Required]
        public DateTime ? ToDate{get;set;}
        [MaxLength(10)]
        [Required]
        public string NewShiftId{get;set;}

        [MaxLength ( 200 )]
        [Required]
        public string Reason { get; set; }

        public bool IsCancelled { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

    }
}