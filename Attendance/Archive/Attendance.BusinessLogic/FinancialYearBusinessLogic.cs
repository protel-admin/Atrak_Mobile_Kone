﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using Newtonsoft.Json;

namespace Attendance.BusinessLogic
{
    public class FinancialYearBusinessLogic
    {
        //
        public string GetFinancialYears()
        {
            using (FinancialYearRepository financialYearRepository = new FinancialYearRepository())
            {
                return ConvertFinancialYearListToJSon(financialYearRepository.GetFinancialYears());                
            }
        }

        private string ConvertFinancialYearListToJSon(List<FinancialYearList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach(var d in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new FinancialYearList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    From = d.From,
                    To = d.To,
                    IsActive = d.IsActive
                }));
                jsontemp.Append(",");
            }

            jsonstring=jsontemp.ToString();

            if(string.IsNullOrEmpty(jsonstring) == false)
            {
                if(jsonstring.EndsWith(",") == true)
                {
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
            }
            return "[" + jsonstring + "]";
        }

        public void SaveInformation(string id, string name, string startdate, string enddate, string isactive)
        {
            using (FinancialYearRepository financialYearRepository = new FinancialYearRepository())
            {
                financialYearRepository.SaveInformation(id, name, startdate, enddate, isactive);
            }
        }
    }
}
