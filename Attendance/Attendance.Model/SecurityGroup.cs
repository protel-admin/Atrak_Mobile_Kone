using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model
{
    public class SecurityGroup
    {
        [Key]
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

    }

    public class SecurityGroupList
    {
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public bool IsActive { get; set; }

    }



}
