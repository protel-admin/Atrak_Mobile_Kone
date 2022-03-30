using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class BulkUpdateBusinessLogic
    {
        public List<SelectListItem> GetMasterList(string TableName)
        {
            List<FilterList> filters = new List<FilterList>();

            using (BulkUpdateRepository bulkUpdateRepository = new BulkUpdateRepository())
            {

                filters = bulkUpdateRepository.GetMasterList(TableName);
            }           

            var items = new List<SelectListItem>();

            foreach ( var l in filters)
                items.Add(new SelectListItem { Value = l.Id.ToString(), 
                    Text = l.Name.ToString().ToUpper(), Selected = false });

            return items;

        }


        public List<SelectListItem> GetWorkingPatternList()
        {
            List<FilterList> filters = new List<FilterList>();


            using (BulkUpdateRepository bulkUpdateRepository = new BulkUpdateRepository())
            {

                filters = bulkUpdateRepository.GetWorkingPatternList();
            }

            var items = new List<SelectListItem>();

            foreach (var l in filters)
                items.Add(new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name.ToString().ToUpper(),
                    Selected = false
                });

            return items;

        }

        public void UpdateEmployees(string EmpList, string Criteria)
        {

            if(Criteria.Trim().EndsWith(",")){
                Criteria = Criteria.Substring(0, Criteria.Length - 1);}

            EmpList = "'" + EmpList.Replace(",", "','") + "'";

            using (BulkUpdateRepository bulkUpdateRepository = new BulkUpdateRepository())
            {

                bulkUpdateRepository.UpdateEmployees(EmpList, Criteria);
            }
        }
    }
}
