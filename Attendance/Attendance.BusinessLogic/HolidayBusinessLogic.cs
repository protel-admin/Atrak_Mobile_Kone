using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using Newtonsoft.Json;

namespace Attendance.BusinessLogic
{
    public class HolidayBusinessLogic
    {
        public List<HolidayGroupList> GetHolidayGroups()
        {
            using (var repo = new HolidayRepository())
            { 
                var lst = repo.GetHolidayGroups();
            return lst;
            }
        }

        public List<HolidayList> GetHolidayCalendar(string holidaygroupid)
        {using (var repo = new HolidayRepository())
            {
                var lst = repo.GetHolidayCalendar(holidaygroupid);
                return lst;
            }
        }

        public string ConvertHolidayCalendarToJSON(List<HolidayList> Hlst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var l in Hlst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new
                {
                    Hid = l.Hid,
                    LeaveTypeId = l.LeaveTypeId,
                    Name = l.Name,
                    HolidayDateFrom = l.HolidayDateFrom,
                    HolidayDateTo = l.HolidayDateTo,
                    IsFixed = l.IsFixed,
                    LeaveYear = l.LeaveYear
                }));

                jsontemp.Append(",");
            }

            jsonstring = jsontemp.ToString();

            if (string.IsNullOrEmpty(jsonstring) == false)
            {
                if (jsonstring.EndsWith(",") == true)
                {
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
            }
            return "[" + jsonstring + "]";
        }

        public string ConvertHolidayGroupsToJSON(List<HolidayGroupList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var l in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new
                {
                    Id = l.Id,
                    Name = l.Name,
                    LeaveYear = l.LeaveYear,
                    IsCurrent = l.IsCurrent,
                    IsActive = l.IsActive,
                    CreatedOn = l.CreatedOn,
                    CreatedBy = l.CreatedBy
                }));
                jsontemp.Append(",");
            }

            jsonstring = jsontemp.ToString();

            if (string.IsNullOrEmpty(jsonstring) == false)
            {
                if (jsonstring.EndsWith(",") == true)
                {
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
            }
            return "[" + jsonstring + "]";
        }


        public HolidayGroupList GetHolidayGroupDetails(string id)
        {
            using (var repo = new HolidayRepository())
            { 
                var hg = repo.GetHolidayGroupDetails(id);
            return hg;
            }
        }

        public string ConvertHolidayGroupDetailsToJSon(HolidayGroupList hgl)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            jsontemp.Clear();
            jsontemp.Append("{" + ConvertHolidayGroupBasicDetailsToJSon(hgl));
            jsontemp.Append("\"HolidayList\":" + ConvertHolidayListToJSon(hgl));
            jsontemp.Append("}");

            jsonstring = jsontemp.ToString();

            return jsonstring;
        }

        public string ConvertHolidayGroupBasicDetailsToJSon(HolidayGroupList hgl)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            jsontemp.Append(JsonConvert.SerializeObject(new
            {
                Id = hgl.Id,
                Name = hgl.Name,
                LeaveYear = hgl.LeaveYear,
                IsCurrent = hgl.IsCurrent,
                IsActive = hgl.IsActive
            }));

            jsonstring = jsontemp.ToString();

            if (string.IsNullOrEmpty(jsonstring) == false)
            {
                jsonstring = jsonstring.Substring(1, jsonstring.Length - 2);
                return jsonstring + ",";
            }
            else
            {
                return string.Empty;
            }
        }

        public string ConvertHolidayListToJSon(HolidayGroupList hgl)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var l in hgl.HolidayGroupTxnList)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new
                {
                    id = l.Hid,
                    LeaveTypeId = l.LeaveTypeId,
                    Name = l.Name,
                    holidayDateFrom = l.HolidayDateFrom,
                    HolidayDateTo = l.HolidayDateTo,
                    LeaveYear = l.LeaveYear,
                    IsFixed = l.IsFixed
                }));

                jsontemp.Append(",");
            }
            jsonstring = jsontemp.ToString();

            if (string.IsNullOrEmpty(jsonstring) == false)
            {
                if (jsonstring.EndsWith(",") == true)
                {
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
            }
            return "[" + jsonstring + "]";
        }

        public void SaveHolidayGroupDetails(HolidayGroupList hgl, string loggedInUser)
        {
            using (var repo = new HolidayRepository())
                repo.SaveHolidayGroupDetails(hgl, loggedInUser);
        }
        //public void SaveHolidayWorkingDetails(HolidayWorking hw, string txnDateforUpdate)
        //{
        //    try
        //    {
        //        var repository = new HolidayRepository();
        //        repository.SaveHolidayWorkingDetails(hw, txnDateforUpdate);
        //    }
        //    catch (Exception err)
        //    {
        //        throw err;
        //    }

        //}

        public List<HolidayListForMaster> GetAllHolidays()
        {
            using (var repo = new HolidayRepository())
            { 
                var lst = repo.GetAllHolidays();
            if (lst.Count > 0)
            {
                return lst;
            }
            else
            {
                return new List<HolidayListForMaster>();
            }
            }
        }
        public List<LeaveTypeListForHoliday> GetLeaveTypesForHolidayMaster()
        {
            using (var repo = new HolidayRepository())
            { 
                var lst = repo.GetLeaveTypesForHolidayMaster();
            if (lst.Count > 0)
            {
                return lst;
            }
            else
            {
                return new List<LeaveTypeListForHoliday>();
            }
            }
        }

        public void SaveHolidayDetails(Holiday hm)
        {
            using (var repo = new HolidayRepository())
            { 
                try
                {

                    repo.SaveHolidayDetails(hm);
                }
                catch (Exception err)
                {
                    throw err;
                }
            }

        }
        //Holiday Working
        public void SaveHolidayWorkingDetails(HolidayWorking hw, string txnDateforUpdate, string UserType)
        {
            try
            {
                using (var repository = new HolidayRepository())
                    repository.SaveHolidayWorkingDetails(hw, txnDateforUpdate, UserType);
            }
            catch (Exception err)
            {
                throw err;
            }

        }
    }
}
