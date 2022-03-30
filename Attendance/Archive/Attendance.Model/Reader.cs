using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class Reader {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [MaxLength(10)]
        public string Id { get; set; }

        public string DoorId{ get; set; }
        
        [Required]
        [MaxLength ( 50 )]
        public string Description { get; set; }
        
        [Required]
        public byte Inout { get; set; }
        
        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime ? CreatedOn { get; set; }

        [Required]
        [MaxLength ( 20 )]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime ? ModifiedOn { get; set; }

        [Required]
        [MaxLength ( 20 )]
        public string MofifiedBy { get; set; }

        [ForeignKey("DoorId")]
        public virtual Door Door { get; set; }
    }
}
