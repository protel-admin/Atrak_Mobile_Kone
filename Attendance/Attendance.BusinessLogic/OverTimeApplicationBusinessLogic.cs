using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.BusinessLogic
{
   public class OverTimeApplicationBusinessLogic
    {
        OverTimeRepository Repo = new OverTimeRepository();
        public List<OTRequestApplicationModel> OverTimeRequestApplication(string StaffId, string AppliedBy)
        {
            var Obj = Repo.OTRequestApplication(StaffId, AppliedBy);
            return Obj;
        }

        public List<OTRequestApplicationModel> GetAllOverTimeList(string StaffId)
        {
            var Obj = Repo.GetAllOverTimeList(StaffId);
            return Obj;
        }
        //public List<OTRequestApplicationModel> GetAllOverTimeList(string StaffId)
        //{
        //    var Obj = Repo.GetAllOverTimeList(StaffId);
        //    return Obj;
        //}
    }
}
