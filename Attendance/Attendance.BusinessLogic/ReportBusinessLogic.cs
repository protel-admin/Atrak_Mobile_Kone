using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic {
    public class ReportBusinessLogic {

        ReportRepository repo = new ReportRepository();
        public List<MOffApplicationReport> GetMOffApplicationReport(string beginningdate, string endingdate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetMOffApplicationReport(beginningdate, endingdate, stafflist);
            return lst;
        }
        public List<PermissionRequisitionHistory> GetPermissionRequisitionHistory(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetPermissionRequisitionHistory(fromdate, todate, stafflist);
            return lst;
        }
        public List<LeaveApplicationReport> GetLeaveApplicationReport(string beginningdate, string endingdate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetLeaveApplicationReport(beginningdate, endingdate, stafflist);
            return lst;
        }
        public List<CanteenReport> GetCanteenReport( string beginningdate , string endingdate , string stafflist )
        {
            var repo = new ReportRepository();
            var lst = repo.GetCanteenReport( beginningdate , endingdate , stafflist );
            return lst;
        }
        public List<NightShiftData> GetNightShiftDataRepoert(string beginningdate, string endingdate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetNightShiftDataRepoert(beginningdate, endingdate, stafflist);
            return lst;
        }

        public List<ShiftViolation> GetShiftViolation(string beginningdate, string endingdate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetShiftViolation(beginningdate, endingdate, stafflist);
            return lst;
        }

        public List<Form15> GetForm15(string StaffId , string FromDate , string ToDate)
        {
            var repo = new ReportRepository();
            var lst = repo.GetForm15(StaffId, FromDate, ToDate);
            return lst;
        }

        public Form15StaffPersonalDetails GetForm15StaffPersonalDetails(string StaffId)
        {
            var repo = new ReportRepository();
            var data = repo.GetForm15StaffPersonalDetails(StaffId);
            return data;
        }
        public List<LeaveApplicationDeatails> GetLeaveNewApplicationReport(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetLeaveNewApplicationReport(fromdate, todate, stafflist);
            return lst;
        }

        public List<PresentOnNFH> GetPresentOnNFH(string beginningdate, string endingdate , string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetPresentOnNFH(beginningdate, endingdate, stafflist);
            return lst;
        }

        public List<ShiftChangeStatement> GetShiftChangeStatement(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetShiftChangeStatement(fromdate, todate, stafflist);
            return lst;
        }

        public List<OnDutyReportModel> GetOutDoorStatement(string beginningdate, string endingdate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetOutDoorStatement(beginningdate, endingdate, stafflist);
            return lst;
        }

        public List<ContinuousAbsent> GetContinuousAbsentList(string fromdate, string todate, string stafflist, int DayCount)
        {
            var repo = new ReportRepository();
            var lst = repo.GetContinuousAbsentList(fromdate, todate, stafflist , DayCount);
            return lst;
        }

        public List<ContinuousLateComing> GetContinuousLateComing(string fromdate, string todate, string stafflist, int DayCount)
        {
            var repo = new ReportRepository();
            var lst = repo.GetContinuousLateComing(fromdate, todate, stafflist, DayCount);
            return lst;
        }

        public List<GraceTime> GetGraceTime(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetGraceTime(fromdate, todate, stafflist);
            return lst;
        }
        

        public List<ContinuousEarlyGoing> GetContinuousEarlyGoing(string fromdate, string todate, string stafflist, int DayCount)
        {
            var repo = new ReportRepository();
            var lst = repo.GetContinuousEarlyGoing(fromdate, todate, stafflist, DayCount);
            return lst;
        }

        public List<MissedPunchList> GetMissedPunchList(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetMissedPunchList(fromdate, todate, stafflist);
            return lst;
        }

        public List<RawPunchDetails> GetRawPunchDetailsReport( string fromdate , string todate , string stafflist )
        {
            var repo = new ReportRepository();
            var lst = repo.GetRawPunchDetailsReport( fromdate , todate , stafflist );
            return lst;
        }

        public List<FirstInLastOutDiamlerNew> GetFirstInLastOuts(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetFirstInLastOuts(fromdate, todate, stafflist);
            return lst;
        }

        public List<PunchDetails> GetPunchDetails(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetPunchDetails(fromdate, todate, stafflist);
            return lst;
        }

        public List<PresentList> GetPresentLists(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetPresentLists(fromdate, todate, stafflist);
            return lst;
        }

        public List<PresentList> GetAbsentLists(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetAbsentLists( fromdate , todate , stafflist );
            return lst;
        }

        public List<PresentList> GetAttendanceLists( string fromdate , string todate , string stafflist ) {
            var repo = new ReportRepository();
            var lst = repo.GetAttendanceLists( fromdate , todate , stafflist );
            return lst;
        }

        public List<DailyPerformance> GetDailyPerformance( string fromdate , string todate , string stafflist ) {
            var repo = new ReportRepository();
            var lst = repo.GetDailyPerformance( fromdate , todate , stafflist );
            return lst;
        }

        public List<PresentAndAbsentCountReport> GetPresentAndAbsentReport(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetPresentAndAbsentReport(fromdate, todate, stafflist);
            return lst;
        }

        public List<ShiftAllowanceReport> GetShiftAllowanceReport(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetShiftAllowanceReport(fromdate, todate, stafflist);
            return lst;
        }

        public List<AttendanceIncentive> GetAttendanceIncentiveReport(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetAttendanceIncentiveReport(fromdate, todate, stafflist);
            return lst;
        }

        public List<GetHeadCountAlertDetails> GetHeadCountAlertReport(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetHeadCountAlertReport(fromdate, todate, stafflist);
            return lst;
        }

        public List<BasicDetails> GetBasicDetailsReport(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetBasicDetailsReport(fromdate, todate, stafflist);
            return lst;
        }

        public List<Form25> GetForm25(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetForm25(fromdate, todate, stafflist);
            return lst;
        }

        public List<Form25FLSmidth> GetForm25FLSmidth(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetForm25FLSmidth( fromdate , todate , stafflist );
            return lst;
        }
        public List<Form25> GetForm25SL(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetForm25SL(fromdate, todate, stafflist);
            return lst;
        }
        public List<Form25> GetForm25Payroll(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetForm25Payroll(fromdate, todate, stafflist);
            return lst;
        }

        public List<Form25> GetForm25status(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetForm25status(fromdate, todate, stafflist);
            return lst;
        }

        
        public List<ManualPunchApprovalList> GetManualPunchApprovalLists(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetManualPunchApprovalLists(fromdate, todate, stafflist);
            return lst;
        }

        public List<ShiftChangeApproval> GetShiftChangeApprovalLists(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetShiftChangeApprovalLists(fromdate, todate, stafflist);
            return lst;
        }

        public List<PlannedLeave> GetPlannedLeaveLists(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetPlannedLeaveLists(fromdate, todate, stafflist);
            return lst;
        }

        public List<UnPlannedLeave> GetUnPlannedLeaveLists( string fromdate , string todate , string stafflist )
        {
            var repo = new ReportRepository();
            var lst = repo.GetUnPlannedLeaveLists(fromdate, todate, stafflist);
            return lst;
        }



        public List<DepartmentSummary> GetDepartmentSummaryLists(string fromdate, string todate, string stafflist, string CompanyId)
        {
            var repo = new ReportRepository();
            var lst = repo.GetDepartmentSummaryList(fromdate, todate, stafflist, CompanyId);
            return lst;
        }

        public List<LateComers> GetLateComersLists(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetLateComersList(fromdate, todate, stafflist);
            return lst;
        }

        public List<OvertimeStatement> GetOvertimeStatementLists(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetOvertimeStatementList(fromdate, todate, stafflist);
            return lst;
        }


        public List<ExtraHoursWorked> GetExtraHoursWorkedLists(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetExtraHoursWorkedList(fromdate, todate, stafflist);
            return lst;
        }

        public List<EarlyDeparture> GetEarlyDepartureLists(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetEarlyDepartureLists(fromdate, todate, stafflist);
            return lst;
        }

        public List<EarlyArraival> GetEarlyArraival(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetEarlyArraival(fromdate, todate, stafflist);
            return lst;
        }


       

      
        //public DataSet GetStaffInformation(string Flag)
        //{
        //    var repo = new ReportRepository();
        //    var lst = repo.GetStaffInformation(Flag);
        //    return lst;
        //}



        public List<DailyAttendance> GetDailyAttendance(string fromdate, string todate, string stafflist, bool SUMrpt)
        {
            var repo = new ReportRepository();
            var lst = repo.GetDailyAttendance(fromdate, todate, stafflist, SUMrpt);
            return lst;
        }

        public List<DailyExtraHoursWorkedDetails> GetExtraHoursWorkedDetails(string fromdate, string todate, string stafflist,bool flag)
        {
            var repo = new ReportRepository();
            var lst = repo.GetExtraHoursWorkedDetails(fromdate, todate, stafflist,flag);
            return lst;
        }

        public List<DepartmentWiseDailyAttendance> GetDepartmentWiseDailyAttendance(string fromdate, string todate, string stafflist, bool SUMrpt)
        {
            var repo = new ReportRepository();
            var lst = repo.GetDepartmentWiseDailyAttendance(fromdate, todate, stafflist, SUMrpt);
            return lst;
        }

        public List<BranchWiseDailyAttendance> GetBranchWiseDailyAttendance(string fromdate, string todate, string stafflist, bool SUMrpt)
        {
            var repo = new ReportRepository();
            var lst = repo.GetBranchWiseDailyAttendance(fromdate, todate, stafflist, SUMrpt);
            return lst;
        }

        public List<OverTime> GetOverTime(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetOverTime(fromdate, todate, stafflist);
            return lst;
        }

        public List<AttendanceDataView> GetOTBusinessLogic(string StaffId, string fromdate, string todate)
        {
            var repo = new ReportRepository();
            var lst = repo.GetOTRepository(StaffId, fromdate, todate);
            return lst;
        }
        public List<EmployeeMasterSummaryReportModel> EmployeeMasterSummaryReportBusinessLogic(string staffList)
        {
            return repo.EmployeeMasterSummaryReportRepository(staffList);
        }
        public List<LeaveRequisitionHistoryModel> GetLeaveRequisitionHistoryRepot(string beginningdate, string endingdate, string stafflist)
        {
            return repo.GetLeaveRequisitionHistoryRepot(beginningdate, endingdate, stafflist);
        }
        public List<CoffAvailingModel> GetCompOffAvailedReport(string beginningdate, string endingdate, string stafflist)
        {
            return repo.GetCompOffAvailedReport(beginningdate, endingdate, stafflist);
        }
        public List<CurrentDayInSwipModel> GetCurrentDayInSwipeReport(string beginningdate, string endingdate, string stafflist)
        {
            return repo.GetCurrentDayInSwipeReport(beginningdate, endingdate, stafflist);
        }
        public List<COffCreditReportModel> COffCreditReqBusinessLogic(string StaffList, string FromDate, string ToDate)
        {
            return repo.COffCreditReqReportRepository(StaffList, FromDate, ToDate);
        }
        public List<BusinessTravelReportModel> GetBusinessTravelReportBusinessLogic(string StaffId, string fromdate, string todate)
        {
            var repo = new ReportRepository();
            var lst = repo.GetBusinessTravelReportRepository(StaffId, fromdate, todate);
              return lst;
        }
        public List<CommonPermissionReportModel> GetCommonPermissionReportBusinessLogic(string StaffId, string fromdate, string todate)
        {
            var repo = new ReportRepository();
            var lst = repo.GetCommonPermissionReportRepository(StaffId, fromdate, todate);
            return lst;
        }
        public List <LeaveBalance> GetLeaveBalanceReport(string StaffId,  string todate)
        {
            var Repo = new ReportRepository();
            var lst = Repo.GetLeaveBalanceReport(StaffId, todate);
            return lst;
        }
        public List<LeaveAvailedReport> GetLeaveAvailedReport(string StaffId, string fromdate, string todate)
        {
            var Repo = new ReportRepository();
            var Lst = Repo.GetLeaveAvailedReport(StaffId, fromdate, todate);
            return Lst;
        }
        #region Daimler
        public List<DeptWiseGenderHeadCount> GetDeptWiseGenderHeadCount(string fromdate, string todate)
        {
            return repo.GetDeptWiseGenderHeadCount(fromdate, todate);
        }
        public List<DrawAttendanceYearlySummary> GetAttendanceYearlySummary(string fromdate,string stafflist)
        {
            return repo.GetAttendanceYearlySummary(fromdate, stafflist);
        }
        public List<ShiftSummaryReport> GetShiftSummaryReport(string fromdate, string todate, string stafflist)
        {
            return repo.GetShiftSummaryReport(fromdate,  todate,  stafflist);
        }
        public List<LateInEarlyOutReport> GetLateInEarlyOutReport(string fromdate, string todate, string stafflist)
        {
            return repo.GetLateInEarlyOutReport (fromdate, todate, stafflist);
        }
        public List<DrawFORMS> GetFORM_S(string stafflist)
        {
            return repo.GetFORM_S(stafflist);
        }
        public List<DrawFORM10> GetFORM10(string fromdate, string todate, string stafflist)
        {
            return repo.GetFORM10(fromdate,  todate,  stafflist);
        }
        public List<ExtraHoursWorked> GetExtraHoursWorkedDetails(string fromdate, string todate, string stafflist)
        {
            return repo.GetExtraHoursWorkedDetails(fromdate, todate, stafflist);
        }
        public List<HWOWokingReport> GetHWOWokingReport(string beginningdate, string endingdate, string stafflist)
        {
            return repo.GetHWOWokingReport(beginningdate, endingdate, stafflist);
        }
        public List<LeaveSummaryReport> GetLeaveSummaryReport(string Year, string stafflist)
        {
            return repo.GetLeaveSummaryReport(Year, stafflist);
        }
        public List<EmployeeAdditionDeletionReport> GetEmpAddioonDeletionReport(string beginningdate, string endingdate)
        {
            return repo.GetEmpAdditionDeletionReport(beginningdate, endingdate);
        }
        #endregion
        #region Swing Stetter
        public List<SalaryOTSAfinal> GetSalaryOTSAfinalDetails(string beginningdate, string endingdate, string stafflist)
        {
            return repo.GetSalaryOTSAfinalDetails(beginningdate, endingdate, stafflist);
        }
        public List<OverTime> NewGetOverTime(string fromdate, string todate, string stafflist)
        {
            return repo.GetOverTime(fromdate, todate, stafflist);
        }
        public List<ShiftAllowance> GetShiftAllowance(string fromdate, string todate, string stafflist)
        {
            return repo.GetShiftAllowance(fromdate, todate, stafflist);
        }
        public List<YearlyReportShiftDetails> GetYearlyReportShiftDetails(string fromdate, string todate, string stafflist)
        {
            return repo.GetYearlyReportShiftDetails(fromdate, todate, stafflist);
        }
        public List<YearlyReportWorkedDays> GetYearlyReportWorkedDays(string fromdate, string todate, string stafflist)
        {
            return repo.GetYearlyReportWorkedDays(fromdate, todate, stafflist);
        }
        public List<YearlyOTReport> GetYearlyOTReport(string fromdate, string todate, string stafflist)
        {
            return repo.GetYearlyOTReport(fromdate, todate, stafflist);
        }
        public List<LeaveReport> GetLeaveReport(string fromdate, string todate, string stafflist)
        {
            return repo.GetLeaveReport(fromdate, todate, stafflist);
        }
        public List<ShopFloorAttendance> GetShopFloorAttendance(string fromdate, string todate, string stafflist)
        {
            return repo.GetShopFloorAttendance(fromdate, todate, stafflist);
        }
        public List<DailyAttendance_Trd_Dep_Cat> GetTradeWiseHC(string fromdate, string todate)
        {
            return repo.GetTradeWiseHC(fromdate, todate);
        }
        public List<DailyAttendance_Trd_Dep_Cat> GetDepartmentWiseHC(string fromdate, string todate)
        {
            return repo.GetDepartmentWiseHC(fromdate, todate);
        }
        public List<DailyAttendance_Trd_Dep_Cat> GetCategoryWiseHC(string fromdate, string todate)
        {
            return repo.GetCategoryWiseHC(fromdate, todate);
        }
        public List<DailyAttendance_Trd_Dep_Cat> GetCategoryWiseAbsenteeism(string fromdate, string todate)
        {
            return repo.GetCategoryWiseAbsenteeism(fromdate, todate);
        }
        public List<ShopFloorAttendanceMonthWise> GetShoopFloorAttendanceMonthWise(string fromdate, string todate, string stafflist)
        {
            return repo.GetShoopFloorAttendanceMonthWise(fromdate, todate, stafflist);
        }
        public List<ExtraHoursWorked> GetExtraHours(string fromdate, string todate, string stafflist)
        {
            return repo.GetExtraHoursWorkedDetails(fromdate, todate, stafflist);
        }
        #endregion
        public List<LeaveTransactionDetails> GetLeaveTransactionDetails(string stafflist, string fromdate, string todate)
        {
            try
            {
                var repo = new ReportRepository();
                var lst = repo.GetLeaveTransactionDetails(stafflist, fromdate, todate);
                return lst;
            }
            catch 
            {
                return new List<LeaveTransactionDetails>();
            }
        }
        public List<WorkFromHome> GetWorkFromHomeRequisition(string stafflist, string fromdate, string todate)
        {
            try
            {
                var repo = new ReportRepository();
                var lst = repo.GetWorkFromHomeRequisition(stafflist, fromdate, todate);
                return lst;
            }
            catch 
            {
                return new List<WorkFromHome>();
            }
        }
        public List<LeaveBalanceReport> GetLeaveBalanceHistoryReport(string stafflist, string endingdate)
        {
            try
            {
                var repo = new ReportRepository();
                var list = repo.GetLeaveBalanceHistoryReport(stafflist, endingdate);
                return list;
            }
            catch
            {
                return new List<LeaveBalanceReport>();
            }
        }
        public List<GetCompOffLapsReport> GetCompOffLapsReport(string stafflist, string fromDate, string toDate)
        {
            try
            {
                var repo = new ReportRepository();
                var list = repo.GetCompOffLapsReport(stafflist, fromDate, toDate);
                return list;
            }
            catch
            {
                return new List<GetCompOffLapsReport>();
            }
        }
        public List<AutoLeaveDeduction> GetLeaveAutoDeductionReport(string stafflist, string fromDate, string toDate)
        {
            try
            {
                var repo = new ReportRepository();
                var list = repo.GetLeaveAutoDeductionReport(stafflist, fromDate, toDate);
                return list;
            }
            catch
            {
                return new List<AutoLeaveDeduction>();
            }
        }
        public List<OffRoleAttendanceReport> GetOffRoleAttendanceReport(string staffList, string fromDate, string toDate)
        {
            try
            {
                ReportRepository reportRepository = new ReportRepository();
                var lst = reportRepository.GetOffRoleAttendanceReport(staffList, fromDate, toDate);
                return lst;
            }
            catch
            {
                return null;
            }
        }
        public List<LeaveNightShiftAllowance> GetNightShiftAllowanceDetails(string stafflist, string fromDate, string toDate)
        {
            try
            {
                var repo = new ReportRepository();
                var list = repo.GetNightShiftAllowanceDetails(stafflist, fromDate, toDate);
                return list;
            }
            catch
            {
                return new List<LeaveNightShiftAllowance>();
            }
        }
        public List<FormQ> GetFormQ(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetFormQ(fromdate, todate, stafflist);
            return lst;
        }
        public List<ManualAttendanceStatusChange> GetManualAttendanceStatusChange(string stafflist, string fromDate, string toDate)
        {
            try
            {
                var repo = new ReportRepository();
                var list = repo.GetManualAttendanceStatusChange(stafflist, fromDate, toDate);
                return list;
            }
            catch
            {
                return new List<ManualAttendanceStatusChange>();
            }
        }
        public List<ShiftExtension> GetShiftExtensionReport(string stafflist, string fromdate, string todate)
        {
            try
            {
                var repo = new ReportRepository();
                var lst = repo.GetShiftExtensionReport(stafflist, fromdate, todate);
                return lst;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        public List<NightShiftCount> GetNightShiftCount(string beginningdate, string endingdate, string stafflist)
        {
            try
            {
                var repo = new ReportRepository();
                var lst = repo.GetNightShiftCount(beginningdate, endingdate, stafflist);
                return lst;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        public List<HolidayWorkingDetails> GetHolidayWorkingDetails(string fromdate, string todate, string stafflist)
        {
            var repo = new ReportRepository();
            var lst = repo.GetHolidayWorkingDetails(fromdate, todate, stafflist);
            return lst;
        }
        public List<HolidayWorkingRequisition> GetHolidayWorkingRequisitionHistory(string stafflist, string fromdate, string todate)
        {
            try
            {
                var repo = new ReportRepository();
                var lst = repo.GetHolidayWorkingRequisitionHistory(stafflist, fromdate, todate);
                return lst;
            }                         
            catch (Exception err)
            {
                throw err;
            }
        }
        public List<AutoLeaveDeductionDryRun> GetAutoLeaveDeductionDryRun(string stafflist, string fromdate, string todate)
        {
            try
            {
                var repo = new ReportRepository();
                var lst = repo.GetAutoLeaveDeductionDryRun(stafflist, fromdate, todate);
                return lst;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
