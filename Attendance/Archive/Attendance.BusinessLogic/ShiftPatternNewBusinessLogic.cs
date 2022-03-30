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
    public class ShiftPatternNewBusinessLogic
    {
        public string LoadShiftPattern()
        {
            using (ShiftPatternNewRepository shiftPatternNewRepository = new ShiftPatternNewRepository())
            {

                return ConvertShiftPatterToJSon(shiftPatternNewRepository.LoadShiftPattern());
            }
     
        }

        public string GetShiftList(int id)
        {
            using (ShiftPatternNewRepository shiftPatternNewRepository = new ShiftPatternNewRepository())
            {
                return ConvertShiftListToJSon(shiftPatternNewRepository.GetShiftList(id));
            }      

        }

        public void SaveInformation(ShiftPatternNewList spnl)
        {
            using (ShiftPatternNewRepository shiftPatternNewRepository = new ShiftPatternNewRepository())
            {
                shiftPatternNewRepository.SaveInformation(spnl);
            }
        }

        public string ConvertShiftListToJSon(List<ShiftList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var d in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new ShiftList()
                {
                    ShiftId = d.ShiftId,
                    ShiftShortName = d.ShiftShortName,
                    ShiftName = d.ShiftName,
                    ShiftIn = d.ShiftIn,
                    ShiftOut = d.ShiftOut
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

        public string ConvertShiftPatterToJSon(List<ShiftPatternNewList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var d in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new ShiftPatternNewList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsRotational = d.IsRotational,
                    IsLifeTime = d.IsLifeTime,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    UpdatedUntil = d.UpdatedUntil,
                    DayPattern = d.DayPattern,
                    WOStartDate = d.WOStartDate,
                    WODayOffSet = d.WODayOffSet,
                    WOLastUpdatedDate = d.WOLastUpdatedDate,
                    IsActive = d.IsActive
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
    }
}
