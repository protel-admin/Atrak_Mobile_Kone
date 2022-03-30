using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class UploadControlTable
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Filename { get; set; }

        [Required]
        [MaxLength(5)]
        public string TypeOfData { get; set; }

        [Required]
        public DateTime UploadedOn { get; set; }

        [Required]
        [MaxLength(50)]
        public string UploadedBy { get; set; }

        [Required]
        public bool IsProcessed { get; set; }

        public DateTime? ProcessedOn { get; set; }

        [MaxLength(20)]
        public string ProcessStatus { get; set; }


        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }

       

    }

    public class UploadControlTabledataList
    {
        public string FileName { get; set; }
        public string TypeofData { get; set; }
        public string UploadedBy { get; set;}
        public DateTime Uploadedon { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }

    }
}
