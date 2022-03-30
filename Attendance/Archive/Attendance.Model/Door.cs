using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class Door {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [MaxLength(10)]
        public string Id { get; set; }

        [Required]
        [MaxLength ( 50 )]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime ? CreatedOn { get; set; }

        [Required]
        [MaxLength ( 10 )]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime ? ModifiedOn { get; set; }

        [Required]
        [MaxLength ( 10 )]
        public string MofifiedBy { get; set; }
    }
}
