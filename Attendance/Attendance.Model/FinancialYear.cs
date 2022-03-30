using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class FinancialYear
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id{get;set;}
        [MaxLength(50)]
        [Required]
        public string Name{get;set;}
        [Required]
        public DateTime? From{get;set;}
        [Required]
        public DateTime? To{get;set;}
        [Required]
        public bool IsActive{get;set;}
        public DateTime? CreatedOn{get;set;}
        public string CreatedBy{get;set;}
        public DateTime? ModifiedOn{get;set;}
        public string ModifiedBy { get; set; }
    }

    public class FinancialYearList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string IsActive { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
