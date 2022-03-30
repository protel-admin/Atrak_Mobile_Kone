using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    public class COffAvailingRequest_Dto
    {
      
        public string StaffId { get; set; }
        public DateTime WorkedDate { get; set; }
        public string CoffStartDate { get; set; }
        public string CoffEndDate { get; set; }
        public int LeaveStartDurationId { get; set; }
        public int LeaveEndDurationId { get; set; }
        public string Remarks { get; set; }
        public string TotalDays { get; set; }
        public string ContactNumber { get; set; }
        
        
    }


    public class COffCreditBalanceToAvail_Dto
    {
        ///COffCreditBalanceList
        public string   StaffId    { get; set; }
        public DateTime WorkedDate { get; set; }
        public decimal  Balance    { get; set; }
        public DateTime ExpiryDate { get; set; }
         public string Status { get; set; }

    }
}
//COffCreditBalanceToAvail_Dto
//{
//    "StaffId": "00163",
//    "WorkedDate": "08-Dec-2019"
//    "Balance": 1.00,
//    "ExpiryDate": "02-Dec-2020",
//    "Status": null,
//    }



//{AttendanceManagement.Models.COffReqAvail}

//{

//    "COffFromDate": "21-May-20 "
//    "COffReqDate": "08-Dec-19 "
//    "WorkedDate": "08-Dec-19 "
//    "COffToDate": "21-May-20 "
//    "FromDurationId": 1
//    "Reason": "090"
//    "StaffId": "00163"
//    "ToDurationId": 1
//    "TotalDays": 1.00

//}






