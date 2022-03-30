using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class Workstation
    {
        [Key]
        [MaxLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(5)]
        public string ShortName { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
    }

    public class WorkstationAllocation
    {
        [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }



        [MaxLength(20)]
        [Required]
        public string Staffid { get; set; }

        [Required]
        [MaxLength(10)]
        public string WorkstationId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
        
        [ForeignKey("WorkstationId")]
        public virtual Workstation WorkStation { get; set; }
                
    }
    public class WorkStationList
    {
        
        public string Id { get; set; }
        
        public string Name { get; set; }

        
        public string ShortName { get; set; }
        
        public bool IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
