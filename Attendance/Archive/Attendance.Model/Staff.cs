using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Attendance.Model
{
    public class Staff
    {
        //FOREIGN KEYS
        public int StaffStatusId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(20)]
	    public string Id { get; set; }

        [MaxLength(10)]
        public string PeopleSoftCode { get; set; }

        [Required]
        public int SalutationId { get; set; }

        [MaxLength(8)]
	    public string CardCode { get; set; }

        [MaxLength(50)]
        [Required]
	    public string FirstName { get; set; }
        
        [MaxLength(50)]
	    public string MiddleName { get; set; }
        
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(50)]
	    public string ShortName { get; set; }
        
        [MaxLength(1)]
	    public string Gender { get; set; }
	    
	    public DateTime ? CreatedOn { get; set; }
        
        [MaxLength(20)]
	    public string CreatedBy { get; set; }

	    public DateTime ? ModifiedOn { get; set; }
        
        [MaxLength(20)]
        public string ModifiedBy { get; set; }

        public bool IsSentToSMax { get; set; }

        public bool IsHidden { get; set; }

        public bool IsAttached { get; set; }

        public virtual ICollection<StaffFamily> FamilyDetails { get; set; }
        public virtual ICollection<StaffEducation> EducationDetails { get; set; }

        [ForeignKey("StaffStatusId")]
        public virtual StaffStatus StaffStatus { get; set; }

        [ForeignKey("SalutationId")]
        public virtual Salutation Salutation { get; set; }
    }

    public class StaffInformation
    {
        public string PageStaffId { get; set; }
        public string Id { get; set; }
        public int StaffStatusId { get; set; }
        public int SalutationId { get; set; }
        public string CardCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ShortName { get; set; }
        public string Gender { get; set; }
        public bool IsHidden { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public virtual StaffOfficialInformation StaffOfficial { get; set; }
        public virtual StaffPersonalInformation StaffPersonal { get; set; }
        public virtual List<StaffFamilyInformation> FamilyDetails { get; set; }
        public virtual List<StaffEducationInformation> EducationDetails { get; set; }
        public virtual StaffStatus StaffStatus { get; set; }        
    }

    public class StaffList
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
     
        
    }
}