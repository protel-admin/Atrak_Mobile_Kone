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
    public class LeaveGroupBusinessLogic
    {
        public void SaveInformation(LeaveTypeSave lst)
        {
            using (LeaveGroupRepository leaveGroupRepository = new LeaveGroupRepository())
            {    
                leaveGroupRepository.SaveInformation(lst);
            }
        }

        public string GetLeaveGroupDetails(string id)
        {
            using (LeaveGroupRepository leaveGroupRepository = new LeaveGroupRepository())
            {
                return ConvertLeaveGroupDetailsToJSon(leaveGroupRepository.GetLeaveGroupDetails(id));
            }
           
        }

        public List<LeaveGroupTxnListModel> GetTransactionList()
        {
            using (LeaveGroupRepository leaveGroupRepository = new LeaveGroupRepository())
            {
                return leaveGroupRepository.GetTransactionList();
            }
        }

        public string ConvertLeaveGroupDetailsToJSon(List<LeaveGroupDetailsList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var l in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new LeaveGroupDetailsList()
                {
                    LeaveTypeId = l.LeaveTypeId,
                    LeaveCount = l.LeaveCount
                }));
                jsontemp.Append(",");
            }

            jsonstring = jsontemp.ToString();

            if (string.IsNullOrEmpty(jsonstring) == false)
            {
                if (jsonstring.EndsWith(","))
                {
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
            }

            return "[" + jsonstring + "]";
        }

        public string GetAccountableLeaves()
        {
 
            using (LeaveGroupRepository leaveGroupRepository = new LeaveGroupRepository())
            {
                return ConvertAccountableLeavesToJSon(leaveGroupRepository.GetAccountableLeaves());
            }
        }

        public string ConvertAccountableLeavesToJSon(List<AccountableLeaveList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var l in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new AccountableLeaveList()
                {
                    Id = l.Id,
                    Name = l.Name,
                    ShortName = l.ShortName
                }));
                jsontemp.Append(",");
            }

            jsonstring = jsontemp.ToString();

            if (string.IsNullOrEmpty(jsonstring) == false)
            {
                if (jsonstring.EndsWith(","))
                {
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
            }

            return "[" + jsonstring + "]";
        }

        public string LoadLeaveGroups()
        {
             using (LeaveGroupRepository leaveGroupRepository = new LeaveGroupRepository())
            {
                return ConvertLeaveGroupListToJSon(leaveGroupRepository.LoadLeaveGroups());
            }
        }

        private string ConvertLeaveGroupListToJSon(List<LeaveGroupList> lst)
        {
            StringBuilder jsontemp = new StringBuilder();
            string jsonstring = string.Empty;

            foreach (var l in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new LeaveGroupList()
                {
                    Id = l.Id,
                    Name = l.Name,
                    IsActive = l.IsActive
                }));
                jsontemp.Append(",");
            }

            jsonstring = jsontemp.ToString();

            if (string.IsNullOrEmpty(jsonstring) == false)
            {
                if (jsonstring.EndsWith(","))
                {
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
            }

            return "[" + jsonstring + "]";
        }

        public LeaveGroupTxnListModel GetTransaction(int TxnId)
        {
            using (LeaveGroupRepository leaveGroupRepository = new LeaveGroupRepository())
            {
                return leaveGroupRepository.GetTransaction(TxnId);
            }
        }
    }
}
