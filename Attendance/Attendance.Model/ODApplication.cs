using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class ODApplication
    {
        [Key]
        [MaxLength(10)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id{get;set;}

        [MaxLength(20)]
        [Required]
        public string StaffId{get;set;}

        [MaxLength(15)]
        [Required]
        public string ODDuration{get;set;}

        [Required]
        public DateTime? From{get;set;}

        [Required]
        public DateTime? To{get;set;}

        [MaxLength(200)]
        [Required]
        public string ODReason{get;set;}

        public bool IsCancelled { get; set; }

        [Required]
        public DateTime? CreatedOn{get;set;}

        [MaxLength(20)]
        [Required]
        public string CreatedBy{get;set;}

        [Required]
        public DateTime? ModifiedOn{get;set;}

        [MaxLength(20)]
        [Required]
        public string ModifiedBy { get; set; }
    }

    public class ODApplicationList
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string ODDuration { get; set; }
        public string ODDate { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string ODReason { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string StatusId { get; set; }
        public string Status { get; set; }
        public string IsCancelled { get; set; }
    }
}
