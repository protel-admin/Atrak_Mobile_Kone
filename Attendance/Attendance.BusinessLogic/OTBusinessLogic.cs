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
    public class OTBusinessLogic
    {
        OTRepository repo = new OTRepository();

        public string ValidateApplication(string StaffId, string FromDate, string ToDate)
        {
            var repo = new OTRepository();
            var str = repo.ValidateApplication(StaffId, FromDate, ToDate);
            return str;
        }

        public List<CostCentreList> GetCategory()
        {
            var repo = new OTRepository();
            return repo.GetCategory();
        }

        public List<AttendanceDataView> GetOT(string StaffId, string fromdate, string todate, string CategoryId)
        {
            var repo = new OTRepository();
            var lst = repo.GetOT(StaffId, fromdate, todate, CategoryId);
            return lst;
        }

        public void SaveOTApplicationEntry(string Staffid, string FromDate, string ToDate, string Createdby)
        {
            var repo = new OTRepository();
            repo.SaveOTApplicationEntry(Staffid, FromDate, ToDate, Createdby);


        }

        public EmpData GetEmployeeDetails(string StaffId)
        {
            var repo = new OTRepository();
            var data = repo.GetEmployeeDetails(StaffId);
            return data;
        }
        #region OT and Coff Approval (Extra hours Approvals)
        public List<AttendanceDataView> GetOTBusinessLogic(string StaffId, string fromdate, string todate)
        {
            ReportRepository repo = new ReportRepository();
            return repo.GetOTRepository(StaffId, fromdate, todate);
        }
        public string SaveOT_COFF_DetailsBusinessLogic(List<OTData> lst, string CreatedBy, string IsOTorCoff)
        {
            return repo.SaveOT_COFF_DetailsRepository(lst, CreatedBy, IsOTorCoff);
        }
        #endregion

    }
}
