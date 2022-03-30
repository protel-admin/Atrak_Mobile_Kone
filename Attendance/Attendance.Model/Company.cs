using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(10)]
        public string Id { get; set; }

        [MaxLength(10)]
        public string PeopleSoftCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string ShortName { get; set; }

        [Required]
        [MaxLength(50)]                
        public string LegalName { get; set; }

        [MaxLength(50)]
        public string Website { get; set; }

        [MaxLength(20)]
        public string RegisterNo { get; set; }

        [MaxLength(20)]
        public string TNGSNo { get; set; }

        [MaxLength(20)]
        public string CSTNo { get; set; }

        [MaxLength(20)]
        public string TINNo { get; set; }

        [MaxLength(20)]
        public string ServiceTaxNo { get; set; }

        [MaxLength(20)]
        public string PANNo { get; set; }

        [MaxLength(20)]
        public string PFNo { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        
    }

    public class CompanyList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string LegalName { get; set; }
        public string Website { get; set; }
        public string RegisterNo { get; set; }
        public string TNGSNo { get; set; }
        public string CSTNo { get; set; }
        public string TINNo { get; set; }
        public string ServiceTaxNo { get; set; }
        public string PANNo { get; set; }
        public string PFNo { get; set; }
        public string IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}