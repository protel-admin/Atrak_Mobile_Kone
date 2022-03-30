using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
   
    public class MobileSwipeTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string StaffId { get; set; }
        public string PunchMode { get; set; }
        public DateTime PunchDateTime { get; set; }
        public decimal Longitude { get; set; }
        public decimal Lattitude { get; set; }
        public decimal Radius { get; set; }

    }
}
