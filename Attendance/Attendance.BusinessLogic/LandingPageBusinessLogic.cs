using Attendance.Model;
using Attendance.Repository;
using System.Collections.Generic;

namespace Attendance.BusinessLogic
{
    public class LandingPageBusinessLogic {

        //LandingPageRepository repo = new LandingPageRepository();
        public List<DepartmentWiseHeadCount> GetDepartmentWiseHeadCount( string reportingmanagerid )
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetDepartmentWiseHeadCount(reportingmanagerid);
            return lst;
            }
        }
  public List<HolidayGroupTxn1> GetHolidayCalendar(string StaffId)
        {
            using (var vrepo = new LandingPageRepository())
                return vrepo.GetAllHolidays(StaffId);
        }
        public List<ShiftWiseHeadCount> GetShiftWiseHeadCount( string reportingmanagerid ) {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetShiftWiseHeadCount(reportingmanagerid);
            return lst;
            }
        }

        public void ApproveApplication(string ApprovalId, int ApprovalStatusId, string ApproverId)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                landingPageRepository.ApproveApplication(ApprovalId, ApprovalStatusId, ApproverId);
            }
        }


        public List<LeaveRequestList> GetLeaveRequests( string reportingmanagerid )
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetLeaveRequests(reportingmanagerid);
            return lst;
            }
        }

        public List<ManualPunchRequest> GetManualPunchRequests( string reportingmanagerid )
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetManualPunchRequests(reportingmanagerid);
            return lst;
            }
        }

        public List<ShiftChangeRequest> GetShiftChangeRequests( string reportingmanagerid )
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetShiftChangeRequests(reportingmanagerid);
            return lst;
            }
        }

        public List<PermissionRequest> GetPermissionRequests( string reportingmanagerid )
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetPermissionRequests(reportingmanagerid);
            return lst;
            }
        }

        public List<COffRequest> GetCOffRequests( string reportingmanagerid )
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetCOffRequests(reportingmanagerid);
            return lst;
            }
        }
        public List<COffAvailling> GetCOffAvaillingRequests( string reportingmanagerid )
        {
            using (var repo = new LandingPageRepository())
            {
                var lst = repo.GetCOffAvaillingRequests(reportingmanagerid);
            return lst;
            }
        }

        public List<LaterOffRequest> GetLaterOffRequests( string reportingmanagerid )
        {using (var repo = new LandingPageRepository())
            {
                var lst = repo.GetLaterOffRequests(reportingmanagerid);
                return lst;
            }
        }
        public List<MaintenanceOffRequest> GetMaintenanceOffRequests( string reportingmanagerid )
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetMaintenanceOffRequests(reportingmanagerid);
            return lst;
            }
        }

        public List<OTRequest> GetOTRequests(string reportingmanagerid)
        {using (var repo = new LandingPageRepository())
            {
                var lst = repo.GetOTRequests(reportingmanagerid);
                return lst;
            }
        }

        public List<GetPlannedLeave> GetPlannedLeave()
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetPlannedLeave();
            return lst;
            }
        }

        public List<GetHeadCountAlertDetails> GetHeadCountAlertDetails()
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetHeadCountAlertDetails();
            return lst;
            }
        }

        public List<GetBirthdayAlert> GetBirthdayAlertList(string StaffId)
        {
            using (var repo = new LandingPageRepository())
            {
                return repo.GetBirthdayAlertList(StaffId);
            }
        }


        public List<ODRequest> GetOD_OR_BTRequest(string ReportingManagerId,string ApplicationType)
        {

            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetOD_OR_BTRequest(ReportingManagerId, ApplicationType);
            return lst;
            }
        }

        public void SendEmailToStaff(string ApprovalId)
        {
            using (var repo = new CommonRepository())
                repo.SendEmailToStaff(ApprovalId);
        }

        public List<ShiftWiseHeadCount> GetShiftWiseCount(string LoggedInUserId, string ShiftName, string HeadCountType,string company)
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetShiftWiseCount(LoggedInUserId, ShiftName, HeadCountType, company);
            return lst;
            }
        }
        public List<DepartmentWiseHeadCount> GetDepartmentWiseCount(string LoggedInUserId, string DepartmentName, string HeadCountType)
        {
            using (var repo = new LandingPageRepository())
            {
                var lst = repo.GetDepartmentWiseCount(LoggedInUserId, DepartmentName, HeadCountType); 
            return lst;
            }
        }

        public List<CompleteHeadCount> ShowCompleteHeadCount(string id, string TxnDate)
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.ShowCompleteHeadCount(id, TxnDate);
            return lst;
            }
        }

        public List<CompleteHeadCount> ShowLiveHeadCount()
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.ShowLiveHeadCount();
            return lst;
            }
        }

        public List<Remainder> RemainderDetails(string StaffId)
        {
            using (var repo = new LandingPageRepository())
            {
                return repo.RemainderDetails(StaffId);
            }
        }
        public List<RALeaveDonation> GetLeaveDonationRequest(string reportingmanagerid, string ApplicationType)
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetLeaveDonationRequest(reportingmanagerid, ApplicationType);
            return lst;
            }
        }
        public FirstInLastOutDiamlerNew GetMyPunch(string LoggedInUserId, string SelectedDate)
        {
            using (var repo = new LandingPageRepository())
            { 
                var lst = repo.GetMyPunch(LoggedInUserId, SelectedDate);
            return lst;
            }
        }

        public string GetDashBoardSettings(string settingsName, int PolicyId)
        {
            using (var repo = new LandingPageRepository())
            { 
                var value = repo.GetDashBoardSettings(settingsName, PolicyId);
            return value;
            }
        }

        public int GetPolicyId(string StaffId)
        {
            using (var repo = new LandingPageRepository())
            { 
                var value = repo.GetPolicyId(StaffId);
            return value;
            }
        }
        public DocumentData GetDocumentData(string Id)
        {
            using (var repo = new LandingPageRepository())
            { 
                var value = repo.GetDocumentData(Id);
            return value;
            }
        }
        public GetEmpRoleModel GetEmpRole(string Staffid)
        {
            using (var repo = new LandingPageRepository())
            { 
                var value = repo.GetEmpRole(Staffid);
            return value;
            }
        }
        public void ApproveApplication(string ApprovalId, int ApprovalStatusId, string ApproverId, string ParentType, string LocationId)
        {
            using (var repo = new LandingPageRepository())
                repo.ApproveApplication(ApprovalId, ApprovalStatusId, ApproverId, ParentType, LocationId);
        }
        //Rajesh TODO add to latest BL Source
        public List<AllApplicationHistory> GetAllApplicationHistory(string staffId,int numberOfRec,string applnType)
        {
            using (var repo = new LandingPageRepository())
            { 
                var applications = repo.GetAllApplicationHistory(staffId, numberOfRec, applnType);
            return applications;
            }

        }
        //Rajesh Aug 20: added aplnType in the method argument
        public List<AllPendingApprovals> GetAllPendingApplications(string staffId,string aplnType="")
        {
            using (var repo = new LandingPageRepository())
            { 
                var applications = repo.GetAllPendingApplications(staffId, aplnType);
            return applications;
            }
        }
    }
}
