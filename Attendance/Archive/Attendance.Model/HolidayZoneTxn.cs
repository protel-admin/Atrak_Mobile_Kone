using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class HolidayZoneTxn {
        [Key]
        [Required]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        public int Id { get; set; }
        
        [Required]
        public int HolidayZoneId { get; set; }
        
        [Required]
        public int HolidayId { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        
        [ForeignKey( "HolidayId" )]
        public virtual Holiday Holiday { get; set; }

        [ForeignKey( "HolidayZoneId" )]
        public virtual HolidayZone HolidayZone { get; set; }

    }

    public class HolidayZoneTxnList {

        public int Id { get; set; }
        public int HolidayZoneId { get; set; }
        public int HolidayId { get; set; }
        public string HolidayName { get; set; }
        public bool Check { get; set; }
        public bool IsActive { get; set; }

    }
}
