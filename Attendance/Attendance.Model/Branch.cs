using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class Branch
    {
        //FOREIGN KEYS
        public string CompanyID { get; set; }

        [Key]
        [MaxLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public string Id { get; set; }

        [MaxLength(10)]
        public string PeopleSoftCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(5)]
        public string ShortName { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Address { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        
        [MaxLength(50)]
        public string District { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string State { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }
        
        public int PostalCode { get; set; }
        
        [Required]
        [MaxLength(15)]
        public string Phone { get; set; }
        
        [MaxLength(15)]
        public string Fax { get; set; }
        
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        public bool IsHeadOffice { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }
    }

    public class BranchList {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public bool IsHeadOffice { get; set; }
        public bool IsActive { get; set; }
        public string CompanyID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}