using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class ReportToBeSentTxn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [MaxLength(20)]
        [Required]
        public string StaffId { get; set; }
        [Required]
        public int ReportId { get; set; }
        public DateTime? LastRunTime { get; set; }
        public DateTime? NextRunTime { get; set; }
        [Required]
        public int OffSet { get; set; }
        [Required]
        [MaxLength(5)]
        public string Duration { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        [MaxLength(20)]
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [MaxLength(20)]
        public string ModifiedBy { get; set; }
        public string QueryString { get; set; }
    }
}
