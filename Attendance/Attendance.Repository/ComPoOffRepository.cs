using Attendance.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class ComPoOffRepository : IDisposable
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
    
        public AttendanceManagementContext Context = null;
        string msg = "";
        public ComPoOffRepository()
        {
            Context = new AttendanceManagementContext();
        }
        public CompoOffModel GetEmpData(string StaffId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            try
            {
                QryStr.Append("SELECT DBO.fnGetStaffName(@StaffId) AS StaffName ," +
                    " DBO.fnGetMasterName(@StaffId,'DP') AS DEPARTMENT, DBO.fnGetMasterName" +
                    "(@StaffId,'CT') AS CATEGORY ");
                var data = Context.Database.SqlQuery<CompoOffModel>(QryStr.ToString(),new SqlParameter("@StaffId", StaffId)).Select(d => new CompoOffModel()
                {
                    StaffName = d.StaffName,
                    Department = d.Department,
                    Category = d.Category
                }).FirstOrDefault();
                if (data == null)
                {
                    return new CompoOffModel();
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
            catch (Exception e)
            {
                throw e;
                //return new IndividualLeaveCreditDebit_EmpDetails();
            }
        }
        public string SaveStaffDetails(CompoOffModel Model)
        {
            using (var trans = Context.Database.BeginTransaction())
            {
                try
                {
                    CommonRepository ObjComBL = new CommonRepository();
                    RequestApplication TBLReqApp = new RequestApplication();
                    /*Table A*/
                    TBLReqApp.StaffId = Model.StaffId;
                    TBLReqApp.LeaveTypeId = "LV0005";
                    TBLReqApp.LeaveEndDurationId = 1;
                    TBLReqApp.StartDate = Model.CompFrom;
                    TBLReqApp.EndDate = Model.CompFrom;
                    TBLReqApp.Id = Model.Id;
                    TBLReqApp.TotalDays = Model.LeaveCount;
                    TBLReqApp.RHId = 0;
                    TBLReqApp.Remarks = Model.Remark;
                    TBLReqApp.ContactNumber = Model.ContNo;
                    TBLReqApp.ApplicationDate = DateTime.Now;
                    TBLReqApp.AppliedBy = Model.AppliedBy;
                    TBLReqApp.IsCancelled = false;
                    TBLReqApp.IsApproved = false;
                    TBLReqApp.IsRejected = false;
                    TBLReqApp.RequestApplicationType = "CR";
                    TBLReqApp.IsCancelApprovalRequired = false;
                    TBLReqApp.IsCancelApproved = false;
                    TBLReqApp.IsCancelRejected = false;
                    TBLReqApp.WorkedDate = Model.CompFrom;
                    string ExpDate = ObjComBL.GetCompOffExpDateValue();
                    TBLReqApp.ExpiryDate = Convert.ToDateTime(Model.CompFrom).AddDays(Convert.ToInt16(ExpDate));
                    /*Table B*/
                    EmployeeLeaveAccount TBLEmpLevApp = new EmployeeLeaveAccount();
                    TBLEmpLevApp.StaffId = Model.StaffId;
                    TBLEmpLevApp.LeaveTypeId = "LV0005";
                    TBLEmpLevApp.TransactionFlag = 1;
                    TBLEmpLevApp.TransactionDate = DateTime.Now;
                    TBLEmpLevApp.LeaveCount = Model.LeaveCount;
                    TBLEmpLevApp.Narration = "EMPLOYEECOMPOFF-APPLICATIONCREDITBY";
                    TBLEmpLevApp.RefId = Model.Id;
                    TBLEmpLevApp.LeaveCreditDebitReasonId = 28;
                    TBLEmpLevApp.IsSystemAction = false;
                    TBLEmpLevApp.TransctionBy = Model.StaffId;
                    /*Table C*/
                    RACoffRequestApplicationRepository BL = new RACoffRequestApplicationRepository();
                    ApplicationApproval AA = new ApplicationApproval();
                    AA.Id = BL.GetUniqueId();
                    AA.ParentId = Model.Id;
                    AA.ApprovalStatusId = 2;
                    AA.ApprovedBy = Model.AppliedBy;
                    AA.ApprovedOn = DateTime.Now;
                    AA.Approval2statusId = 2;
                    AA.Approval2By = Model.AppliedBy;
                    AA.Approval2On = DateTime.Now;
                    AA.Comment = "APPROVED MANUAL COMP-OFF REQUEST.";
                    AA.ApprovalOwner = Model.AppliedBy;
                    AA.ParentType = "CR";
                    AA.ForwardCounter = 1;
                    AA.ApplicationDate = DateTime.Now;

                    Context.RequestApplication.Add(TBLReqApp);
                    Context.EmployeeLeaveAccount.Add(TBLEmpLevApp);
                    Context.ApplicationApproval.Add(AA);
                    Context.SaveChanges();
                    trans.Commit();
                    msg = "OK";
                }
                catch (DbEntityValidationException e)
                {
                    string Message = string.Empty;
                    foreach (var error in e.EntityValidationErrors)
                    {
                        foreach (var er in error.ValidationErrors)
                        {
                            if (Message.ToString() != "")
                            {
                                Message = Message + "~" + er.ErrorMessage;
                            }
                            else
                            {
                                Message = er.ErrorMessage;
                            }
                        }
                    }
                    trans.Rollback();
                }
                catch (Exception e)
                {
                    msg = e.Message;
                    trans.Rollback();
                }
            }
            return msg;
        }
    }
}
