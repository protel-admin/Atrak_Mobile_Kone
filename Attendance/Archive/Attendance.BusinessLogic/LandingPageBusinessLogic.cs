using Attendance.Model;
using Attendance.Repository;
using System.Collections.Generic;

namespace Attendance.BusinessLogic {
    public class LandingPageBusinessLogic {
        //Rajesh  To BE ADDED IN THE ORIGINAL SOURCE MAY 16 2020

        public List<HolidayGroupTxn1> GetHolidayCalendar(string StaffId)
        {
            var vrepo = new LandingPageRepository();
            return vrepo.GetAllHolidays(StaffId);
        }

        public List<DepartmentWiseHeadCount> GetDepartmentWiseHeadCount(string reportingmanagerid)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                return landingPageRepository.GetDepartmentWiseHeadCount(reportingmanagerid);

        	}
		}

        public List<ShiftWiseHeadCount> GetShiftWiseHeadCount(string reportingmanagerid)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
        {
                return landingPageRepository.GetShiftWiseHeadCount(reportingmanagerid);
            }
        }

        public List<LeaveRequestList> GetLeaveRequests(string reportingmanagerid)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                return landingPageRepository.GetLeaveRequests(reportingmanagerid);
            }
        }

           public List<ApplicationApprovalpendingCountModel> GetApplicationApprovalpendingCounts(string ReportingManagerId)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                return landingPageRepository.GetApplicationApprovalpendingCounts(ReportingManagerId);
            }
        }

       public List<ManualPunchRequest> GetManualPunchRequests(string reportingmanagerid)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
        {
                return landingPageRepository.GetManualPunchRequests(reportingmanagerid);
            }
        }

        public List<ShiftChangeRequest> GetShiftChangeRequests(string reportingmanagerid)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
        {
                return landingPageRepository.GetShiftChangeRequests(reportingmanagerid);
            }
        }

        public List<PermissionRequest> GetPermissionRequests(string reportingmanagerid)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
        {
                return landingPageRepository.GetPermissionRequests(reportingmanagerid);
            }
        }

        public List<COffRequest> GetCOffRequests(string reportingmanagerid)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
        {
                return landingPageRepository.GetCOffRequests(reportingmanagerid);
            }
        }

        public List<LaterOffRequest> GetLaterOffRequests(string reportingmanagerid)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
        {
                return landingPageRepository.GetLaterOffRequests(reportingmanagerid);
            }
        }

        public void ApproveApplication(string ApprovalId, int ApprovalStatusId, string ApproverId)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                landingPageRepository.ApproveApplication(ApprovalId, ApprovalStatusId, ApproverId);
            }
        }

        public List<MaintenanceOffRequest> GetMaintenanceOffRequests(string reportingmanagerid)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
        {
                return landingPageRepository.GetMaintenanceOffRequests(reportingmanagerid);
            }
        }

        public List<OTRequest> GetOTRequests(string reportingmanagerid)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                return landingPageRepository.GetOTRequests(reportingmanagerid);
            }
        }

        public List<ODRequest> GetOD_OR_BTRequest(string ReportingManagerId, string ApplicationType)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
        {
                return landingPageRepository.GetOD_OR_BTRequest(ReportingManagerId, ApplicationType);
            }
        }

        public void SendEmailToStaff(string ApprovalId)
        {
            using (CommonRepository commonRepository = new CommonRepository())
            {
                commonRepository.SendEmailToStaff(ApprovalId);
            }
        }

        public List<ShiftWiseHeadCount> GetShiftWiseCount(string LoggedInUserId, string ShiftName, string HeadCountType, string company)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
        {
                return landingPageRepository.GetShiftWiseCount(LoggedInUserId, ShiftName, HeadCountType, company);
            }
        }
        public List<DepartmentWiseHeadCount> GetDepartmentWiseCount(string LoggedInUserId, string DepartmentName, string HeadCountType)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                return landingPageRepository.GetDepartmentWiseCount(LoggedInUserId, DepartmentName, HeadCountType);
            }
        }

        public List<CompleteHeadCount> ShowCompleteHeadCount(string id, string TxnDate)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                return landingPageRepository.ShowCompleteHeadCount(id, TxnDate);
            }
        }

        public FirstInLastOutDiamlerNew GetMyPunch(string LoggedInUserId, string SelectedDate)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                return landingPageRepository.GetMyPunch(LoggedInUserId, SelectedDate);
            }
        }

        public string GetDashBoardSettings(string settingsName, int PolicyId)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                return landingPageRepository.GetDashBoardSettings(settingsName, PolicyId);
            }
        }

        public int GetPolicyId(string StaffId)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                return landingPageRepository.GetPolicyId(StaffId);
            }
        }
        #region Coff Req Availing
        public List<COffAvailling> GetCOffAvaillingRequests(string reportingmanagerid)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                return landingPageRepository.GetCOffAvaillingRequests(reportingmanagerid);
            }
        }
        #endregion

        public DocumentData GetDocumentData(string Id)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                return landingPageRepository.GetDocumentData(Id);
            }
        }
        public List<MonthlyLeavePlanner> GetMonthlyLeavePlanner(string StartDate, string EndDate, string Reporting)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                return landingPageRepository.GetMonthlyLeavePlanner(StartDate, EndDate, Reporting);
            }
        }
        //public List<Remainder> RemainderDetails()
        //{
        //    var repo = new LandingPageRepository();
        //    var lst = repo.RemainderDetails();
        //    return lst;
        //}
 public List<HolidayGroupTxn1> GetAllHolidays(string StaffID)
        {
            using (LandingPageRepository landingPageRepository = new LandingPageRepository())
            {
                return landingPageRepository.GetAllHolidays(StaffID);
            }
        }

            //Rajesh TODO add to latest BL Source
        public List<AllApplicationHistory> GetAllApplicationHistory(string staffId,int numberOfRec,string applnType)
        {
            var repo = new LandingPageRepository();
            var applications = repo.GetAllApplicationHistory(staffId,numberOfRec,applnType);
            return applications;

        }
        //Rajesh Aug 20: added aplnType in the method argument
        public List<AllPendingApprovals> GetAllPendingApplications(string staffId,string aplnType="")
        {
            var repo = new LandingPageRepository();
            var applications = repo.GetAllPendingApplications(staffId,aplnType);
            return applications;
        }
    }
}
