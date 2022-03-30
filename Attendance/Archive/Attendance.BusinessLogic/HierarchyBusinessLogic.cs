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
            using (HierarchyRepository hierarchyRepository = new HierarchyRepository())
            {
                return ConvertTeamToJSon(hierarchyRepository.GetTeam());
            }           
        }
        public string GetReviewBlankTeam()
        {
            using (HierarchyRepository hierarchyRepository = new HierarchyRepository())
            {
                return ConvertTeamToJSon(hierarchyRepository.GetReviewBlankTeam());
            }
            
        }
        public string GetApproverBlankTeam()
        {
         
            using (HierarchyRepository hierarchyRepository = new HierarchyRepository())
            {
                return ConvertTeamToJSon(hierarchyRepository.GetApproverBlankTeam());
            }
          
        }

        public void SaveInformation(string ReportingManager, string stafflist)
        {
            using (HierarchyRepository hierarchyRepository = new HierarchyRepository())
            {
                hierarchyRepository.SaveInformation(ReportingManager, stafflist);
            }
        }

        public void SaveOTApproverInformation(string ReportingManager, string stafflist)
        {
            using (HierarchyRepository hierarchyRepository = new HierarchyRepository())
            {
                hierarchyRepository.SaveOTApproverInformation(ReportingManager, stafflist);
            }
        }

        public void SaveReviewerInformation(string ReportingManager, string stafflist)
        {
            using (HierarchyRepository hierarchyRepository = new HierarchyRepository())
            {
                hierarchyRepository.SaveReviewerInformation(ReportingManager, stafflist);
            }
        }

        public void SaveReviewerInformationforAP1(string ReportingManager, string stafflist)
        {
            using (HierarchyRepository hierarchyRepository = new HierarchyRepository())
            {
                hierarchyRepository.SaveReviewerInformationforAP1(ReportingManager, stafflist);
            }
        }

        public void SaveOTReviewerInformation(string ReportingManager, string stafflist)
        {
            using (HierarchyRepository hierarchyRepository = new HierarchyRepository())
            {
                hierarchyRepository.SaveOTReviewerInformation(ReportingManager, stafflist);
            }
        }

        public string GetTeam(string id)
        {
           
            using (HierarchyRepository hierarchyRepository = new HierarchyRepository())
            {
                return ConvertTeamToJSon(hierarchyRepository.GetTeam(id));
            }
          
        }

        public string GetOTApproverTeam(string id)
        {
            using (HierarchyRepository hierarchyRepository = new HierarchyRepository())
            {
               return ConvertTeamToJSon(hierarchyRepository.GetOTApproverTeam(id));
            }

        }

        public string GetReviewerTeam(string id)
        {
            using (HierarchyRepository hierarchyRepository = new HierarchyRepository())
            {
               return ConvertTeamToJSon(hierarchyRepository.GetReviewerTeam(id));
            }            
        }

        public string GetOTReviewerTeam(string id)
        {
             using (HierarchyRepository hierarchyRepository = new HierarchyRepository())
            {
               return ConvertTeamToJSon(hierarchyRepository.GetOTReviewerTeam(id));
            }
           
        }

        public string ConvertTeamToJSon(List<ReportingList> lst)
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach(var d in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new ReportingList(){
                    StaffId = d.StaffId,
                    Name = d.Name,
                    ReportingManagerName = d.ReportingManagerName,
                    ApproverLevel = d.ApproverLevel
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
            return "["+jsonstring+"]";
        }
    }
}
