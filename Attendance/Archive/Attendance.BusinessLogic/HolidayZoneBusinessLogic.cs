﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Attendance.Model;
using Attendance.Repository;
using Newtonsoft.Json;

namespace Attendance.BusinessLogic
{
    public class HolidayZoneBusinessLogic
    {

        public List<HolidayList> GetAllHolidays()
        {
            using (HolidayZoneRepository holidayZoneRepository = new HolidayZoneRepository())
            {
                return holidayZoneRepository.GetAllHolidays();
                
            }
        }

        public void SaveHolidayZoneWiseHolidayList(string id, string name, string isactive, string olddata, string newdata, string loggedInUser, DateTime createdOn, string createdBy)
        {
            var repo = new HolidayZoneRepository();
            if (string.IsNullOrEmpty(olddata) == false)
            {
                //olddata = olddata.Substring(0, olddata.Length - 1);
                var a = olddata.Split(',');
                foreach (var l in a)
                {
                    if (string.IsNullOrEmpty(l) == false)
                    {
                        if (newdata.Contains(l + ","))
                        {
                            olddata = olddata.Replace(l + ",", string.Empty);
                            newdata = newdata.Replace(l + ",", string.Empty);
                        }
                    }
                }
            }
            olddata = olddata.Replace("],[", ",");
            olddata = olddata.Replace("[", string.Empty);
            olddata = olddata.Replace("],", string.Empty);

            newdata = newdata.Replace("],[", ",");
            newdata = newdata.Replace("[", string.Empty);
            newdata = newdata.Replace("],", string.Empty);
            //}
            using (HolidayZoneRepository holidayZoneRepository = new HolidayZoneRepository())
            {
                holidayZoneRepository.SaveHolidayZoneWiseHolidayList(id, name, isactive, olddata, newdata, loggedInUser, createdOn, createdBy);
            }
        }
        public string GetholidayZoneWiseHolidayList(string id, string HolidayGroupid)
        {
            List<HolidayZoneWiseHolidayList> holidayZoneWiseHolidays = new List<HolidayZoneWiseHolidayList>();
            using (HolidayZoneRepository holidayZoneRepository = new HolidayZoneRepository())
            {
                holidayZoneWiseHolidays =  holidayZoneRepository.GetholidayZoneWiseHolidayList(id, HolidayGroupid);
            }
            var str = ConvertHolidayZoneWiseHolidayListToJson(holidayZoneWiseHolidays);
            return "[" + str + "]";
        }

        //public List<HolidayZoneTxnList> GetholidayZoneWiseHolidayList1(string id)
        //{
        //    var repo = new HolidayZoneRepository();
        //    var lst = repo.GetholidayZoneWiseHolidayList(id);
        //    return lst;
        //}

        public string GetHolidayZones()
        {
            List<HolidayZoneList> holidayZones = new List<HolidayZoneList>();
            using (HolidayZoneRepository holidayZoneRepository = new HolidayZoneRepository())
            {
                holidayZones = holidayZoneRepository.GetHolidayZones();
            }
            var str = ConvertHolidayZoneListToJSON(holidayZones);
            return "[" + str + "]";
        }

        public List<HolidayZoneList> GetAllHolidayZones()
        {
            using (HolidayZoneRepository holidayZoneRepository = new HolidayZoneRepository())
            {
                return holidayZoneRepository.GetHolidayZones();
            }
        }

        public string ConvertHolidayZoneWiseHolidayListToJson(List<HolidayZoneWiseHolidayList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var d in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new HolidayZoneWiseHolidayList()
                {
                    HolidayId = d.HolidayId,
                    LeaveYear = d.LeaveYear,
                    LeaveTypeId = d.LeaveTypeId,
                    HolidayName = d.HolidayName,
                    HolidayDateFrom = d.HolidayDateFrom,
                    HolidayDateTo = d.HolidayDateTo,
                    IsChecked = d.IsChecked
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

            return jsonstring;

        }

        public string ConvertHolidayZoneListToJSON(List<HolidayZoneList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var l in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new HolidayZoneList()
                {
                    Id = l.Id,
                    Name = l.Name,
                    IsActive = l.IsActive,
                    CreatedDate = l.CreatedDate,
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

            return jsonstring;
        }

        public string SaveHolidayZones()
        {
            using (HolidayZoneRepository holidayZoneRepository = new HolidayZoneRepository())
            {
                return holidayZoneRepository.SaveHolidayZones();
            }
            
        }
    }
}
