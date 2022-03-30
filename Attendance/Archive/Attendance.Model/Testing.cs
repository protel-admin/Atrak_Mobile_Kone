using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class Testing
    {
        [Key]
        [MaxLength(20)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [MaxLength(20)]
        public string StaffId { get; set; }
        
        [MaxLength(2)]
        public string StaffAge { get; set; }
        
        [MaxLength(10)]
        public string PhoneNo { get; set; }
    }
}
