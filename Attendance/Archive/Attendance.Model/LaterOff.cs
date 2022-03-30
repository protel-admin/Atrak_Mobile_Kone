using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model
{
    public class LaterOff
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
        public DateTime? LaterOffReqDate { get; set; }
        [Required]
        public DateTime? LaterOffAvailDate { get; set; }
        [MaxLength(200)]
        [Required]
        public string Reason { get; set; }

        public bool IsCancelled { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
    }


    public class LaterOffList
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public DateTime? LaterOffReqDate { get; set; }
        public DateTime? LaterOffAvailDate { get; set; }
        public string Reason { get; set; }
        public string IsCancelled { get; set; }
    }

    public class ValidLaterOffDates
    {
        public string LaterOffReqDate { get; set; }
        public int Validity { get; set; }
    }
}
