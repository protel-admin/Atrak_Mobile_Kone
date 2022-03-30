using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Attendance.Model
{
    public class ShiftPatternTxn
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Int64 Id { get; set; }

        //FOREIGN KEY
        public int PatternId { get; set; }

        [Required]
        [MaxLength(10)]
        public string ParentId { get; set; }

        [Required]
        [MaxLength(1)]
        public string ParentType { get; set; }

        [ForeignKey("PatternId")]
        public virtual ShiftPattern ShiftPattern { get; set; }
        
    }
}