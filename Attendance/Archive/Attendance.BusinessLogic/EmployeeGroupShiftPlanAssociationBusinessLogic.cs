using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace Attendance.BusinessLogic {
    public class EmployeeGroupShiftPlanAssociationBusinessLogic {
        //

        public string LoadAssociations()
        {

            using (EmployeeGroupShiftPlanAssociationRepository employeeGroupShiftPlanAssociation = new EmployeeGroupShiftPlanAssociationRepository())
            {
               return ConvertEmployeeGroupShiftPlanLinkToJSon(employeeGroupShiftPlanAssociation.LoadAssociations());
            }
       
        }

        private string ConvertEmployeeGroupShiftPlanLinkToJSon(List<AssociateEmployeeGroupShiftPlan> lst )
        {
            var jsontemp = new StringBuilder();
            var jsonString = string.Empty;

            foreach (var d in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new AssociateEmployeeGroupShiftPlan()
                {
                    EmployeeGroupId = d.EmployeeGroupId ,
                    EmployeeGroupName = d.EmployeeGroupName ,
                    Id = d.Id ,
                    ShiftPlanId = d.ShiftPlanId ,
                    ShiftPlan = d.ShiftPlan,
                    IsActive = d.IsActive
                }));
                jsontemp.Append(",");
            }

            jsonString = jsontemp.ToString();
            if (string.IsNullOrEmpty(jsonString) == false)
            {
                if (jsonString.EndsWith(",") == true)
                {
                    jsonString = jsonString.Substring(0, jsonString.Length - 1);
                }
            }
            return "[" + jsonString + "]";
        }

        public List<SelectListItem> LoadShiftPatterns()
        {
            List<ShiftPatternNewList> shiftPatternNews = new List<ShiftPatternNewList>();

            using (EmployeeGroupShiftPlanAssociationRepository employeeGroupShiftPlanAssociation = new EmployeeGroupShiftPlanAssociationRepository())
            {
                shiftPatternNews = employeeGroupShiftPlanAssociation.LoadShiftPatterns();
            }
            var items = new List<SelectListItem>();

            items = shiftPatternNews.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }

        public void SaveInformation( string Id , string EmployeeGroupId , string ShiftPatternId , string IsActive )
        {
            using (EmployeeGroupShiftPlanAssociationRepository employeeGroupShiftPlanAssociation = new EmployeeGroupShiftPlanAssociationRepository())
            {
                employeeGroupShiftPlanAssociation.SaveInformation(Id, EmployeeGroupId, ShiftPatternId, IsActive);
            }
        }
    }
}
