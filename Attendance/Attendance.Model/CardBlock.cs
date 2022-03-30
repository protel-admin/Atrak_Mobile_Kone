using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class CardBlock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string StaffId { get; set; }
        [Required]
        public DateTime? DateFrom { get; set; }
        [Required]
        public DateTime? DateTo { get; set; }
        [Required]
        public int AbsentCount { get; set; }
        [Required]
        public bool IsCardBlocked { get; set; }
        [Required]
        public DateTime? CardBlockedOn { get; set; }
        [Required]
        public bool IsCardOpened { get; set; }
        [Required]
        public DateTime? CardOpenedOn { get; set; }
        [Required]
        public DateTime? CreatedOn { get; set; }
    }
}
