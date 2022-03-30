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
        //: IDisposable
    {
        public EmpData GetEmpData(string StaffId)
        {
            using (ApplicationEntryRepository repo = new ApplicationEntryRepository())
            { 
                return repo.GetEmpData(StaffId);
            }

        }
        //#############################
        //DELETE CODING
        // #############################

        public string Delete(string id, string Type, string StaffId, string TotalDays,string cancelledBy)
        {
            using (var repo = new ApplicationEntryRepository())
            { 
                var data1 = repo.Delete(id, Type, StaffId, TotalDays, cancelledBy);
            return data1;
            }
        }



        //#############################
        //SEARCH CODING
        // #############################

        public List<ApplicationEntryList> Search(string StaffId)
        {
            using (var repo = new ApplicationEntryRepository())
            { 
                var data1 = repo.Search(StaffId);
            return data1;
            }
        }

        public void SaveApplicationEntry(ApplicationEntryList _AE_, string UserFullName, string LocationId)
        {
            using (var repo = new ApplicationEntryRepository())
            { 
                repo.SaveApplicationEntry(_AE_, UserFullName, LocationId);
            }
        }


        public List<ApplicationEntryList> GetApplicationEntry()
        {
            using (var repo = new ApplicationEntryRepository())
            {
                var lst = repo.GetApplicationEntry();
                return lst;
            }
         }
        public List<LeaveTypeList> GetAllLeaves()
        {using (var repo = new ApplicationEntryRepository())
            {
                var leaveList = repo.GetAllLeaves();
                return leaveList;
            }
         }

        public List<RHHolidayList> GetRHList(string StaffId)
        {using (var repo = new ApplicationEntryRepository())
            {
                var lst = repo.GetRHList(StaffId);
                return lst;
            }
         }
        public List<StaffList> GetEmpBulk(string StaffId)
        {using (var repo = new ApplicationEntryRepository())
            {
                var data = repo.GetEmpBulk(StaffId);
                return data;
            }
         }
        public void SaveAttendanceStatusChange(List<AttendanceStatusChangeModel> list, string Createdby)
        {using (var repo = new ApplicationEntryRepository())
            {
                repo.SaveAttendanceStatusChange(list, Createdby);
            }
         }
    }
}