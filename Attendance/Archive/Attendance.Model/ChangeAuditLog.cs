using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class ChangeAuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserId { get; set; }

        [Required]
        public string ChangeLog { get; set; }

        [Required]
        [MaxLength(6)]
        public string ActionType { get; set; }

        [Required]
        [MaxLength(100)]
        public string TableName { get; set; }

        [Required]
        [MaxLength(20)]
        public string PrimaryKeyValue { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }
    }
}
