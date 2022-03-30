using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class DocumentUpload
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [ForeignKey("ParentId")]
        public virtual RequestApplication RequestApplication { get; set; }
        public string ParentId { get; set; }

        public byte[] FileContent { get; set; }

        public bool IsActive { get; set; }

        public string TypeOfDocument { get; set; }
        
    }
}
