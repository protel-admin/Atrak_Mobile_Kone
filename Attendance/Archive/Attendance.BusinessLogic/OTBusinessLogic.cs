using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic {
  public  class OTBusinessLogic 
  {
        //public void SaveOtInformation( List<OtInformation> lst , List<ApplicationApprovalList> lst1 )
        //{
        //    var repo = new OTRepository();
        //    repo.SaveInformation( lst,lst1 );
        //}

      public string ValidateApplication(string StaffId, string FromDate, string ToDate)
      {
            using (OTRepository oTRepository = new OTRepository())
            {               
                return oTRepository.ValidateApplication(StaffId, FromDate, ToDate);              
            }
      }

      public List<CostCentreList> GetCostCentre()
      {
            using (OTRepository oTRepository = new OTRepository())
            {
                return oTRepository.GetCostCentre();
            }
      }

      //public List<AttendanceDataView> GetOT(string StaffId, string fromdate, string todate, string CostCentreId)
      //{
      //    var repo = new OTRepository();
      //    var lst = repo.GetOT(StaffId, fromdate, todate, CostCentreId);
      //    return lst;
      //}

      public void SaveOTApplicationEntry(string Staffid,  string FromDate, string ToDate, string Createdby)
      {
            using (OTRepository oTRepository = new OTRepository())
            {
                oTRepository.SaveOTApplicationEntry(Staffid, FromDate, ToDate, Createdby);
            }
      }
      public EmpData GetEmployeeDetails(string StaffId)
      {
            using (OTRepository oTRepository = new OTRepository())
            {
                return oTRepository.GetEmployeeDetails(StaffId);
            }
      }
        public List<AttendanceDataView> GetOTBusinessLogic(string StaffId, string fromdate, string todate, string LogedInUser)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetOTRepository(StaffId, fromdate, todate, LogedInUser);
            }
        }
        public void UpdateAttendanceData(string StaffId, string ShiftInDate, RequestApplication RA, ApplicationApproval AA)
        {
            using (OTRepository oTRepository = new OTRepository())
            {
                oTRepository.UpdateAttendanceData(StaffId, ShiftInDate, RA, AA);
            }
        }
        public void SubmitRequestApplication(ClassesToSave DataToSave)
        {
            var cm = new CommonRepository();
            if (DataToSave.RA.StaffId != DataToSave.RA.AppliedBy)
            {
                //approve the application.
                DataToSave.AA.ApprovedBy = DataToSave.RA.AppliedBy;
                DataToSave.AA.ApprovedOn = DateTime.Now;
                DataToSave.AA.ApprovalOwner = DataToSave.RA.AppliedBy;
            }
            using (OTRepository oTRepository = new OTRepository())
            {
                oTRepository.SubmitRequestApplication(DataToSave);
            }
        }
        public string SaveOT_COFF_DetailsBusinessLogic(List<OTData> lst,string CreatedBy,string IsOTorCoff)
        {
            using (OTRepository oTRepository = new OTRepository())
            {
                return oTRepository.SaveOT_COFF_DetailsRepository(lst, CreatedBy, IsOTorCoff);
            }
        }
    }
}
