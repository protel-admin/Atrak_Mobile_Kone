using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class AttendancePolicyBusinessLogic
    {

        public List<MapRuletoRuleGroupList> GetAllUserRulegroup()
        {
            using (AttendancePolicyRepository attendancePolicyRepository = new AttendancePolicyRepository())
            {

                return attendancePolicyRepository.GetAllUserRulegroup();
            }
        }

        public List<CompanyRule> GetCompany()
        {
                using (AttendancePolicyRepository attendancePolicyRepository = new AttendancePolicyRepository())
                {
                    return attendancePolicyRepository.GetCompany();
                }
        }

        public List<RuleGroup1> GetAllRuleGroupTxsValues(Int32 RuleId)
        {
            using (AttendancePolicyRepository attendancePolicyRepository = new AttendancePolicyRepository())
            {
                return attendancePolicyRepository.GetAllRuleGroupTxsValues(RuleId);           
            }
        }

        public List<RuleGroupTxnsList> GetAllRule()
        {
            using (AttendancePolicyRepository attendancePolicyRepository = new AttendancePolicyRepository())
            {
                return attendancePolicyRepository.GetAllRule();
            }
        }

        public List<RuleGroupTxnsList> GetRule(string id)
        {
            using (AttendancePolicyRepository attendancePolicyRepository = new AttendancePolicyRepository())
            {
                return attendancePolicyRepository.GetRule(id);
            }
        }


        public List<MapRuletoRuleGroupList> GetAllRuleGroupTxns(string id, string Staffid)
        {
            using (AttendancePolicyRepository attendancePolicyRepository = new AttendancePolicyRepository())
            {
                return attendancePolicyRepository.GetAllRuleGroupTxns(id, Staffid);
            }
        }

        public string SaveRuleGroupTxnsInfos(RuleGroupTxn sgt)
        {
            using (AttendancePolicyRepository attendancePolicyRepository = new AttendancePolicyRepository())
            {
                attendancePolicyRepository.SaveRuleGroupTxnsInfo(sgt);
                
            }
            return null;
        }

        public List<SelectListItem> GetAllCompany()
        {
            List<CompanyList> companies = new List<CompanyList>();
            using (AttendancePolicyRepository attendancePolicyRepository = new AttendancePolicyRepository())
            {
                companies = attendancePolicyRepository.GetAllCompany();
            }
            var items = new List<SelectListItem>();

            items = companies.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id.ToString(),
                Selected = false
            }).ToList();
            return items;
        }

        public string GetComapnyName(string StaffId)
        {
            using (AttendancePolicyRepository attendancePolicyRepository = new AttendancePolicyRepository())
            {
                return attendancePolicyRepository.GetComapnyName(StaffId);
            }
        }
    }
}
