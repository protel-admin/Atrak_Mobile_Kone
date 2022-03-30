using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class AdditionalField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
        public string ScreenName { get; set; }

        [Required]
        public string ColumnName { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Access { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string Createdby { get; set; }
               
        public DateTime? ModifiedOn { get; set; }

        
        public string Modifiedby { get; set; }

    }

    public class AdditionalFieldValue
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Staffid { get; set; }

       
        [Required]
        public int AddfId { get; set; }

        public string ActualValue { get; set; }



        [ForeignKey("AddfId")]
        public virtual AdditionalField AdditionalField { get; set; }


        public DateTime? ModifiedOn { get; set; }


        public string Modifiedby { get; set; }

       

    }
}
