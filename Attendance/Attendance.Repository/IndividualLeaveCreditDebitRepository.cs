using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class IndividualLeaveCreditDebitRepository : IDisposable
    {
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }
            disposed = true;
        }
        public AttendanceManagementContext context = null;

        public IndividualLeaveCreditDebitRepository()
        {
            context = new AttendanceManagementContext();
        }
        public IndividualLeaveCreditDebit_EmpDetails GetEmployeeDetails(string StaffId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT DBO.fnGetStaffName(@StaffId ) AS StaffName , DBO.fnGetMasterName(@StaffId,'DP')" +
           "AS DEPARTMENT , (SELECT CONVERT ( VARCHAR , LEAVEBALANCE ) FROM LEAVEBALANCE WHERE STAFFID = @StaffId AND " +
            "    LEAVETYPEID = 'LV0003') AS CASUALLEAVEBALANCE , (SELECT CONVERT ( VARCHAR , LEAVEBALANCE ) FROM LEAVEBALANCE "+
           " WHERE STAFFID = @StaffId AND LEAVETYPEID = 'LV0004') AS SICKLEAVEBALANCE , (SELECT CONVERT ( VARCHAR , LEAVEBALANCE ) " +
          " FROM LEAVEBALANCE  WHERE STAFFID = @StaffId AND LEAVETYPEID = 'LV0032') AS BEREAVEMENTLEAVEBALANCE ,(SELECT CONVERT " +
          " ( VARCHAR , LEAVEBALANCE )  FROM LEAVEBALANCE  WHERE STAFFID = @StaffId AND LEAVETYPEID = 'LV0033') AS PAIDLEAVEBALANCE , " +
              " (SELECT CONVERT ( VARCHAR , LEAVEBALANCE )  FROM LEAVEBALANCE  WHERE STAFFID = @StaffId AND LEAVETYPEID = 'LV0005') AS MATERNITYLEAVEBALANCE ," +
            " (SELECT CONVERT ( VARCHAR , LEAVEBALANCE ) FROM LEAVEBALANCE WHERE STAFFID = @StaffId AND LEAVETYPEID = 'LV0007') AS PATERNITYLEAVEBALANCE ," +
           "(SELECT CONVERT ( VARCHAR , LEAVEBALANCE ) FROM LEAVEBALANCE WHERE STAFFID = @StaffId AND LEAVETYPEID = 'LV0035') AS NONCONFIRMEDLEAVEBALANCE");
       
            try
            {
                var data = context.Database.SqlQuery<IndividualLeaveCreditDebit_EmpDetails>(QryStr.ToString(),new SqlParameter("@StaffId", StaffId)).Select(d => new IndividualLeaveCreditDebit_EmpDetails() {
                    StaffName = d.StaffName,
                    Department=d.Department,
                    CasualLeaveBalance = d.CasualLeaveBalance,
                    SICKLEAVEBALANCE = d.SICKLEAVEBALANCE,
                    BEREAVEMENTLEAVEBALANCE = d.BEREAVEMENTLEAVEBALANCE ,
                    PAIDLEAVEBALANCE = d.PAIDLEAVEBALANCE,
                    MATERNITYLEAVEBALANCE = d.MATERNITYLEAVEBALANCE,
                    PATERNITYLEAVEBALANCE = d.PATERNITYLEAVEBALANCE ,
                    NONCONFIRMEDLEAVEBALANCE = d.NONCONFIRMEDLEAVEBALANCE ,

                }).FirstOrDefault();

                if(data == null)
                {
                    return new IndividualLeaveCreditDebit_EmpDetails();
                }
                else
                {
                    if(string.IsNullOrEmpty( data.StaffName) == true)
                    {
                        throw new Exception("Employee does not exists.");
                    }
                    return data;
                }
            }
            catch(Exception)
            {
                throw;
                //return new IndividualLeaveCreditDebit_EmpDetails();
            }
        }

        public List<EmployeeImportResultMesss> SaveBulkEmployeeLeaveAccount(List<IndividualLeaveCreditDebit> data, string user, bool OverWrite)
        {

            List<EmployeeImportResultMesss> EM = new List<EmployeeImportResultMesss>();
            EmployeeImportResultMesss EIm = new EmployeeImportResultMesss();
            foreach (var El in data)
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        EmployeeLeaveAccount ela = new EmployeeLeaveAccount();
                        EmployeeLeaveAccount overwrite = new EmployeeLeaveAccount();
                        var QRYSTR = new StringBuilder();



                        ela.StaffId = El.StaffId;
                        ela.LeaveTypeId = El.LeaveType;
                        ela.TransactionFlag = Convert.ToInt16(El.TransactionFlag);
                        ela.TransactionDate = DateTime.Now;
                        if (El.TransactionFlag == "1")
                        {
                            if (OverWrite == true)
                            {
                                QRYSTR.Append("select leavebalance from leavebalance where staffid=@El.StaffId and " +
                                    "leavetypeid='" + El.LeaveType + "'");
                                var lvbal = context.Database.SqlQuery<decimal>(QRYSTR.ToString(),new SqlParameter("@El.StaffId", El.StaffId)
                                    , new SqlParameter("@El.LeaveType", El.LeaveType)).FirstOrDefault();

                                overwrite.StaffId = El.StaffId;
                                overwrite.LeaveTypeId = El.LeaveType;
                                overwrite.TransactionFlag = 2;
                                overwrite.LeaveCount = lvbal * -1;
                                overwrite.LeaveCreditDebitReasonId = 24;
                                overwrite.Narration = "BULKPROCESS" + " - ZERO BALANCE DEBIT BY " + user;
                                overwrite.TransactionDate = DateTime.Now;
                                context.EmployeeLeaveAccount.Add(overwrite);
                                context.SaveChanges();
                            }
                            ela.LeaveCreditDebitReasonId = 24;
                            ela.LeaveCount = Convert.ToDecimal(El.LeaveCount);
                            ela.Narration = "BULKPROCESS" + " - APPLICATION CREDIT BY " + user;
                        }
                        else
                        {
                            ela.LeaveCount = Convert.ToDecimal(El.LeaveCount) * -1;
                            ela.LeaveCreditDebitReasonId = 26;
                            ela.Narration = "BULKPROCESS" + " - APPLICATION DEBIT BY " + user;
                        }

                        context.EmployeeLeaveAccount.Add(ela);
                        context.SaveChanges();
                        trans.Commit();
                    }
                    catch (DbEntityValidationException e)
                    {
                        trans.Rollback();
                        foreach (var exp in e.EntityValidationErrors)
                        {
                            foreach (var ex in exp.ValidationErrors)
                            {

                                EIm.Staffid = El.StaffId;
                                EIm.MessageVal = ex.ErrorMessage + "_" + ex.PropertyName;
                                EM.Add(EIm);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        EIm.Staffid = El.StaffId;
                        EIm.MessageVal = Convert.ToString(e.Message);
                        EM.Add(EIm);

                    }
                }
            }

            return EM;
        }

        public void SaveEmployeeLeaveAccount(IndividualLeaveCreditDebit data, string User)
        {
            EmployeeLeaveAccount ela = new EmployeeLeaveAccount();
            ela.StaffId = data.StaffId;
            ela.LeaveTypeId = data.LeaveType;
            ela.TransactionFlag = Convert.ToInt32(data.TransactionFlag);
            ela.TransactionDate = DateTime.Now;
            ela.Month = data.Month;
            ela.Year = data.Year;
            if (data.TransactionFlag == "1")
            {
                ela.LeaveCount = Convert.ToDecimal(data.LeaveCount);
                ela.Narration = data.Narration+" - APPLICATION CREDIT BY " + User;
                ela.LeaveCreditDebitReasonId = Convert.ToInt32(data.LeaveReson); //(16 denotes manual leave credit - see LeaveCreditDebitReason Master table)
            }
            else
            {
                ela.LeaveCount = Convert.ToDecimal(data.LeaveCount) * -1;
                ela.Narration = data.Narration+ " - APPLICATION DEBIT BY " + User;
                ela.LeaveCreditDebitReasonId = Convert.ToInt32(data.LeaveReson);  //(18 denotes manual leave debit - see LeaveCreditDebitReason Master table)
            }
            ela.IsSystemAction = false;
            ela.TransctionBy = User;

            context.EmployeeLeaveAccount.Add(ela);
            context.SaveChanges();
        }

        public  IndiviualCreditDebit GetEmpData(string StaffId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT DBO.fnGetStaffName(@StaffId) AS StaffName ," +
                " DBO.fnGetMasterName(@StaffId,'DP') AS DEPARTMENT ");
            try
            {
                var data = context.Database.SqlQuery<IndiviualCreditDebit>(QryStr.ToString(), new SqlParameter("@StaffId", StaffId)).Select(d => new IndiviualCreditDebit()
                {
                    StaffName = d.StaffName,
                    Department = d.Department
                }).FirstOrDefault();

                if (data == null)
                {
                    return new IndiviualCreditDebit();
                }
                else
                {
                    if (string.IsNullOrEmpty(data.StaffName) == true)
                    {
                        throw new Exception("Employee does not exists.");
                    }
                    return data;
                }
            }
            catch (Exception)
            {
                throw;
                //return new IndividualLeaveCreditDebit_EmpDetails();
            }
        }

        public List<LeaveTypeAndBalance> GetAllLeaveTypeAndBalance (string StaffId,string Gender)
        {
            Gender = context.Staff.Where(condition => condition.Id == StaffId).Select(select => select.Gender).FirstOrDefault();
            var QryStr = new StringBuilder();
            QryStr.Append("select leavetypeid,CAST(LEAVEBALANCE as varchar) as LeaveBalance, LeaveName,AvailableBalance from leavebalance where StaffId='" + StaffId+ "' and IsActive='1' ");
            if (Gender == "M")
            {

                QryStr.Append(" and Leavetypeid not in('LV0036', 'LV0005','LV0006')");
            }
            else
            {
                QryStr.Append(" and Leavetypeid not in('LV0036', 'LV0005','LV0039')");
            }
            try
            {
                var data = context.Database.SqlQuery<LeaveTypeAndBalance>(QryStr.ToString()).Select(d => new LeaveTypeAndBalance()
                {
                    LeaveBalance = d.LeaveBalance,
                    LeaveTypeId = d.LeaveTypeId,
                    LeaveName = d.LeaveName,
                    AvailableBalance = d.AvailableBalance.ToString()
                }).ToList();

                if (data.Count == 0)
                {
                    return new List<LeaveTypeAndBalance>();
                }
                else
                {
                    //if (string.IsNullOrEmpty(data.) == true)
                    //{
                    //    throw new Exception("Employee does not exists.");
                    //}
                    return data;
                }
            }
            catch (Exception e)
            {
                throw e;
                //return new IndividualLeaveCreditDebit_EmpDetails();
            }
        }

        public List<LeaveTypeListForExcel> GetTheLeaveList()
        {
            try
            {
                var lst = context.LeaveType.Where(s => s.IsActive == true).Select(d => new LeaveTypeListForExcel
                {
                    Name = d.Name,
                    Id = d.Id,
                    IsActive = d.IsActive
                }).ToList();
                return lst;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string ValidateLeaveCreditDebit(string StaffId, string Leavetype, string TotalDays, string TransactionFlag)
        {
            string Message = string.Empty;
            try
            {
                if (TransactionFlag == "1")
                {
                    TransactionFlag = "Credit";
                }
                else
                {
                    TransactionFlag = "Debit";
                }

                StringBuilder builder = new StringBuilder();
                builder.Append("Select DBO.[ValidateLeaveCredit_Debit](@StaffId,@Leavetype,@TotalDays,@TransactionFlag)");
                Message = context.Database.SqlQuery<string>(builder.ToString(), new SqlParameter("@StaffId", StaffId)
                    , new SqlParameter("@Leavetype", Leavetype), new SqlParameter("@TotalDays", TotalDays),
                    new SqlParameter("@TransactionFlag", TransactionFlag)).FirstOrDefault();
            }
            catch (Exception e)
            {
                Message = e.GetBaseException().Message;
            }
            return Message;
        }
    }
}
