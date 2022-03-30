using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    public class MobileSwipeTransactionDto
    {
        public string StaffId {get;set;}
        public string PunchMode {get;set;}
        public DateTime PunchDateTime {get;set;}
        public string Longitude {get;set;}
        public string Lattitude { get; set; }
        public string Radius { get; set; }
        public int punchOption { get; set; }
    }
}