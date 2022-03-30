using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Attendance.Model
{
    public class SwipeData
    {
        //FOREIGN KEY
        public string StaffId{ get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Int64 Id { get; set; }
        
        [Required]
        public DateTime? SwipeDate { get; set; }

        [Required]
        public DateTime? SwipeTime { get; set; }

        [Required]
        public Int32 InOut { get; set; }
        
        [Required]
        public bool IsManualPunch { get; set; }

        [Required]
        public DateTime? CreatedOn { get;set;}

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }

    }
}