using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.BusinessLogic
{
    public class ComPoOffBusinessLogic
    {
        //ComPoOffRepository repo = new ComPoOffRepository();
        public CompoOffModel GetEmpData(string StaffId)
        {
            using (ComPoOffRepository repo = new ComPoOffRepository())
            { 
                var data = repo.GetEmpData(StaffId);
            return data;
            }
        }

        public string SaveStaffDetails(CompoOffModel Model)
        {
            using (ComPoOffRepository repo = new ComPoOffRepository())
            {
                return repo.SaveStaffDetails(Model);
            }
        }

        public void FromDateShouldBeLessThanToDate(DateTime FromDate, DateTime ToDate)
        {
            if (FromDate > ToDate)
            {
                throw new Exception("The start date must be less than end date.");
            }
        }
        public void WorkedDatevalidate(DateTime FromDate)
        {
            if (FromDate < new DateTime())
            {
                throw new Exception("Please Select Proper Worked date.");
            }
        }
    }
}
