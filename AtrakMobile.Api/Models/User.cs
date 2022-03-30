using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    //public class User
    //{
    //    public int Id { get; set; }
    //    public string EmailID { get; set; }
    //    public string UserName { get; set; }
    //    public string Password { get; set; }
    //    public int RoleId { get; set; }
    //}

    public class UserClaims
    {
        public string StaffId { get; set; }
        public string EmailId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string RoleId { get; set; }
        public string LocationId { get; set; }
        public string ApprovalOwner { get; set; }
        public string ApprovalOwnerEmailId { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReviewerOwner { get; set; }
        
    }

    public class UserProfileDto
    {
        public string StaffId { get; set; }
        public string UserFullName { get; set; }
        public string CompanyId { get; set; }
        public string BranchId { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string VolumeId { get; set; }
        public string VolumeName { get; set; }
        public string DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string GradeId { get; set; }
        public string GradeName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        //public int SecurityGroupId { get; set; }
        public int UserRoleId { get; set; }
        //public int SecurityGroupId { get; set; }
        public string UserRole { get; set; }
        public string LeaveGroupId { get; set; }
        public string WeeklyOffId { get; set; }
        public int HolidayGroupId { get; set; }
        public int PolicyId { get; set; }
      
        public string DateOfJoining
        {
            get; set;
        }
        
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
      
        public string ReportingManagerId { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReportingManagerEmailId { get; set; }

       
        public string DomainId { get; set; }
        public string Photo { get; set; }
    }

}