using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model
{
    public class OTBulkUpload
    {
        public string StaffId { get; set;}
        public string TransactionDate {get; set;}
        public string ApprovedExtraHours { get; set;}
        public string ConsiderExtraHoursFor {get; set;}
    }
}
