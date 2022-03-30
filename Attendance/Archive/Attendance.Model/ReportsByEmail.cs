using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class ReportsByEmail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string ReportDescription { get; set; }
        [Required]
        [MaxLength(50)]
        public string ReportSubject { get; set; }
        [Required]
        [MaxLength(1000)]
        public string ReportPara1 { get; set; }
        [Required]
        [MaxLength(8000)]
        public string ReportPara2 { get; set; }
        [Required]
        [MaxLength(1000)]
        public string ReportPara3 { get; set; }
        [Required]
        [MaxLength(50)]
        public string FunctionName { get; set; }
        [Required]
        [MaxLength(8000)]
        public string Fields { get; set; }
        [Required]
        [MaxLength(1000)]
        public string ParameterList { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        [MaxLength(20)]
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [MaxLength(20)]
        public string ModifiedBy { get; set; }
    
    }
}
