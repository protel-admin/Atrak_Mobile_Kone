using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class ApplicationEntryBusinessLogic
    {
        public EmpData GetEmpData(string StaffId)
        {
            using (ApplicationEntryRepository applicationEntryRepository = new ApplicationEntryRepository())
            {
                return applicationEntryRepository.GetEmpData(StaffId);
            }
        }
//#############################
//DELETE CODING
// #############################

public string Delete(string id, string Type, string StaffId, string TotalDays,string cancelledBy)
        {

            using (ApplicationEntryRepository applicationEntryRepository = new ApplicationEntryRepository())
        {
                return applicationEntryRepository.Delete(id, Type, StaffId, TotalDays, cancelledBy);                
            }
        }

        //#############################
        //SEARCH CODING
        // #############################

        public List<ApplicationEntryList> Search(string StaffId)
        {
           
            using (ApplicationEntryRepository applicationEntryRepository = new ApplicationEntryRepository())
            {
                return applicationEntryRepository.Search(StaffId);               
            }
        }

        public void SaveApplicationEntry(ApplicationEntryList _AE_, string UserFullName)
        {
            using (ApplicationEntryRepository applicationEntryRepository = new ApplicationEntryRepository())
            {
                applicationEntryRepository.SaveApplicationEntry(_AE_, UserFullName);
            }
        }


        public List<ApplicationEntryList> GetApplicationEntry()
        {
            using (ApplicationEntryRepository applicationEntryRepository = new ApplicationEntryRepository())
            {
                return applicationEntryRepository.GetApplicationEntry();               
            }
        }
        public List<LeaveTypeList> GetAllLeaves()
        {

            using (ApplicationEntryRepository applicationEntryRepository = new ApplicationEntryRepository())
            {
                return applicationEntryRepository.GetAllLeaves();                
            }
        }

        public List<RHHolidayList> GetRHList(string StaffId)
        {

            using (ApplicationEntryRepository applicationEntryRepository = new ApplicationEntryRepository())
            {
                return applicationEntryRepository.GetRHList(StaffId);               
            }
        }

        public List<ApplicationforcancellationList> GetApplication(string staffId, string applicationType, string fromDate, string toDate)
        {
            using (ApplicationEntryRepository applicationEntryRepository = new ApplicationEntryRepository())
            {
                return applicationEntryRepository.GetApplication(staffId, applicationType, fromDate, toDate);
            }
        }

        public List<ApplicationforcancellationList> GetAllApplication(string staffId, string applicationType,string fromDate,string toDate)
        {
            using (ApplicationEntryRepository applicationEntryRepository = new ApplicationEntryRepository())
            {
                return applicationEntryRepository.GetAllApplication(staffId, applicationType, fromDate, toDate);
            }
        }


    }
}