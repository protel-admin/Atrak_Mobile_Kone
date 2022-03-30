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
            using (var repo = new IndividualLeaveCreditDebitRepository())
                repo.SaveEmployeeLeaveAccount(data, User);
        }

        public IndividualLeaveCreditDebit_EmpDetails GetEmployeeDetails(string StaffId)
        {
            using (var repo = new IndividualLeaveCreditDebitRepository())
            { 
                var data = repo.GetEmployeeDetails(StaffId);
            return data;
            }
        }

        public IndiviualCreditDebit GetEmpData(string StaffId)
        {
            using (var repo = new IndividualLeaveCreditDebitRepository())
            { 
                var data = repo.GetEmpData(StaffId);
            return data;
            }
        }
        public List<LeaveTypeAndBalance> GetAllLeaveTypeAndBalance(string StaffId, string Gender)
        {
            using (var repo = new IndividualLeaveCreditDebitRepository())
            { 
            var data = repo.GetAllLeaveTypeAndBalance(StaffId, Gender);
            return data;
            }
        }

        public List<EmployeeImportResultMesss> SaveBulkEmployeeLeaveAccount(List<IndividualLeaveCreditDebit> data, string actionUser, bool OverWrite)
        {
            using (var repo = new IndividualLeaveCreditDebitRepository())
            { 
                var Res = repo.SaveBulkEmployeeLeaveAccount(data, actionUser, OverWrite);
            return Res;
            }
        }

        public List<LeaveTypeListForExcel> GetTheLeaveList()
        {
            using (var repo = new IndividualLeaveCreditDebitRepository())
            { 
                var Res = repo.GetTheLeaveList();
            return Res;
            }
        }
        public string ValidateLeaveCreditDebit(string StaffId, string Leavetype, string TotalDays, string TransactionFlag)
        {
            using (IndividualLeaveCreditDebitRepository individualLeaveCreditDebitRepository = new IndividualLeaveCreditDebitRepository())
            {

                var str = individualLeaveCreditDebitRepository.ValidateLeaveCreditDebit(StaffId, Leavetype, TotalDays, TransactionFlag);

                if (!str.ToUpper().StartsWith("OK"))
                {
                    throw new ApplicationException(str);
                }
                return str;
            }
        }
    }
}

