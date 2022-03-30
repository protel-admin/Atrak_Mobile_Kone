using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class CoffCreditBusinessLogic
    {
        //CoffCreditRepository repo = new CoffCreditRepository();

        public List<CoffReqDates> GetAllOTDates(string Staffid, string FromDate, string ToDate)
        {
            using (CoffCreditRepository repo = new CoffCreditRepository())
            { 
                var Result = repo.GetAllOTDates(Staffid, FromDate, ToDate);
            return Result;
            }
        }
       
        public string VaidateOnBehalfCompOffCredit(string StaffId, string WorkedDate)
        {
            string validationMessage = string.Empty;
            using (CoffCreditRepository repo = new CoffCreditRepository())
            {

                validationMessage = repo.VaidateOnBehalfCompOffCredit(StaffId, WorkedDate);
                return validationMessage;
            }
        }

        public void SaveOnBehalfCompOffCredit(ClassesToSave classesToSave, string loggedInUser, string userRole)
        {
            using (CoffCreditRepository repo = new CoffCreditRepository())
            {
                repo.SaveOnBehalfCompOffCredit(classesToSave, loggedInUser, userRole);
            }
        }
        public int GetGradeRank(string StaffId)
        {
            using (CoffCreditRepository repo = new CoffCreditRepository())
            {
                int gradeRank = repo.GetGradeRank(StaffId);
                return gradeRank;
            }
        }
        public string GetCoffReqPeriodBusinessLogic(string StaffId)
        {
            using (CoffCreditRepository repo = new CoffCreditRepository())
            {
                return repo.GetCoffReqPeriodRepository(StaffId);
            }
        }
    }
}  