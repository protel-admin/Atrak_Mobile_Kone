using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    public class MobilePunches
    {
            public string user_id { get; set; }
            public decimal location { get; set; }
            public string status { get; set; }
            public string timestamp { get; set; }
            public string address { get; set; }
    }
}