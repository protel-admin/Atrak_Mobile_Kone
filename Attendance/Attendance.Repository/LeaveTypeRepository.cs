using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository {
    public class LeaveTypeRepository : IDisposable
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
        private AttendanceManagementContext context;

        public LeaveTypeRepository()
        {
            context = new AttendanceManagementContext();
        }
        StringBuilder builder = new StringBuilder();
        string Message = string.Empty;
        public List<LeaveView> GetLeaveView()
        {
            var qryStr = new StringBuilder();

            qryStr.Clear();
            qryStr.Append("Select Id , Name , ShortName , case when IsAccountable = 1 then 'Yes' else 'No' end as IsAccountable,"
                +" case when IsEncashable = 1 then 'Yes' else 'No' end as IsEncashable, case when IsPaidLeave = 1 then 'Yes' else 'No' end as"
                +" IsPaidLeave, case when IsCommon = 1 then 'Yes' else 'No' end as IsCommon, case when IsPermission = 1 then 'Yes' else 'No'"
                +" end as IsPermission, case when CarryForward = 1 then 'Yes' else 'No' end as CarryForward, case when IsActive = 1 then"
                +" 'Yes' else 'No' end as IsActive , CreatedOn , CreatedBy from Leavetype Order By Name ASC");

            var lstLV = context.Database.SqlQuery<LeaveView>(qryStr.ToString()).Select(d => new LeaveView()
            {
                Id = d.Id,
                Name = d.Name ,
                ShortName = d.ShortName ,
                IsAccountable = d.IsAccountable ,
                IsEncashable = d.IsEncashable ,
                IsPaidLeave = d.IsPaidLeave ,
                IsCommon = d.IsCommon ,
                IsPermission = d.IsPermission ,
                CarryForward = d.CarryForward ,
                IsActive = d.IsActive , 
                CreatedOn = d.CreatedOn ,
                CreatedBy = d.CreatedBy
            }).ToList();

            if (lstLV.Count == 0)
            {
                return new List<LeaveView>();
            }
            else
            {
                return lstLV;
            }
        }

        public void SaveLeaveType(LeaveType lt)
        {
            MasterRepository a= new MasterRepository ( );
            if ( string.IsNullOrEmpty ( lt.Id ) == true ) {
                var maxid = string.Empty;
                var lastid = string.Empty;
                maxid = a.getmaxid ( "leavetype" , "Id" , "LV" , "" , 6 ,ref lastid);
                lt.Id = maxid;
            }
            context.LeaveType.AddOrUpdate(lt);
            context.SaveChanges ( );            
        }
        #region Bulk Leave Credit Debit
        public List<string> GetLeaveTypeRepository()
        {
            List<string> lst = new List<string>();
            try
            {
                builder = new StringBuilder();
                builder.Append("select Name from LeaveType where Id in ('LV0003','LV0004','LV0007','LV0038')");
                lst=context.Database.SqlQuery<string>(builder.ToString()).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public List<string> GetLeaveCreditDebitReasonRepository()
        {
            List<string> lst = new List<string>();
            try
            {
                lst = context.LeaveCreditDebitReason.Where(condition => condition.IsActive == true).Select(select => select.Name).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }
        public int CheckValidOrNotStaffIdRepository(string StaffId)
        {
            int len = 0;
            try
            {
                //return 1 => Valid Staff
                //return 0 => InValid Staff
                len = context.Staff.Where(condition => condition.Id == StaffId && condition.StaffStatusId == 1).Select(select => select.Id).Count();
            }
            catch
            {
                len = 0;
            }
            return len;
        }
        public string CheckValidOrNotLeaveTypeRepository(string LeaveType, string StaffId, int ValidStaff)
        {
            string Valid = "0";
            try
            {
                //return 1 => Valid LeaveType
                //Otherwise  => InValid LeaveType
                string validLeaveType = context.LeaveType.Where(condition => condition.Name.ToUpper() == LeaveType.ToUpper() || condition.ShortName.ToUpper() == LeaveType.ToUpper()).Select(select => select.Id).FirstOrDefault();
                if (string.IsNullOrEmpty(validLeaveType).Equals(true))
                {
                    Valid = "0";
                }
                else
                {
                    if (ValidStaff == 1)
                    {
                        builder = new StringBuilder();
                        builder.Append("select LeaveTypeId from LeaveGroupTxn where LeaveGroupId=(select LeaveGroupId from StaffOfficial where StaffId=@StaffId) and IsActive=1");
                        List<string> List = context.Database.SqlQuery<string>(builder.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
                        if (List != null && List.Contains(validLeaveType))
                        {
                            Valid = validLeaveType;
                        }
                        else if (List == null)
                        {
                            Valid = "Config";
                        }
                        else if (List.Contains(validLeaveType) == false)
                        {
                            Valid = "MisMatch";
                        }
                    }
                    else
                    {
                        Valid = validLeaveType;
                    }
                }
            }
            catch
            {
                Valid = "0";
            }
            return Valid;
        }
        public int CheckValidOrNotReasonRepository(string LeaveCreditDebitReason)
        {
            int Result = 0;
            try
            {
                //return 1 => Valid LeaveCreditDebitReason
                //return 0 => InValid LeaveCreditDebitReason
                int? validReason = context.LeaveCreditDebitReason.Where(condition => condition.Name.ToUpper() == LeaveCreditDebitReason.ToUpper() || condition.ShortName.ToUpper() == LeaveCreditDebitReason.ToUpper()).Select(select => select.Id).FirstOrDefault();
                if (validReason != null && validReason != 0)
                {
                    Result = Convert.ToInt32(validReason);
                }
                else
                {
                    Result = 0;
                }
            }
            catch
            {
                Result = 0;
            }
            return Result;
        }

        public string SaveEmployeeLeaveAccountRepository(List<BulkLeaveCreditDebitModel> list)
        {
            string Message = string.Empty;
            using (DbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (BulkLeaveCreditDebitModel model in list)
                    {
                        EmployeeLeaveAccount tbl = new EmployeeLeaveAccount();
                        tbl.StaffId = model.StaffId.ToUpper();
                        tbl.LeaveTypeId = model.LeaveType;
                        tbl.TransactionDate = DateTime.Now;
                        if (model.TransactionType.ToUpper() == "C" || model.TransactionType.ToUpper() == "CREDIT" || model.TransactionType.ToUpper() == "CREDITS" || model.TransactionType.ToUpper() == "CR")
                        {
                            tbl.TransactionFlag = 1;
                        }
                        else if (model.TransactionType.ToUpper() == "D" || model.TransactionType.ToUpper() == "DEBIT" || model.TransactionType.ToUpper() == "DEBITS" || model.TransactionType.ToUpper() == "DR")
                        {
                            tbl.TransactionFlag = 2;
                        }
                        tbl.LeaveCount = tbl.TransactionFlag == 1 ? Convert.ToDecimal(model.LeaveCount) : -Convert.ToDecimal(model.LeaveCount);
                        tbl.LeaveCreditDebitReasonId = Convert.ToInt32(model.LeaveCreditDebitReason.Substring(0, 1));
                        tbl.Narration = model.LeaveCreditDebitReason.Substring(1, model.LeaveCreditDebitReason.Length - 1);
                        tbl.IsSystemAction = false;
                        tbl.Year = model.Year;
                        tbl.Month = model.Month;
                        context.EmployeeLeaveAccount.Add(tbl);
                        context.SaveChanges();
                    }
                    ((IObjectContextAdapter)context).ObjectContext.CommandTimeout = 360;

                    trans.Commit();
                    Message = "success";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
            return Message;
        }
        #endregion
    }
}
