using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class ErrorLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Int64 Id{get;set;}
        [Required]
        [MaxLength(50)]
        public string AppName {get;set;}
        [Required]
        [MaxLength(50)]
        public string ModuleName {get;set;}
        [Required]
        [MaxLength(50)]
        public string FunctionName {get;set;}
        [Required]
        [MaxLength(4000)]
        public string ErrorMessage {get;set;}
        public DateTime? CreatedOn { get; set; }
    }
}
