using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class StaffEditRequest
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

       
        [Required]
        [MaxLength(10)]
        public string RequestId { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserId { get; set; }

        public string Staff { get; set; }
        public string StaffOfficial { get; set; }
        public string StaffPersonal { get; set; }
        public string AdditionalFieldValue { get; set; }

        [Required]
        public DateTime Createdon { get; set; }
        [Required]
        public string Createdby { get; set; }



    }
}
