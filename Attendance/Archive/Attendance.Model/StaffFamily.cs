using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class StaffFamily
    {
        //FOREIGN KEYS
	    public string StaffId { get; set; }
        public int RelatedAs { get; set; }

        [Key]
        [MaxLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
	    public string Id { get; set; }

        
        [MaxLength(50)]        
        public string Name { get; set; }

        
        public int Age { get; set; }

        [ForeignKey("RelatedAs")]
        public virtual RelationType RelationType{ get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
    }

    public class StaffFamilyInformation
    {
        public string Id { get; set; }
        public string StaffId { get; set; }
        public int RelatedAs { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}