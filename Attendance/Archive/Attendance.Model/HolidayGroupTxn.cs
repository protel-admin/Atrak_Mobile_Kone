using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class HolidayGroupTxn
    {
        //FOREIGN KEYS
        public string HolidayGroupId { get; set; }
        public int HolidayId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
	    public Int64 Id { get; set; }
        
        [Required]
	    public DateTime? HolidayDateFrom { get; set; }

        [Required]
        public DateTime? HolidayDateTo { get; set; }

        public bool IsActive { get; set; }


        [ForeignKey("HolidayGroupId")]
        public virtual HolidayGroup HolidayGroup { get; set; }

        [ForeignKey("HolidayId")]
        public virtual Holiday Holidays { get; set; }
    }

    public class HolidayGroupTxnList
    {
        public Int32 Id { get; set; }
        public string HolidayGroupId { get; set; }
        public int HolidayId { get; set; }
        public DateTime? HolidayDateFrom { get; set; }
        public DateTime? HolidayDateTo { get; set; }
        public bool IsActive { get; set; }
    }
    public class HolidayGroupTxn1
    {

        public string HolidayName { get; set; }
        public string HolidayDateFrom { get; set; }
        public string HolidayDateTo { get; set; }
    }
}