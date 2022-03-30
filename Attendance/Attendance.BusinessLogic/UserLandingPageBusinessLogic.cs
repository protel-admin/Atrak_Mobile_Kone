using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;

namespace Attendance.BusinessLogic
{
    public class UserLandingPageBusinessLogic
    {
        UserLandingPageRepository repo = new UserLandingPageRepository();
        public List<FirstInLastOutNew> GetFirstInLastOutDashBoard(string id, string FromDate, string Todate, bool Default)
        {
            using (var repo = new UserLandingPageRepository())
            {
                var lst = repo.GetFirstInLastOutDashBoard(id, FromDate, Todate, Default);
            return lst;
            }
        }

        //Rajesh
        public TodaysPunchesDashBoardForMobile GetTodaysPunchDashBoardForMobile(string id, DateTime punchDate)
        {
            using (var repo = new UserLandingPageRepository())
            { 
                var lst = repo.GetTodaysPunchDashBoardForMobile(id, punchDate);
            return lst;
            }
        }

        //Changes from
        #region Birthday Reminder
        public int GetSelfBirthDayBusinessLogic(string StaffId)
        {
            using (UserLandingPageRepository repo = new UserLandingPageRepository())
                return repo.GetSelfBirthDayRepository(StaffId);
        }
        #endregion
        public List<TodaysPunchesDashBoardForMobile> GetTodaysPunchDashBoard(string id)
        {
            using (var repo = new UserLandingPageRepository())
            { 
                var lst = repo.GetTodaysPunchDashBoard(id);
            return lst;
            }
        }

        public List<LeaveBalanceList> ShowLeaveBalanceTable(string StaffId)
        {
            using (UserLandingPageRepository userLandingPageRepository = new UserLandingPageRepository())
            {
                return userLandingPageRepository.ShowLeaveBalanceTable(StaffId);
            }
        }

        public List<TeamAttendance> GetTeamAttendanceFor(string managerId, string workedFromDate, string workedToDate)
        {
            using (UserLandingPageRepository userLandingPageRepository = new UserLandingPageRepository())
            {
                return userLandingPageRepository.GetTeamAttendanceFor(managerId, workedFromDate, workedToDate);
            }
        }
    }
}
