using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class HolidayFixedDay {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        public int HolidayId { get; set; }

        [Required]
        public DateTime? HolidayDateFrom { get; set; }

        [Required]
        public DateTime? HolidayDateTo { get; set; }

        [ForeignKey ( "HolidayId" )]
        public virtual Holiday Holiday { get; set; }
    }
}
