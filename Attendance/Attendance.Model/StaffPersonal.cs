using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;


namespace Attendance.Model
{
    public class StaffPersonal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(50)]
        public string StaffId { get; set; }

        public int StaffBloodGroup { get; set; }
        public int StaffMaritalStatus { get; set; }

        [MaxLength(200)]
        public string Addr { get; set; }

        [MaxLength(50)]
        public string Location { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string District { get; set; }

        [MaxLength(50)]
        public string State { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        [MaxLength(10)]
        public string PostalCode { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }


        public DateTime? DateOfBirth { get; set; }
        public DateTime? MarriageDate { get; set; }

        [MaxLength(12)]
        public string AadharNo { get; set; }

        [MaxLength(10)]
        public string PANNo { get; set; }

        [MaxLength(10)]
        public string PassportNo { get; set; }

        [MaxLength(50)]
        public string DrivingLicense { get; set; }

        [MaxLength(50)]
        public string BankName { get; set; }

        [MaxLength(50)]
        public string BankACNo { get; set; }

        [MaxLength(50)]
        public string BankIFSCCode { get; set; }

        [MaxLength(50)]
        public string BankBranch { get; set; }

        [MaxLength(100)]
        public string FatherName { get; set; }

        [MaxLength(100)]
        public string MotherName { get; set; }

        [MaxLength(12)]
        public string FatherAadharNo { get; set; }

        [MaxLength(12)]
        public string MotherAadharNo { get; set; }

        [MaxLength(150)]
        public string EmergencyContactPerson1 { get; set; }

        [MaxLength(150)]
        public string EmergencyContactPerson2 { get; set; }

        [MaxLength(12)]
        public string EmergencyContactNo1 { get; set; }

        [MaxLength(12)]
        public string EmergencyContactNo2 { get; set; }

        [MaxLength(150)]
        public string Qualification { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }

        [ForeignKey("StaffBloodGroup")]
        public virtual BloodGroup BloodGroup { get; set; }

        [ForeignKey("StaffMaritalStatus")]
        public virtual MaritalStatus MaritalStatus { get; set; }
    }

    public class StaffPersonalInformation
    {
        public string Qualification { get; set; }
        public string StaffId { get; set; }
        public int StaffBloodGroup { get; set; }
        public int StaffMaritalStatus { get; set; }
        public string Addr { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? MarriageDate { get; set; }
        public string PANNo { get; set; }
        public string AadharNo { get; set; }
        public string PassportNo { get; set; }
        public string DrivingLicense { get; set; }
        public string BankName { get; set; }
        public string BankACNo { get; set; }
        public string BankIFSCCode { get; set; }
        public string BankBranch { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FatherAadharNo { get; set; }
        public string MotherAadharNo { get; set; }
        public string EmergencyContactNo1 { get; set; }
        public string EmergencyContactNo2 { get; set; }
        public string EmergencyContactPerson1 { get; set; }
        public string EmergencyContactPerson2 { get; set; }

    }
}