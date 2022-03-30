using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class PeopleSoftDump
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [MaxLength(255)]
        public string EmpCode{get;set;}
        [MaxLength(255)]
        public string PSoftEmpId { get; set; }
        [MaxLength(255)]
        public string NamePrefix { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string FatherName { get; set; }
        [MaxLength(255)]
        public string Gender { get; set; }
        [MaxLength(255)]
        public string DateOfBirth { get; set; }
        [MaxLength(255)]
        public string Address1 { get; set; }
        [MaxLength(255)]
        public string Address2 { get; set; }
        [MaxLength(255)]
        public string Address3 { get; set; }
        [MaxLength(255)]
        public string City { get; set; }
        [MaxLength(255)]
        public string Postal { get; set; }
        [MaxLength(255)]
        public string State { get; set; }
        [MaxLength(255)]
        public string Country { get; set; }
        [MaxLength(255)]
        public string EmployeeStatus { get; set; }
        [MaxLength(255)]
        public string JoiningDate { get; set; }
        [MaxLength(255)]
        public string Designation { get; set; }
        [MaxLength(255)]
        public string Grade { get; set; }
        [MaxLength(255)]
        public string DeptId { get; set; }
        [MaxLength(255)]
        public string Department { get; set; }
        [MaxLength(255)]
        public string CompId { get; set; }
        [MaxLength(255)]
        public string Company { get; set; }
        [MaxLength(255)]
        public string Location { get; set; }
        [MaxLength(255)]
        public string LocationDesc { get; set; }
        [MaxLength(255)]
        public string Plant { get; set; }
        [MaxLength(255)]
        public string DomainID { get; set; }
        [MaxLength(255)]
        public string Phone { get; set; }
        [MaxLength(255)]
        public string Mobile { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        [MaxLength(255)]
        public string SupervisorName1 { get; set; }
        [MaxLength(255)]
        public string SupervisorEmail1 { get; set; }
        [MaxLength(255)]
        public string Flag { get; set; }
        [MaxLength(255)]
        public string SupervisorName2 { get; set; }
        [MaxLength(255)]
        public string SupervisorEmail2 { get; set; }
        [MaxLength(255)]
        public string TotalLeave { get; set; }
        [MaxLength(255)]
        public string LeaveTaken { get; set; }
        [MaxLength(255)]
        public string LeaveBalance { get; set; }
        [MaxLength(255)]
        public string NoofWorkingDays { get; set; }
        [MaxLength(255)]
        public string NoOfWorkedDays { get; set; }
        [MaxLength(255)]
        public string Month { get; set; }
        [MaxLength(255)]
        public string SanctionLeave { get; set; }
        [MaxLength(255)]
        public string LOP { get; set; }
        [MaxLength(255)]
        public string LTA { get; set; }
        [MaxLength(255)]
        public string Absent { get; set; }
        [MaxLength(255)]
        public string LTAStatus { get; set; }
        [MaxLength(255)]
        public string Moff { get; set; }
        [MaxLength(255)]
        public string MOffStatus { get; set; }
        [MaxLength(255)]
        public string Flag2 { get; set; }
        [MaxLength(255)]
        public string Dummy2 { get; set; }
        [MaxLength(255)]
        public string Dummy3 { get; set; }
        [MaxLength(255)]
        public string RHStatus { get; set; }
        [MaxLength(255)]
        public string Dummy5 { get; set; }
        [MaxLength(255)]
        public string Dummy6 { get; set; }
        [MaxLength(255)]
        public string Dummy7 { get; set; }

        [MaxLength(255)]
        public string WorkWeekPattern{get;set;}
        [MaxLength(255)]
        public string BusinessArea{get;set;}
        [MaxLength(255)]
        public string CostCentre{get;set;}
        [MaxLength(255)]
        public string Team{get;set;}
        

        public DateTime? CreatedOn { get; set; }

        public bool IsProcessed { get; set; }

        public DateTime? ProcessedOn { get; set; }

        public bool IsSentToSMAX { get; set; }

        public DateTime? SentToSMAXOn { get; set; }

        [MaxLength(50)]
        public string ImportFileName { get; set; }
        
        [MaxLength(5000)]
        public string DataLineFromFile { get; set; }
        [MaxLength(50)]
        public string DataOrigin { get; set; }
    }
}
