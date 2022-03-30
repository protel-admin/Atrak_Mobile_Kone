using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model {
    public class EmailSendLog {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Int64 Id { get; set; }
        [MaxLength(100)]
        public string From { get; set; }
        [Required]
        [MaxLength( 100 )]
        public string To { get; set; }
        [MaxLength( 1000 )]
        public string CC { get; set; }
        [MaxLength( 1000 )]
        public string BCC { get; set; }
        [Required]
        [MaxLength( 200 )]
        public string EmailSubject { get; set; }
        [Required]
        public string EmailBody { get; set; }
        [Required]
        public DateTime ? CreatedOn { get; set; }
        [Required]
        [MaxLength( 50 )]
        public string CreatedBy { get; set; }
        [Required]
        public bool IsSent { get; set; }
        [Required]
        public DateTime? SentOn { get; set; }
        [Required]
        public bool IsError { get; set; }
        [MaxLength(1000)]
        public string ErrorDescription { get; set; }
        [Required]
        public int SentCounter { get; set; }
    }
}
