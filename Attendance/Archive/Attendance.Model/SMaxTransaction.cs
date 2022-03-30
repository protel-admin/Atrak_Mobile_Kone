using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model {
    public class SMaxTransaction {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Int64 Id { get; set; }
        public DateTime? Tr_Date { get; set; }
        public DateTime? Tr_Time { get; set; }
        public int Tr_TType { get; set; }
        
        [MaxLength( 500 )]
        public string Tr_Message { get; set; }
        
        public int Tr_NodeId { get; set; }
        
        [MaxLength(50)]
        public string Tr_OpName { get; set; }
        [MaxLength( 20 )]
        public string Tr_CardNumber { get; set; }

        public int Tr_TrackCard { get; set; }
        public int Tr_Reason { get; set; }
        public int Tr_LnId { get; set; }
        [MaxLength( 15 )]
        public string Tr_IPAddress { get; set; }
        [MaxLength( 20 )]
        public string Tr_ChId { get; set; }

        public int Tr_Unit { get; set; }

        public Int64 SMAX_Id { get; set; }
    }
}
