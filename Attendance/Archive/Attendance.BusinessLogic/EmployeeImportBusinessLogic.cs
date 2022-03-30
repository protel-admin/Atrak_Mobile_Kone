using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class EmployeeImportBusinessLogic
    {
        public List<EmployeeImportResultMesss> ImportStaffDetails(List<EmployeeImportStaff> ei)
        {
            using (EmployeeImportRepository employeeImportRepository = new EmployeeImportRepository())
            {
                return employeeImportRepository.ImportStaffDetails(ei);                 
            }
        }
    }
}
