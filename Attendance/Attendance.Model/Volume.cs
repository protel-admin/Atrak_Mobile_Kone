using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class Volume
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(10)]
        [Required]
        public string Id { get; set; }

        [MaxLength(10)]
        [Required]
        public string PeopleSoftCode { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(5)]
        [Required]
        public string ShortName { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set;}
        public DateTime? MOdifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    //public class VolumeList
    //{
    //    public string Id { get; set; }
    //    public string PeopleSoftCode { get; set; }
    //    public string Name { get; set; }
    //    public string ShortName { get; set; }
    //    public bool IsActive { get; set; }
    //    public DateTime? CreatedOn { get; set; }
    //    public string CreatedBy { get; set; }
    //    public DateTime? MOdifiedOn { get; set; }
    //    public string ModifiedBy { get; set; }
    //}
}
