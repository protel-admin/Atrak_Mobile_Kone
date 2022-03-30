using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using Newtonsoft.Json;

namespace Attendance.BusinessLogic
{
    public class EmployeeListBusinessLogic
    {
        public string LoadListOfEmployees()
        {
            using (EmployeeListRepository employeeListRepository = new EmployeeListRepository())
            {
                return ConvertEmployeeListToJSON(employeeListRepository.LoadListOfEmployees());                
            }
            
        }

        public string LoadAlphabetWiseListOfEmployees(char alphabet)
        {
            using (EmployeeListRepository employeeListRepository = new EmployeeListRepository())
            {
                return ConvertEmployeeListToJSON(employeeListRepository.LoadAlphabetWiseListOfEmployees(alphabet));
            }           
        }

        public string LoadCriteriaWiseListOfEmployees(string criteriastring)
        {
            using (EmployeeListRepository employeeListRepository = new EmployeeListRepository())
            {
                return ConvertEmployeeListToJSON(employeeListRepository.LoadCriteriaWiseListOfEmployees(criteriastring));          
            }
        }

        public string ConvertEmployeeListToJSON(List<EmployeeList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var d in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new EmployeeList()
                {
                    StaffId = d.StaffId,
                    FirstName = d.FirstName,
                    DeptName = d.DeptName,
                    DesignationName = d.DesignationName,
                    GradeName = d.GradeName,
                    StatusName = d.StatusName,
                    WorkingPattern = d.WorkingPattern
                }));
                jsontemp.Append(",");
            }
            jsonstring = jsontemp.ToString();

            if(string.IsNullOrEmpty(jsonstring) == false)
            {
                if (jsonstring.EndsWith(",") == true)
                {
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
            }

            return "[" + jsonstring + "]";
        }
    }
}
