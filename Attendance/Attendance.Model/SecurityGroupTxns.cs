using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Attendance.Model
{
    public class SecurityGroupTxns
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        public int RoleID { get; set; }
        
        public int ScreenID { get; set; }

        public Int16 Options { get; set; }

        [ForeignKey("RoleID")]
        public virtual SecurityGroup SecurityGroup { get; set; }

        [ForeignKey("ScreenID")]
        public virtual Screen Screen { get; set; }
    }

    public class SecurityGroupTxnsList
    {
        public string Id { get; set; }

        [Required]
        public string RoleID { get; set; }
        [Display(Name = "Role Name:")]
        [UIHint("Entry")]
        //public List<SelectListItem> RName { get; set; }  //selection

        public string RoleName { get; set; }

        [Required]
        public string ScreenID { get; set; }
        [Display(Name = "Screen Name:")]
        [UIHint("List")]
        public List<SelectListItem> SName { get; set; }  //selection

        public string ScrName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Options { get; set; }

        public int RId { get; set; }
        public string RName { get; set; }
        public bool IsActive { get; set; }
        public int SId { get; set; }
        public string ScreenName { get; set; }
        public string ScreenOption { get; set; }
        public string Level { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
    }
}
