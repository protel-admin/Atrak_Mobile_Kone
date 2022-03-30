using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class LeaveGroupRepository : IDisposable
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
        private AttendanceManagementContext context = null;

        public LeaveGroupRepository()
        {
            context = new AttendanceManagementContext();
        }

        public void SaveInformation(LeaveTypeSave lst)
        {
            using (DbContextTransaction Trans = context.Database.BeginTransaction())
            {
                try
                {
                    LeaveGroup _lg_ = new LeaveGroup();
                    string lastid = string.Empty;

                    if( string.IsNullOrEmpty( lst.LeaveId ) == true ) {
                        var mr = new MasterRepository();
                        lastid = mr.getmaxid( "leavegroup" , "Id" , "LG" , "" , 6 , ref lastid );
                        _lg_.Id = lastid;
                    } else {
                        _lg_.Id = lst.LeaveId;
                    }

                    _lg_.Name = lst.LeaveGroupName;
                    if( lst.IsActive.Trim().ToUpper() == "YES" )
                        _lg_.IsActive = true;
                    else if( lst.IsActive.Trim().ToUpper() == "NO" )
                        _lg_.IsActive = false;

                    context.LeaveGroup.AddOrUpdate( _lg_ );
                    context.SaveChanges();

                    if( lst.LeaveGroupDetailsList.Count > 0 ) {
                        StringBuilder builder = new StringBuilder();
                        builder.Append("delete from leavegrouptxn where LeaveGroupId = @lgId");
                        context.Database.ExecuteSqlCommand(builder.ToString(), new SqlParameter("@lgId", _lg_.Id));
                        LeaveGroupTxn _lgt_ = null;
                        foreach( var l in lst.LeaveGroupDetailsList ) {
                            _lgt_ = new LeaveGroupTxn();
                            _lgt_.LeaveGroupId = _lg_.Id;
                            _lgt_.LeaveTypeId = l.LeaveTypeId;
                            _lgt_.LeaveCount = Convert.ToInt16( l.LeaveCount );
                            _lgt_.MaxSeqLeaves = 0;
                            _lgt_.IsActive = _lg_.IsActive;

                            context.LeaveGroupTxn.AddOrUpdate( _lgt_ );
                            context.SaveChanges();
                        }
                    }                    
                    Trans.Commit();
                }
                catch (Exception)
                {
                    Trans.Rollback();
                    throw;
                }
            }

        }

        public List<LeaveGroupDetailsList> GetLeaveGroupDetails(string id)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select LeaveTypeId , Convert( varchar , LeaveCount ) as LeaveCount from leavegrouptxn where " +
                "LeaveGroupId = @id");

            try
            {
                var lst =
                    context.Database.SqlQuery<LeaveGroupDetailsList>(qryStr.ToString(),new SqlParameter("@id", id))
                        .Select(d => new LeaveGroupDetailsList()
                        {
                            LeaveTypeId = d.LeaveTypeId,
                            LeaveCount = d.LeaveCount
                        }).ToList();

                if (lst == null)
                {
                    return new List<LeaveGroupDetailsList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LeaveGroupDetailsList>();
            }
        }

        public List<AccountableLeaveList> GetAccountableLeaves()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id , Name , ShortName from leavetype where IsAccountable = 1 and IsActive = 1");

            try
            {
                var lst = context.Database.SqlQuery<AccountableLeaveList>(qryStr.ToString())
                    .Select(d => new AccountableLeaveList()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        ShortName = d.ShortName
                    }).ToList();

                if (lst == null)
                {
                    return new List<AccountableLeaveList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<AccountableLeaveList>();
            }

        }

        public List<LeaveGroupList> LoadLeaveGroups()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append(
                "select Id , Name , case when ( IsActive = 1 ) then 'Yes' else 'No' end as IsActive from leavegroup");

            try
            {
                var lst = context.Database.SqlQuery<LeaveGroupList>(qryStr.ToString()).Select(d => new LeaveGroupList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<LeaveGroupList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<LeaveGroupList>();
            }
            //return null;
        }
        //self
        public List<LeaveGroupTxnListModel> GetTransactionList()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select a.Id, b.Name,c.Name as LeaveType, a.IsActive from LeaveGroupTxn a " +
                " inner join LeaveType c on c.Id = a.LeaveTypeId " +
                " inner join LeaveGroup b on b.Id = a.LeaveGroupId " +
                "where a.IsActive = 1");
            try
            {
                var lst = context.Database.SqlQuery<LeaveGroupTxnListModel>(qryStr.ToString())
                    .Select(d => new LeaveGroupTxnListModel()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        LeaveType = d.LeaveType,
                        IsActive = d.IsActive
                    }).ToList();

                if (lst == null)
                {
                    return null;
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }
        public LeaveGroupTxnListModel GetTransaction(int TxnId)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select a.Id,a.LeaveGroupId,a.LeaveTypeId, b.Name,c.Name as LeaveType,a.LeaveCount,a.MaxSeqLeaves,a.PaidLeave,a.Accountable,a.CarryForward, " +
                        "a.MaxAccDays,a.MaxAccYears, a.MaxDaysPerReq, a.MinDaysPerReq,a.ElgInMonths, a.IsCalcToWorkingDays, a.CalcToWorkingDays, " +
                        "a.ConsiderWO,a.ConsiderPH, a.IsExcessEligibleAllowed, a.IsEnCashmentAllowed, a.EncashmentLimit, a.CreditFreq, " +
                        "a.CreditDays,a.ProRata, a.RoundOffTo, a.RoundOffValue, a.IsActive " +
                        "from LeaveGroupTxn a inner join LeaveType c on c.Id = a.LeaveTypeId " +
                        "inner join LeaveGroup b on b.Id = a.LeaveGroupId where a.Id = @TxnId");
            try
            {
                var lst = context.Database.SqlQuery<LeaveGroupTxnListModel>(qryStr.ToString(), new SqlParameter("@TxnId", TxnId))
                    .Select(d => new LeaveGroupTxnListModel()
                    {
                        Id = d.Id,
                        LeaveGroupId = d.LeaveGroupId,
                        LeaveTypeId = d.LeaveTypeId,
                        Name = d.Name,
                        LeaveType = d.LeaveType,
                        LeaveCount = d.LeaveCount,
                        MaxSeqLeaves = d.MaxSeqLeaves,
                        PaidLeave = d.PaidLeave,
                        Accountable = d.Accountable,
                        CarryForward = d.CarryForward,
                        MaxAccDays = d.MaxAccDays,
                        MaxAccYears = d.MaxAccYears,
                        MaxDaysPerReq = d.MaxDaysPerReq,
                        MinDaysPerReq = d.MinDaysPerReq,
                        CheckBalance = d.CheckBalance,
                        ElgInMonths = d.ElgInMonths,
                        IsCalcToWorkingDays = d.IsCalcToWorkingDays,
                        CalcToWorkingDays = d.CalcToWorkingDays,
                        ConsiderWO = d.ConsiderWO,
                        ConsiderPH = d.ConsiderPH,
                        IsExcessEligibleAllowed = d.IsExcessEligibleAllowed,
                        IsEnCashmentAllowed = d.IsEnCashmentAllowed,
                        EncashmentLimit = d.EncashmentLimit,
                        ComponentId = d.ComponentId,
                        CreditFreq = d.CreditFreq,
                        CreditDays = d.CreditDays,
                        ProRata = d.ProRata,
                        LCAFor = d.LCAFor,
                        RoundOffTo = d.RoundOffTo,
                        RoundOffValue = d.RoundOffValue,
                        IsActive = d.IsActive
                        //CreatedOn = Convert.ToDateTime(d.CreatedOn),
                        //CreatedBy = d.CreatedBy
                    }).FirstOrDefault();

                if (lst == null)
                {
                    return null;
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception E)
            {
                throw E;
            }
        }


    }
}
