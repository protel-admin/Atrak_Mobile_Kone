using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class ShiftExtensionAndDoubleShift
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        public string StaffId { get; set; }

        [Required]
        public DateTime? TxnDate { get; set; }

        [MaxLength(30)]
        public string ShiftExtensionType { get; set; }

        [MaxLength(30)]
        public string DurationOfHoursExtension { get; set; }

        public int NoOfHoursBeforeShift { get; set; }

        public int NoOfHoursAfterShift { get; set; }
        [MaxLength(50)]
        public string Shift1 { get; set; }
        [MaxLength(50)]
        public string Shift2 { get; set; }
        [MaxLength(50)]
        public string Shift3 { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }

    }
}
