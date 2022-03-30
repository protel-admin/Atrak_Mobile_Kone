using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class Settings {
        [Key]
        [DatabaseGenerated ( DatabaseGeneratedOption.Identity )]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Parameter { get; set; }

        [Required]
        [MaxLength ( 100 )]
        public string Description { get; set; }

        [MaxLength ( 50 )]
        public string Value { get; set; }

        [MaxLength ( 50 )]
        public string DefaultValue { get; set; }

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
        public string ModifiedBy { get; set; }

    }
}
