using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Model
{
    public class DashboardSwipes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(30)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string StaffId { get; set; }
        public DateTime TransactionTime { get; set; }
        public int TransactionTypeId { get; set; }
        [MaxLength(5)]
        public string TransactionType { get; set; }

        [MaxLength(30)]
        public string IpAddress { get; set; }

        [MaxLength(50)]
        public string Lattitude { get; set; }

        [MaxLength(50)]
        public string Longitude { get; set; }

        [MaxLength(50)]
        public string PunchLocation { get; set; }

    }
}
