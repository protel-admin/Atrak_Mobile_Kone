using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Attendance.Model {
    public class SqlExecute {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Int64 Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        [MaxLength(5000)]
        public string SqlQuery { get; set; }

        [Required]
        public DateTime? ExecuteDateTime { get; set; }

        [Required]
        [MaxLength(10)]
        public string ParentId { get; set; }

        [Required]
        public bool IsCancelled { get; set; }

        [Required]
        public DateTime? CancelledOn{ get; set; }

        [Required]
        [MaxLength ( 20 )]
        public string CancelledBy { get; set; }

        [Required]
        public bool IsExecuted { get; set; }

        [Required]
        public DateTime? ExecutedOn { get; set; }

        [Required]
        [MaxLength ( 20 )]
        public string ExecutedBy { get; set; }

        [Required]
        public bool ExecuteEveryTime { get; set; }

        [Required]
        [MaxLength ( 10 )]
        public string QueryType { get; set; }

        [Required]
        public int SeqId { get; set; }

        [Required]
        public Int64 GroupId { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [Required]
        [MaxLength ( 20 )]
        public string CreatedBy { get; set; }

    }
}
