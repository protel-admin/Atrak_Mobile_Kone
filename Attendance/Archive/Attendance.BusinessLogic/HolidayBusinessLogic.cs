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
            using (HolidayRepository holidayRepository = new HolidayRepository())
            {
                return holidayRepository.GetHolidayGroups();            
            }
        }

        public List<HolidayList> GetHolidayCalendar(string holidaygroupid)
        {
            using (HolidayRepository holidayRepository = new HolidayRepository())
            {
                return holidayRepository.GetHolidayCalendar(holidaygroupid);
            }
        }

        public string ConvertHolidayCalendarToJSON(List<HolidayList> Hlst )
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var l in Hlst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new
                {
                    Hid = l.Hid,
                    LeaveTypeId = l.LeaveTypeId,
                    Name = l.Name ,
                    HolidayDateFrom = l.HolidayDateFrom,
                    HolidayDateTo = l.HolidayDateTo,
                    IsFixed = l.IsFixed ,
                    LeaveYear= l.LeaveYear
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
            return "["+jsonstring+"]";
        }

        public string ConvertHolidayGroupsToJSON(List<HolidayGroupList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach ( var l in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new
                {
                    Id = l.Id,
                    Name = l.Name,
                    LeaveYear = l.LeaveYear,
                    IsCurrent = l.IsCurrent,
                    IsActive = l.IsActive , 
                    CreatedOn = l.CreatedOn , 
                    CreatedBy = l.CreatedBy
                }));
                jsontemp.Append(",");
            }

            jsonstring = jsontemp.ToString();

            if(string.IsNullOrEmpty(jsonstring) == false)
            {
                if(jsonstring.EndsWith(",") == true)
                {
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
            }
            return "[" + jsonstring + "]";
        }


        public HolidayGroupList GetHolidayGroupDetails(string id)
        {
            using (HolidayRepository holidayRepository = new HolidayRepository())
            {
                return holidayRepository.GetHolidayGroupDetails(id);
            }
        }

        public string ConvertHolidayGroupDetailsToJSon(HolidayGroupList hgl)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            jsontemp.Clear();
            jsontemp.Append("{"+ConvertHolidayGroupBasicDetailsToJSon(hgl));
            jsontemp.Append("\"HolidayList\":"+ConvertHolidayListToJSon(hgl));
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

            if(string.IsNullOrEmpty(jsonstring) == false)
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

            foreach(var l in hgl.HolidayGroupTxnList)
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

            if(string.IsNullOrEmpty(jsonstring) == false)
            {
                if(jsonstring.EndsWith(",") == true)
                {
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
            }
            return "[" + jsonstring + "]";
        }

        public void SaveHolidayGroupDetails(HolidayGroupList hgl, string loggedInUser)
        {
            using (HolidayRepository holidayRepository = new HolidayRepository())
            {
                holidayRepository.SaveHolidayGroupDetails(hgl, loggedInUser);
            }
        }

        public List<HolidayListForMaster> GetAllHolidays()
        {
            List<HolidayListForMaster> holidayListForMasters = new List<HolidayListForMaster>();
            using (HolidayRepository holidayRepository = new HolidayRepository())
            {
                holidayListForMasters =  holidayRepository.GetAllHolidays();
            }
            if (holidayListForMasters.Count > 0)
            {
                return holidayListForMasters;
            }
            else
            {
                return new List<HolidayListForMaster>();
            }
        }
        public List<LeaveTypeListForHoliday> GetLeaveTypesForHolidayMaster()
        {
            List<LeaveTypeListForHoliday> leaveTypeListForHolidays = new List<LeaveTypeListForHoliday>();
            using (HolidayRepository holidayRepository = new HolidayRepository())
            {
                leaveTypeListForHolidays = holidayRepository.GetLeaveTypesForHolidayMaster();
            }
            if (leaveTypeListForHolidays.Count > 0)
            {
                return leaveTypeListForHolidays;
            }
            else
            {
                return new List<LeaveTypeListForHoliday>();
            }
        }

        public void SaveHolidayDetails(Holiday hm)
        {
            try
            {
                using (HolidayRepository holidayRepository = new HolidayRepository())
                {
                    holidayRepository.SaveHolidayDetails(hm);
                }
            }
            catch (Exception err)
            {
                throw err;
            }

        }
    }
}
