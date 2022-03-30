using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class OTApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(40)]
        public string Id { get; set; }

        [Required]
        public string StaffId { get; set; }

        [Required]
        public DateTime? OTDate { get; set; }

        [Required]
        public DateTime? InTime { get; set; }

        [Required]
        public DateTime? OutTime { get; set; }


        [Required]
        [MaxLength(20)]
        public string OTTime { get; set; }
        [Required]
        public DateTime? OTDuration { get; set; }
        [Required]
        [MaxLength(200)]
        public string OTReason { get; set; }

        public bool IsCancelled { get; set; }
        [Required]
        public DateTime? CreatedOn { get; set; }

        [Required]
        [MaxLength(20)]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime? ModifiedOn { get; set; }

        [Required]
        [MaxLength(20)]
        public string ModifiedBy { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
    }

    public class OTEntrylist
    {

        List<string> stafflist { get; set; }
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string CreatedBy { get; set; }


    }

   

    public class OTApplicationList
    {

        public string Id { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string OTDate { get; set; }
        public string OTTime { get; set; }
        public string OTDuration { get; set; }
        public string OTReason { get; set; }
        public string StatusId { get; set; }
        public string Status { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    public class OTApplicationEntryList
    {

        public int Id { get; set; }


        public string StaffId { get; set; }

        public string StaffName { get; set; }


        public string FromDate { get; set; }


        public string ToDate { get; set; }


        public string CreatedOn { get; set; }

    }
    public class OTForm
    {

        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public List<OTList> OTList { get; set; }

    }

    
    public class OTList
    {

        public bool Checked { get; set; }
        public string StaffId { get; set; }
        public string Name { get; set; }
        public string OTDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string OTTime { get; set; }

    }
}
