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
    public class VisitorPassBusinessLogic
    {

        //public string save(VisitorPass model)
        //{
        //    var repo = new VisitorPassRepository();
        //    return repo.Save(model);
        //}

        public StaffView GetStaffDetails(string Id, string IdType)
        {
            using (VisitorPassRepository visitorPassRepository = new VisitorPassRepository())
            {
                return visitorPassRepository.GetStaffDetails(Id, IdType);
            }
        }

        public string SaveVisitTransaction(VisitAppointment Model)
        {
            using (VisitorPassRepository visitorPassRepository = new VisitorPassRepository())
            {
                return visitorPassRepository.SaveVisitTransaction(Model);
            }
        }

        //public List<VisitorPassView> GetAppointmentList(string StaffId)
        //{
        //    var repo = new VisitorPassRepository();
        //    var lst = repo.GetAppointmentList(StaffId);
        //    return lst;
        //}

        //public List<VisitorPassView> VisitorDetails(string PhoneNumber)
        //{
        //    var repo = new VisitorPassRepository();
        //    var lst = repo.VisitorDetails(PhoneNumber);
        //    return lst;
        //}

        public void CancelVisitorPass(string Id)
        {
            using (VisitorPassRepository visitorPassRepository = new VisitorPassRepository())
            {
                 visitorPassRepository.CancelVisitorPass(Id);
            }
        }

        public VisitorDetails GetVisitorDetails(string PhoneNumber)
        {
            using (VisitorPassRepository visitorPassRepository = new VisitorPassRepository())
            {
                return visitorPassRepository.GetVisitorDetails(PhoneNumber);
            }
        }

        public List<SelectListItem> GetVisitPurposeList()
        {
            List<VisitPurposeList> visitPurposeLists = new List<VisitPurposeList>();
            using (VisitorPassRepository visitorPassRepository = new VisitorPassRepository())
            {
                visitPurposeLists =  visitorPassRepository.GetVisitPurposeList();
            }
            var items = new List<SelectListItem>();

            foreach(var l in visitPurposeLists)
            {
                items.Add(new SelectListItem()
                {
                    Value = l.VisitPurposeID.ToString(),
                    Text = l.Description
                });
            }
            
            return items;
        }

        public List<SelectListItem> GetVisitorTypeList()
        {
            List<VisitTypeList> visitTypes = new List<VisitTypeList>();
            using (VisitorPassRepository visitorPassRepository = new VisitorPassRepository())
            {
                visitTypes = visitorPassRepository.GetVisitorTypeList();
            }
            var items = new List<SelectListItem>();

            foreach (var l in visitTypes)
            {
                items.Add(new SelectListItem()
                {
                    Value = l.VisitorTypeID.ToString(),
                    Text = l.Description
                });
            }
            return items;
        }

        public List<SelectListItem> GetVisitingAreaList()
        {
            List<VisitingAreaList> visitingAreas = new List<VisitingAreaList>();
            using (VisitorPassRepository visitorPassRepository = new VisitorPassRepository())
            {
                visitingAreas = visitorPassRepository.GetVisitingAreaList();
            }
            var items = new List<SelectListItem>();

            foreach (var l in visitingAreas)
            {
                items.Add(new SelectListItem()
                {
                    Value = l.WaitLocationID.ToString(),
                    Text = l.Description
                });
            }
            return items;
        }

        public List<SelectListItem> GetReportingManagersOfDepartment(string DepartmentId)
        {
            List<DeptWiseReportingManagers> deptWiseReportings = new List<DeptWiseReportingManagers>();
            using (VisitorPassRepository visitorPassRepository = new VisitorPassRepository())
            {
                deptWiseReportings = visitorPassRepository.GetReportingManagersOfDepartment(DepartmentId);
            }
            var items = new List<SelectListItem>();

            foreach (var l in deptWiseReportings)
            {
                items.Add(new SelectListItem()
                {
                    Value = l.ReportingManagerId,
                    Text = l.ReportingManagerName
                });
            }
            return items;
        }

        public List<PermittedMaterialList> GetPermittedMaterialList()
        {
            using (VisitorPassRepository visitorPassRepository = new VisitorPassRepository())
            {
                return visitorPassRepository.GetPermittedMaterialList();
            }
            //var items = new List<SelectListItem>();

            //foreach (var l in lst)
            //{
            //    items.Add(new SelectListItem()
            //    {
            //        Value = l.PermittedMaterialId.ToString(),
            //        Text = l.PermittedMaterialName
            //    });
            //}

           // return lst;
        }
        
        
        
        public CellNo GetCellNo(string staffid)
        {
            using (VisitorPassRepository visitorPassRepository = new VisitorPassRepository())
            {
                return visitorPassRepository.GetCellNo(staffid);
            }
        }

        public void GetVisitorPassDetails(VisitAppointment model, string SlNo)
        {
            using (VisitorPassRepository visitorPassRepository = new VisitorPassRepository())
            {
                visitorPassRepository.GetVisitorPassDetails(model, SlNo);
            }
        }
    }
}
