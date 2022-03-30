using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Attendance.Model;
using Attendance.Repository;
using Newtonsoft.Json;

namespace Attendance.BusinessLogic {
    public class PrefixSuffixBusinessLogic {

        public List<LeaveView> LoadLeaves() {
            using (PrefixSuffixRepository prefixSuffixRepository = new PrefixSuffixRepository())
            {
                return prefixSuffixRepository.LoadLeaves();
            }
        }

        public void SavePrefixSuffix(PrefixSuffixSetting pss)
        {
            using (PrefixSuffixRepository prefixSuffixRepository = new PrefixSuffixRepository())
            {
                prefixSuffixRepository.SavePrefixSuffix(pss);
            }
        }

        public List<PrefixSuffixList> GetAllPrefixSuffix()
        {
            using (PrefixSuffixRepository prefixSuffixRepository = new PrefixSuffixRepository())
            {
                return prefixSuffixRepository.GetAllPrefixSuffix();
            }
        }

        public string ConvertPrefixSuffixToJSON(List<PrefixSuffixList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var l in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new
                {
                    Id = l.Id,
                    LeaveTypeId = l.LeaveTypeId,
                    LeaveName = l.LeaveName,
                    PrefixLeaveTypeId = l.PrefixLeaveTypeId,
                    PrefixLeaveName = l.PrefixLeaveName,
                    SuffixLeaveTypeId = l.SuffixLeaveTypeId,
                    SuffixLeavename = l.SuffixLeavename,
                    IsActive = l.IsActive
                }));
                jsontemp.Append(",");
            }

            jsonstring = jsontemp.ToString();

            if (string.IsNullOrEmpty(jsonstring) == false){
                if (jsonstring.EndsWith(",") == true){
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
            }
            else
            {
                jsonstring = "EMPTY!";
            }

            return jsonstring;
        }
    }
}
