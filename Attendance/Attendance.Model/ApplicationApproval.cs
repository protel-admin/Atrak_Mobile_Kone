using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Attendance.Model
{
    public class ApplicationApproval
    {
        [Key]
        [MaxLength(20)]
        [Required]
        public string Id{get;set;}
        
        [MaxLength(20)]
        [Required]
        public string ParentId{get;set;}
        
        [Required]
        public int ApprovalStatusId{get;set;}
        
        //[Required]
        //public int ReviewerstatusId { get; set; }
        
        [MaxLength(50)]
        //[Required]
        public string ApprovedBy{get;set;}

        //[Required]
        public DateTime ? ApprovedOn{get;set;}

        [MaxLength(200)]
        //[Required]
        public string Comment{get;set;}

        [MaxLength(50)]
        [Required]
        public string ApprovalOwner { get; set; }


 //[MaxLength(20)]
 //       public string ReviewedBy { get; set; }

 //       //[Required]
 //       public DateTime? ReviewedOn { get; set; }

 //       [MaxLength(10)]
 //       [Required]
 //       public string ReviewerOwner { get; set; }
        [Required]
        [MaxLength(5)]
        public string ParentType { get; set; }

        [Required]
        public int ForwardCounter { get; set; }

        [Required]
        public DateTime? ApplicationDate { get; set; }

        [MaxLength(50)]
        public string Approval2Owner { get; set; }
        [DefaultValue(1)]
        public int Approval2statusId { get; set; }
        public DateTime? Approval2On { get; set; }
        [MaxLength(50)]
        public string Approval2By { get; set; }
        

        [ForeignKey("ApprovalStatusId")]
        public virtual ApprovalStatus ApprovalStatus { get; set; }
    }


    public class ApplicationApprovalList {
        public string Id { get; set; }

        public string ParentId { get; set; }
        public int ApprovalStatusId { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public string Comment { get; set; }
        public string ApprovalOwner { get; set; }
        public string ParentType { get; set; }
        public int ForwardCounter { get; set; }
        public DateTime ApplicationDate { get; set; }


        public string staffid { get; set; }




        public string ReportingManagerEmailId { get; set; }
    }
}
