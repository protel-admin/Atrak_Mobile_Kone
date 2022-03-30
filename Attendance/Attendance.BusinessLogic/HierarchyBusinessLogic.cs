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
    public class HierarchyBusinessLogic
    {
        public string GetTeam()
        {
            var repo = new HierarchyRepository();
            var lst = repo.GetTeam();
            var str = ConvertTeamToJSon(lst);
            return str;
        }
        public string GetReviewBlankTeam()
        {
            var repo = new HierarchyRepository();
            var lst = repo.GetReviewBlankTeam();
            var str = ConvertTeamToJSon(lst);
            return str;
        }
        public string GetApproverBlankTeam()
        {
            var repo = new HierarchyRepository();
            var lst = repo.GetApproverBlankTeam();
            var str = ConvertTeamToJSon(lst);
            return str;
        }

        public string SaveInformation(string Approver, string stafflist, string ApprovalLevel)
        {
            var repo = new HierarchyRepository();
            return repo.SaveInformation(Approver, stafflist, ApprovalLevel);
        }

        public void SaveOTApproverInformation(string ReportingManager, string stafflist)
        {
            var repo = new HierarchyRepository();
            repo.SaveOTApproverInformation(ReportingManager, stafflist);
        }

        public void SaveReviewerInformation(string ReportingManager, string stafflist)
        {
            var repo = new HierarchyRepository();
            repo.SaveReviewerInformation(ReportingManager, stafflist);
        }

        public void SaveReviewerInformationforAP1(string Reviewer, string stafflist)
        {
            var repo = new HierarchyRepository();
            repo.SaveReviewerInformationforAP1(Reviewer, stafflist);
        }

        public void SaveOTReviewerInformation(string ReportingManager, string stafflist)
        {
            var repo = new HierarchyRepository();
            repo.SaveOTReviewerInformation(ReportingManager, stafflist);
        }

        public string GetTeam(string id)
        {
            var repo = new HierarchyRepository();
            var lst = repo.GetTeam(id);
            var str = ConvertTeamToJSon(lst);
            return str;
        }

        public string GetOTApproverTeam(string id)
        {
            var repo = new HierarchyRepository();
            var lst = repo.GetOTApproverTeam(id);
            var str = ConvertTeamToJSon(lst);
            return str;
        }

        public string GetReviewerTeam(string id)
        {
            var repo = new HierarchyRepository();
            var lst = repo.GetReviewerTeam(id);
            var str = ConvertTeamToJSon(lst);
            return str;
        }

        public string GetOTReviewerTeam(string id)
        {
            var repo = new HierarchyRepository();
            var lst = repo.GetOTReviewerTeam(id);
            var str = ConvertTeamToJSon(lst);
            return str;
        }

        public string ConvertTeamToJSon(List<ReportingList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var d in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new ReportingList()
                {
                    StaffId = d.StaffId,
                    Name = d.Name,
                    ReportingManagerName = d.ReportingManagerName,
                    ReviewerName = d.ReviewerName,
                    ApproverLevel = d.ApproverLevel
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
