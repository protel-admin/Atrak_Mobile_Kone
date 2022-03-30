using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model
{
    public class StaffOfficialInformationForApi
    {
        //public string StaffId { get; set; }
        //public string UserFullName { get; set; }
        //public string UserRole { get; set; }
        //public string ReportingManagerId { get; set; }
        //public string ReportingManagerName { get; set; }
        //public string CompanyId { get; set; }
        //public string BranchId { get; set; }
        //public string DepartmentId { get; set; }
        //public string GradeName { get; set; }
        //public string CompanyName { get; set; }
        //public string BranchName { get; set; }
        //public string DeptName { get; set; }
        //public string OfficialPhone { get; set; }
        //public string UserEmailId { get; set; }
        //public string ReportingManagerEmailId { get; set; }

        public string StaffId { get; set; }
        public string UserFullName { get; set; }
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
        //public int SecurityGroupId { get; set; }
        public int UserRoleId { get; set; }
        //public int SecurityGroupId { get; set; }
        public string UserRole { get; set; }
        public string LeaveGroupId { get; set; }
        public string WeeklyOffId { get; set; }
        public int HolidayGroupId { get; set; }
        public int PolicyId { get; set; }
        public bool Interimhike { get; set; }
        public decimal Tenure { get; set; }
        public string DateOfJoining
        {
            get; set;
        }
        public string ResignationDate { get; set; }
        public string DateOfRelieving { get; set; }
        public bool IsConfirmed { get; set; }
        public string ConfirmationDate { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int ExtensionNo { get; set; }
        public string PFNo { get; set; }
        public string ESINo { get; set; }

        public int WorkingDayPatternId { get; set; }
        public string ReportingManagerId { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReportingManagerEmailId { get; set; }
        public string Reviewer { get; set; }
        public bool Canteen { get; set; }
        public bool Travel { get; set; }
        public int SalaryDay { get; set; }
        public bool IsWorkingDayPatternLocked { get; set; }
        public bool IsLeaveGroupLocked { get; set; }
        public bool IsHolidayGroupLocked { get; set; }
        public bool IsWeeklyOffLocked { get; set; }
        public bool IsPolicyLocked { get; set; }
        public string DomainId { get; set; }
        public string GradeName { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string DeptName { get; set; }
        public string DesignationName { get; set; }
        public string LocationName { get; set; }
        public string  DepartmentName { get; set; }
        public string DivisionName { get; set; }
        public string  PhotoB64String { get; set; }

    }
}
