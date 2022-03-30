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
        LeaveTypeRepository repoObj = new LeaveTypeRepository();
        public void SaveInformation(LeaveTypeSave lst)
        {
            var repo = new LeaveGroupRepository();
            repo.SaveInformation(lst);
        }

        public string GetLeaveGroupDetails(string id)
        {
            var repo = new LeaveGroupRepository();
            var lst = repo.GetLeaveGroupDetails(id);
            var str = ConvertLeaveGroupDetailsToJSon(lst);
            return str;
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
            var repo = new LeaveGroupRepository();
            var lst = repo.GetAccountableLeaves();
            var str = ConvertAccountableLeavesToJSon(lst);
            return str;
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
            var repo = new LeaveGroupRepository();
            var lst = repo.LoadLeaveGroups();
            var str = ConvertLeaveGroupListToJSon(lst);
            return str;
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

        public List<LeaveGroupTxnListModel> GetTransactionList()
        {
            var repo = new LeaveGroupRepository();
            var lst = repo.GetTransactionList();
            return lst;
        }
        public LeaveGroupTxnListModel GetTransaction(int TxnId)
        {
            var repo = new LeaveGroupRepository();
            var lst = repo.GetTransaction(TxnId);
            return lst;
        }
        #region Bulk Leave Credit Debit
        public int CheckValidOrNotStaffIdBusinessLogic(string StaffId)
        {
            return repoObj.CheckValidOrNotStaffIdRepository(StaffId);
        }
        public string CheckValidOrNotLeaveTypeBusinessLogic(string LeaveType, string StaffId, int ValidStaff)
        {
            return repoObj.CheckValidOrNotLeaveTypeRepository(LeaveType, StaffId, ValidStaff);
        }
        public int CheckValidOrNotReasonBusinessLogic(string LeaveCreditDebitReason)
        {
            return repoObj.CheckValidOrNotReasonRepository(LeaveCreditDebitReason);
        }
        public string SaveEmployeeLeaveAccountBusinessLogic(List<BulkLeaveCreditDebitModel> lst)
        {
            return repoObj.SaveEmployeeLeaveAccountRepository(lst);
        }
        public List<string> GetLeaveCreditDebitReasonBusinessLogic()
        {
            return repoObj.GetLeaveCreditDebitReasonRepository();
        }
        public List<string> GetLeaveTypeBusinessLogic()
        {
            return repoObj.GetLeaveTypeRepository();
        }
        #endregion
    }
}
