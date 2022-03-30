using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using System.Security;
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

        public List<LeaveTypeMasterDetails> GetAllLeaveTypesFromMaster()
        {
            var qryStr = new StringBuilder();

            qryStr.Clear();
            qryStr.Append("Select Id , Name , ShortName , LeaveType , Case When PaidLeave = 1 then 'Yes' Else 'No' End as PaidLeave , Case When Accountable = 1 then 'Yes'" +
                " Else 'No' End as Accountable  , Case When CarryForward = 1 then 'Yes' Else 'No' End as CarryForward , MaxAccDays , MaxAccYears ," +
                " MaxDaysPerReq ,  ElgInMonths , Case When IsCalcToWorkingDays = 1 then 'Yes' " +
                " Else 'No' End as IsCalcToWorkingDays , CalcToWorkingDays , Case When ConsiderWO = 1 then 'Yes' Else 'No' End as ConsiderWO ," +
                " Case When ConsiderPH = 1 then 'Yes' Else 'No' End as ConsiderPH , Case When IsExcessEligibleAllowed = 1 then 'Yes' Else 'No' End as IsExcessEligibleAllowed ," +
                " Case When IsEncashmentAllowed = 1 then 'Yes' Else 'No' End as IsEncashmentAllowed , EnCashmentLimit , CreditFreq , CreditDays ," +
                " Case When ProRata = 1 then 'Yes' Else 'No' End as ProRata ,  RoundOffTo , RoundOffValue , Case When IsActive = 1 then 'Yes' Else 'No' End as IsActive , CreatedOn , CreatedBy " +
                " from LeaveTypeMaster where IsActive = 1 AND Accountable = 1 Order By Name ASC");

            var lstLV = context.Database.SqlQuery<LeaveTypeMasterDetails>(qryStr.ToString()).Select(d => new LeaveTypeMasterDetails()
            {
                Id = d.Id,
                Name = d.Name,
                ShortName = d.ShortName,
                LeaveType = d.LeaveType,
                MaxAccDays = d.MaxAccDays,
                MaxAccYears = d.MaxAccYears,
                MaxDaysPerReq = d.MaxDaysPerReq,
                ElgInMonths = d.ElgInMonths,
                IsCalcToWorkingDays = d.IsCalcToWorkingDays,
                CalcToWorkingDays = d.CalcToWorkingDays,
                ConsiderWO = d.ConsiderWO,
                ConsiderPH = d.ConsiderPH,
                IsExcessEligibleAllowed = d.IsExcessEligibleAllowed,
                IsEnCashmentAllowed = d.IsEnCashmentAllowed,
                EncashmentLimit = d.EncashmentLimit,
                CreditFreq = d.CreditFreq,
                CreditDays = d.CreditDays,
                ProRata = d.ProRata,
                RoundOffTo = d.RoundOffTo,
                RoundOffValue = d.RoundOffValue,
                CarryForward = d.CarryForward,
                PaidLeave = d.PaidLeave,
                Accountable = d.Accountable,
                IsActive = d.IsActive,
                CreatedOn = d.CreatedOn,
                CreatedBy = d.CreatedBy
            }).ToList();

            if (lstLV.Count == 0)
            {
                return new List<LeaveTypeMasterDetails>();
            }
            else
            {
                return lstLV;
            }
        }
    }
}
