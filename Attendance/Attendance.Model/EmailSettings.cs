using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class EmailSettings {
        [Key]
        [Required]
        [MaxLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string OutgoingServer { get; set; }
        [Required]
        public int OutgoingPort { get; set; }
        [Required]
        [MaxLength( 50 )]
        public string UserName { get; set; }
        [Required]
        [MaxLength( 50 )]
        public string Password { get; set; }
        [Required]
        public bool EnableSSL { get; set; }
        [Required]
        public string SenderEmail { get; set; }
        [Required]
        public DateTime? CreatedOn { get; set; }
        [Required]
        [MaxLength(20)]
        public string CreatedBy { get; set; }
    }
}
