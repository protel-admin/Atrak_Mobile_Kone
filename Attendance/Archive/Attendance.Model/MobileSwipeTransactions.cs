using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class MobileSwipeTransactions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Int64 Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string StaffId { get; set; }

        [Required]
        [MaxLength(20)]
        public string SwipeMode { get; set; }

        public DateTime SwipeDateTime { get; set; }

        [MaxLength(20)]
        public string Longitude { get; set; }

        [MaxLength(20)]
        public string Lattitude { get; set; }

        public DateTime? CreatedOn { get; set; }

    }
}
