using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class ReportBusinessLogic
    {

        public List<MOffApplicationReport> GetMOffApplicationReport(string beginningdate, string endingdate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {                
                return reportRepository.GetMOffApplicationReport(beginningdate, endingdate, stafflist);                
            }
        }

        public List<LeaveApplicationReport> GetLeaveApplicationReport(string beginningdate, string endingdate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetLeaveApplicationReport(beginningdate, endingdate, stafflist);
            }
        }
        public List<CanteenReport> GetCanteenReport(string beginningdate, string endingdate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetCanteenReport(beginningdate, endingdate, stafflist);
            }
        }
        public List<NightShiftData> GetNightShiftDataRepoert(string beginningdate, string endingdate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetNightShiftDataRepoert(beginningdate, endingdate, stafflist);
            }
        }

        public List<ShiftViolation> GetShiftViolation(string beginningdate, string endingdate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetShiftViolation(beginningdate, endingdate, stafflist);
            }
        }

        public List<Form15> GetForm15(string StaffId, string FromDate, string ToDate)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetForm15(StaffId, FromDate, ToDate);
            }
        }

        public Form15StaffPersonalDetails GetForm15StaffPersonalDetails(string StaffId)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetForm15StaffPersonalDetails(StaffId);
            }
        }
        public List<LeaveApplicationListNew> GetLeaveNewApplicationReport(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetLeaveNewApplicationReport(fromdate, todate, stafflist);
            }
        }

        public List<PermissionOffReport> GetPermissionOff(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetPermissionOff(fromdate, todate, stafflist);
            }
        }

        public List<PresentOnNFH> GetPresentOnNFH(string beginningdate, string endingdate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetPresentOnNFH(beginningdate, endingdate, stafflist);
            }
        }

        public List<ShiftChangeStatement> GetShiftChangeStatement(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetShiftChangeStatement(fromdate, todate, stafflist);
            }
        }

        public List<ODApplicationListNew> GetOutDoorStatement(string beginningdate, string endingdate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetOutDoorStatement(beginningdate, endingdate, stafflist);
            }
        }

        public List<ContinuousAbsent> GetContinuousAbsentList(string fromdate, string todate, string stafflist, int DayCount)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetContinuousAbsentList(fromdate, todate, stafflist, DayCount);
            }
        }

        public List<ContinuousLateComing> GetContinuousLateComing(string fromdate, string todate, string stafflist, int DayCount)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetContinuousLateComing(fromdate, todate, stafflist, DayCount);
            }
        }

        public List<GraceTime> GetGraceTime(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetGraceTime(fromdate, todate, stafflist);
            }
        }


        public List<ContinuousEarlyGoing> GetContinuousEarlyGoing(string fromdate, string todate, string stafflist, int DayCount)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetContinuousEarlyGoing(fromdate, todate, stafflist, DayCount);
            }
        }

        public List<MissedPunchList> GetMissedPunchList(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetMissedPunchList(fromdate, todate, stafflist);
            }
        }

        public List<RawPunchDetails> GetRawPunchDetailsReport(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetRawPunchDetailsReport(fromdate, todate, stafflist);
            }
        }

        public List<FirstInLastOutDiamlerNew> GetFirstInLastOuts(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetFirstInLastOuts(fromdate, todate, stafflist);
            }
        }

        public List<PunchDetails> GetPunchDetails(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetPunchDetails(fromdate, todate, stafflist);
            }
        }

        public List<PresentList> GetPresentLists(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetPresentLists(fromdate, todate, stafflist);
            }
        }

        public List<PresentList> GetAbsentLists(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetAbsentLists(fromdate, todate, stafflist);
            }
        }

        public List<PresentList> GetAttendanceLists(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetAttendanceLists(fromdate, todate, stafflist);
            }
        }

        public List<DailyPerformance> GetDailyPerformance(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetDailyPerformance(fromdate, todate, stafflist);
            }
        }

        public List<StaffExport> GetStaffDetails(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetStaffDetails(fromdate, todate, stafflist);
            }
        }

        public List<Form25> GetForm25(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetForm25(fromdate, todate, stafflist);
            }
        }

        public List<Form25FLSmidth> GetForm25FLSmidth(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetForm25FLSmidth(fromdate, todate, stafflist);
            }
        }
        public List<Form25> GetForm25SL(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetForm25SL(fromdate, todate, stafflist);
            }
        }
        public List<Form25> GetForm25Payroll(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetForm25Payroll(fromdate, todate, stafflist);
            }
        }


        public List<ManualPunchApprovalList> GetManualPunchApprovalLists(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetManualPunchApprovalLists(fromdate, todate, stafflist);
            }
        }

        public List<ShiftChangeApproval> GetShiftChangeApprovalLists(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetShiftChangeApprovalLists(fromdate, todate, stafflist);
            }
        }

        public List<PlannedLeave> GetPlannedLeaveLists(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetPlannedLeaveLists(fromdate, todate, stafflist);
            }
        }

        public List<UnPlannedLeave> GetUnPlannedLeaveLists(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetUnPlannedLeaveLists(fromdate, todate, stafflist);
            }
        }

        public List<DepartmentSummary> GetDepartmentSummaryLists(string fromdate, string todate, string stafflist, string CompanyId)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetDepartmentSummaryList(fromdate, todate, stafflist, CompanyId);
            }
        }

        public List<LateComers> GetLateComersLists(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetLateComersList(fromdate, todate, stafflist);
            }
        }

        public List<OvertimeStatement> GetOvertimeStatementLists(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetOvertimeStatementList(fromdate, todate, stafflist);
            }
        }


        public List<ExtraHoursWorked> GetExtraHoursWorkedLists(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetExtraHoursWorkedList(fromdate, todate, stafflist);
            }
        }

        public List<EarlyDeparture> GetEarlyDepartureLists(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetEarlyDepartureLists(fromdate, todate, stafflist);
            }
        }

        public List<EarlyArraival> GetEarlyArraival(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetEarlyArraival(fromdate, todate, stafflist);
            }
        }





        //public DataSet GetStaffInformation(string Flag)
        //{
        //    var repo = new ReportRepository();
        //    var lst = repo.GetStaffInformation(Flag);
        //    return lst;
        //}



        public List<DailyAttendance> GetDailyAttendance(string fromdate, string todate, string stafflist, bool SUMrpt)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetDailyAttendance(fromdate, todate, stafflist, SUMrpt);
            }
        }

        public List<DailyExtraHoursWorkedDetails> GetExtraHoursWorkedDetails(string fromdate, string todate, string stafflist, bool flag)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetExtraHoursWorkedDetails(fromdate, todate, stafflist, flag);
            }
        }

        public List<DepartmentWiseDailyAttendance> GetDepartmentWiseDailyAttendance(string fromdate, string todate, string stafflist, bool SUMrpt)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetDepartmentWiseDailyAttendance(fromdate, todate, stafflist, SUMrpt);
            }
        }

        public List<BranchWiseDailyAttendance> GetBranchWiseDailyAttendance(string fromdate, string todate, string stafflist, bool SUMrpt)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetBranchWiseDailyAttendance(fromdate, todate, stafflist, SUMrpt);
            }
        }
        public List<OverTime> GetOverTime(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetOverTime(fromdate, todate, stafflist);
            }
        }
        public List<DrawFORM1> GetForm1(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetFORM1(fromdate, todate, stafflist);
            }
        }
        public List<DrawFORM10> GetForm10(string stafflist, string fromdate, string todate)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetFORM10(stafflist, fromdate, todate);
            }
        }
        public List<DrawFORM12> GetForm12(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetFORM12(fromdate, todate, stafflist);
            }
        }
        public List<FormVI> GetFormVI(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetFormVI(fromdate, todate, stafflist);
            }
        }
        public List<FormQ> GetFormQ(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetFormQ(fromdate, todate, stafflist);
            }
        }
        public List<DrawFORMS> GetFORM_S(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetFORM_S(fromdate, todate, stafflist);
            }
        }
        public List<LeaveSummary> LeaveSummary(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.LeaveSummary(fromdate, todate, stafflist);
            }
        }
        public List<ShiftExtension> ShiftExtension(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.ShiftExtension(fromdate, todate, stafflist);
            }
        }
        public List<CandAReport> CandAReport(string fromdate, string todate, string stafflist)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.CandAReport(fromdate, todate, stafflist);
            }
        }
        public List<AttendanceDataView> GetOTBusinessLogic(string StaffId, string fromdate, string todate,string LogedInUser)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetOTRepository(StaffId, fromdate, todate, LogedInUser);
            }
        }
        public List<BusinessTravelReportModel> GetBusinessTravelReportBusinessLogic(string StaffId, string fromdate, string todate)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetBusinessTravelReportRepository(StaffId, fromdate, todate);
            }
        }
        public List<CommonPermissionReportModel> GetCommonPermissionReportBusinessLogic(string StaffId, string fromdate, string todate)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetCommonPermissionReportRepository(StaffId, fromdate, todate);
            }
        }
        public List<CompOffRequistionModel> GetCompOffAvailingRequisitionBusinessLogic(string StaffId, string fromdate, string todate)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetCompOffAvailingRequisitionRepository(StaffId, fromdate, todate);
            }
        }
        public List<LeaveDeduction> GetLeaveDuductionReport(string StaffId, string fromdate, string todate)
        {
            using (ReportRepository reportRepository = new ReportRepository())
            {
                return reportRepository.GetLeaveDeductionReport(StaffId, fromdate, todate);
            }
        }
    }
}
