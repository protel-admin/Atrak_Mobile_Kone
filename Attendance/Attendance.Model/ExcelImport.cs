using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class ExcelImport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [MaxLength(255)]
        public string EmpNo { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string DateOfBirth { get; set; }
        [MaxLength(255)]
        public string DateOfJoining { get; set; }
        [MaxLength(255)]
        public string Gender { get; set; }
        [MaxLength(255)]
        public string FatherName { get; set; }
        [MaxLength(255)]
        public string WorkWeekPattern { get; set; }
        [MaxLength(255)]
        public string Company { get; set; }
        [MaxLength(255)]
        public string BussinessArea { get; set; }
        [MaxLength(255)]
        public string Grade { get; set; }
        [MaxLength(255)]
        public string Designation { get; set; }
        [MaxLength(255)]
        public string CostCenter { get; set; }
        [MaxLength(255)]
        public string Department { get; set; }
        [MaxLength(255)]
        public string Team { get; set; }
        [MaxLength(255)]
        public string ImportFileName { get; set; }
        [MaxLength(255)]
        public string ImportLine { get; set; }
        [MaxLength(255)]
        public string DataOrigin { get; set; }

        public DateTime? CreatedOn { get; set; }

        public bool IsProcessed { get; set; }

    }
}
