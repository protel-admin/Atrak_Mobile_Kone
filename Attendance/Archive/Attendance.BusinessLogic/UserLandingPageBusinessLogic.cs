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
        //Rajesh
        public TodaysPunchesDashBoard_DAIMLER GetTodaysPunchDashBoardForMobile(string id, DateTime punchDate)
        {
            var repo = new UserLandingPageRepository();
            var lst = repo.GetTodaysPunchDashBoardForMobile(id, punchDate);

            return lst;
        }

        public List<FirstInLastOutNew> GetFirstInLastOutDashBoard(string id, string FromDate , string Todate, bool Default)
        {
            using (UserLandingPageRepository userLandingPageRepository = new UserLandingPageRepository())
            {
                return userLandingPageRepository.GetFirstInLastOutDashBoard(id, FromDate, Todate, Default);
            }
        }

        public List<TodaysPunchesDashBoard_DAIMLER> GetTodaysPunchDashBoard(string id)
        {
            using (UserLandingPageRepository userLandingPageRepository = new UserLandingPageRepository())
            {
                return userLandingPageRepository.GetTodaysPunchDashBoard(id);
            }
        }

        public List<LeaveBalanceList> ShowLeaveBalanceTable(string id)
        {
            using (UserLandingPageRepository userLandingPageRepository = new UserLandingPageRepository())
            {
                return userLandingPageRepository.ShowLeaveBalanceTable(id);
            }
        }

        public List<TeamAttendance> GetTeamAttendanceFor(string managerId,string workedFromDate,string workedToDate)
        {
            using (UserLandingPageRepository userLandingPageRepository = new UserLandingPageRepository())
            {
                return userLandingPageRepository.GetTeamAttendanceFor(managerId,workedFromDate, workedToDate);
            }
        }
    }
}
