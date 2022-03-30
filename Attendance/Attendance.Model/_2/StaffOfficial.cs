using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class StaffOfficial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(10)]
        public string StaffId { get; set; }

        public string CompanyId { get; set; }
        public string LocationId { get; set; }
        public string BranchId { get; set; }
        public string DepartmentId { get; set; }
        public string DivisionId { get; set; }
        public string DesignationId { get; set; }
        public string GradeId { get; set; }
        public string LeaveGroupId { get; set; }
        public string WeeklyOffId { get; set; }
        public int HolidayGroupId { get; set; }
        public int PolicyId { get; set; }
        public string CategoryId { get; set; }
        public string CostCentreId { get; set; }
        public string VolumeId { get; set; }
        public int SecurityGroupId { get; set; }
        public bool Interimhike { get; set; }
        public decimal Tenure { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public DateTime? ResignationDate { get; set; }

        public DateTime? DateOfRelieving { get; set; }

        public bool IsConfirmed { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        [MaxLength(15)]
        public string Fax { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public int ExtensionNo { get; set; }

        public int WorkingDayPatternId { get; set; }

        [MaxLength(10)]
        public string ReportingManager { get; set; }


        public bool Canteen { get; set; }


        public bool Travel { get; set; }


        public bool IsWorkingDayPatternLocked { get; set; }


        public bool IsLeaveGroupLocked { get; set; }


        public bool IsHolidayGroupLocked { get; set; }


        public bool IsWeeklyOffLocked { get; set; }


        public bool IsPolicyLocked { get; set; }


        public int SalaryDay { get; set; }

        [MaxLength(50)]
        public string PFNo { get; set; }

        [MaxLength(50)]
        public string ESINo { get; set; }

        [MaxLength(50)]
        public string DomainId { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }

        [ForeignKey("WorkingDayPatternId")]
        public virtual WorkingDayPattern WorkingDayPattern { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

        [ForeignKey("SecurityGroupId")]
        public virtual SecurityGroup SecurityGroup { get; set; }


        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [ForeignKey("DivisionId")]
        public virtual Division Division { get; set; }

        [ForeignKey("VolumeId")]
        public virtual Volume Volume { get; set; }

        [ForeignKey("DesignationId")]
        public virtual Designation Designation { get; set; }

        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }

        //[ForeignKey("ReportMg")]
        //public virtual Staff ReportingManager { get; set; }

        [ForeignKey("LeaveGroupId")]
        public virtual LeaveGroup LeaveGroup { get; set; }

        [ForeignKey("WeeklyOffId")]
        public virtual WeeklyOffs WeeklyOffs { get; set; }

        [ForeignKey("HolidayGroupId")]
        public virtual HolidayZone HolidayZone { get; set; }

        [ForeignKey("PolicyId")]
        public virtual RuleGroup RuleGroup { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("CostCentreId")]
        public virtual CostCentre CostCentre { get; set; }

    }

    public class StaffOfficialInformation
    {
        public string StaffId { get; set; }
        public string CompanyId { get; set; }
        public string BranchId { get; set; }
        public string DepartmentId { get; set; }
        public string DivisionId { get; set; }
        public string VolumeId { get; set; }
        public string DesignationId { get; set; }
        public string GradeId { get; set; }

        public string CategoryId { get; set; }
        public string CostCentreId { get; set; }
        public string LocationId { get; set; }
        public int SecurityGroupId { get; set; }

        public string LeaveGroupId { get; set; }
        public string WeeklyOffId { get; set; }
        public int HolidayGroupId { get; set; }
        public int PolicyId { get; set; }
        public bool Interimhike { get; set; }
        public decimal Tenure { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? ResignationDate { get; set; }
        public DateTime? DateOfRelieving { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int ExtensionNo { get; set; }
        public string PFNo { get; set; }
        public string ESINo { get; set; }

        public int WorkingDayPatternId { get; set; }
        public string ReportingManager { get; set; }
        public string ReportingManagerName { get; set; }
        public bool Canteen { get; set; }
        public bool Travel { get; set; }
        public int SalaryDay { get; set; }
        public bool IsWorkingDayPatternLocked { get; set; }
        public bool IsLeaveGroupLocked { get; set; }
        public bool IsHolidayGroupLocked { get; set; }
        public bool IsWeeklyOffLocked { get; set; }
        public bool IsPolicyLocked { get; set; }
        public string DomainId { get; set; }
    }
}