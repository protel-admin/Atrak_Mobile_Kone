using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class ShiftParentTxn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string ShiftId { get; set; }
        [Required]
        [MaxLength(10)]
        public string ParentId { get; set; }

        [Required]
        [MaxLength(10)]
        public string CompanyId { get; set; }

        [Required]
        [MaxLength(10)]
        public string BranchId { get; set; }

        [Required]
        [MaxLength(10)]
        public string DepartmentId { get; set; }

        [Required]
        [MaxLength(10)]
        public string DivisionId { get; set; }

        [Required]
        [MaxLength(10)]
        public string DesignationId { get; set; }

        [Required]
        [MaxLength(10)]
        public string GradeId { get; set; }

        [Required]
        [MaxLength(10)]
        public string CategoryId { get; set; }

        [Required]
        [MaxLength(10)]
        public string CostCentreId { get; set; }

        [Required]
        [MaxLength(10)]
        public string LocationId { get; set; }



        [Required]
        [MaxLength(1)]
        public string ParentType { get; set; }

        public DateTime? CreatedOn { get; set; }
        [MaxLength(20)]
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [MaxLength(20)]
        public string ModifiedBy { get; set; }
    }
}
