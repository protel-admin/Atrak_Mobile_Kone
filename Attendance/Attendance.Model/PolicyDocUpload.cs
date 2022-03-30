using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model
{
    public class PolicyDocUpload
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        public string PolicyName { get; set; }

        [Required]
        public string FileType { get; set; }

        [Required]
        public byte[] FileExtension { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public string Createdby { get; set; }

        public Boolean isCancelled { get; set; }

        public DateTime CancelledOn { get; set; }

        public string CancelledBy { get; set; }

    }
}
