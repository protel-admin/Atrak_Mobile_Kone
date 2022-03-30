using System.Linq;
using WebApisTokenAuth.Models;

namespace WebApisTokenAuth.Repository
{
    public class Employees:ApplicationDbContexts
    {
        //Get employees data from the Employees table in your database
        public dynamic GetEmployees()
        {
            return ApplicationDbContext.Employees.AsEnumerable().Select(x => new { x.EmloyeeName, Designation=x.Designation.Name, x.Address,x.Salary }).ToList();

        }

        public Employee FirstEmployee()
        {
            var e = ApplicationDbContext.Employees.AsEnumerable().Select(x => new { x.EmloyeeName, Designation = x.Designation.Name, x.Address, x.Salary }).ToArray();
            Employee firstEmployee = new Employee()
                                    {
                                        Address = e.First().Address,
                                        Designation = null,
                                        EmloyeeName = e.First().EmloyeeName,
                                        Salary = e.First().Salary,
                                        Department = "temp"
                                    };
            
              
        return firstEmployee;
        }
    }
}