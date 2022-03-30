using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model
{
    public class CoffReq
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(10)]
        public String Id { get; set; }
        [MaxLength(20)]
        [Required]
        public string Staffid { get; set; }
        [Required]
        public DateTime? CoffReqFrom { get; set; }
        [Required]
        public DateTime? CoffReqTo { get; set; }
        [Required]
        public decimal TotalDays { get; set; }
        [Required]
        [MaxLength(200)]
        public string Reason { get; set; }

        public bool IsCancelled { get; set; }
        public DateTime? ExpiryDate { get; set; }

        [ForeignKey("Staffid")]
        public virtual Staff Staff { get; set; }
    }
}