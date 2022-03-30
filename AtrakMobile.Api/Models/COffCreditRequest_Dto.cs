using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    public class COffCreditRequest_Dto
    {
        public string StaffId { get; set; }
        public DateTime WorkedDate { get; set; }
        public string Remarks { get; set; }
        public decimal TotalDays { get; set; }
        public string ContactNumber { get; set; }

    }
    public class COffEligibleDate_Dto
    {
        //class CoffReqDates
        public string Staffid { get; set; }
        public string ShiftDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string ActualCoffTime { get; set; }
        public string CoffBalance { get; set; }
    }
}


//{
//"StaffId":"00163",
//"WorkedDate":"2020-02-09"
//"Remarks":"just testing"
//"TotalDays":1
//"ContactNumber":"9600034828"
//}