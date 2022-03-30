using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class IndividualLeaveCreditDebitBusinessLogic
    {
        public void SaveEmployeeLeaveAccount(IndividualLeaveCreditDebit data, string User)
        {
            using (IndividualLeaveCreditDebitRepository individualLeaveCreditDebitRepository = new IndividualLeaveCreditDebitRepository())
            {

                individualLeaveCreditDebitRepository.SaveEmployeeLeaveAccount(data, User);
            }
        }

        public IndividualLeaveCreditDebit_EmpDetails GetEmployeeDetails(string StaffId)
        {
            using (IndividualLeaveCreditDebitRepository individualLeaveCreditDebitRepository = new IndividualLeaveCreditDebitRepository())
            {
                return individualLeaveCreditDebitRepository.GetEmployeeDetails(StaffId);
            }
        }
        public IndiviualCreditDebit GetEmpData(string StaffId)
        {
            using (IndividualLeaveCreditDebitRepository individualLeaveCreditDebitRepository = new IndividualLeaveCreditDebitRepository())
            {
                return individualLeaveCreditDebitRepository.GetEmpData(StaffId);
            }
        }
        public List<LeaveTypeAndBalance> GetAllLeaveTypeAndBalance(string StaffId)
        {
            using (IndividualLeaveCreditDebitRepository individualLeaveCreditDebitRepository = new IndividualLeaveCreditDebitRepository())
            {
                return individualLeaveCreditDebitRepository.GetAllLeaveTypeAndBalance(StaffId);
            }
        }

        public List<EmployeeImportResultMesss> SaveBulkEmployeeLeaveAccount(List<IndividualLeaveCreditDebit> data, string actionUser, bool OverWrite)
        {
            using (IndividualLeaveCreditDebitRepository individualLeaveCreditDebitRepository = new IndividualLeaveCreditDebitRepository())
            {
                return individualLeaveCreditDebitRepository.SaveBulkEmployeeLeaveAccount(data, actionUser, OverWrite);
            }
        }

        public List<LeaveTypeListForExcel> GetTheLeaveList()
        {
            using (IndividualLeaveCreditDebitRepository individualLeaveCreditDebitRepository = new IndividualLeaveCreditDebitRepository())
            {
                return individualLeaveCreditDebitRepository.GetTheLeaveList();
            }
        }
    }
}
