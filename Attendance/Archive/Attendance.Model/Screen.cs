using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model
{
    public class Screen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ScreenName { get; set; }

        [Required]
        public Int16 ScreenOption { get; set; }

        [Required]
        public Int16 Level { get; set; }

        [Required]
        public Int16 ParentID { get; set; }


    }

    public class ScreenList
    {

        public int Id { get; set; }

        public string ScreenName { get; set; }

        public string ScreenOption { get; set; }

        public string Level { get; set; }

        public string ParentId { get; set; }

    }
}
