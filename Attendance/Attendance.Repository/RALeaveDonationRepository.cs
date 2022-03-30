using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class RALeaveDonationRepository : IDisposable
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

        public RALeaveDonationRepository()
        {
            context = new AttendanceManagementContext();
        }
        public List<LeaveDonationDetails> GetLeaveDonationHistory(string StaffId)
        {
            try
            {
                var Str1 = new StringBuilder();
                Str1.Clear();
                Str1.Append("Select RA.Id as ApplicationId,RA.StaffId as DonarStaffId,DBO.fnGetStaffName(RA.StaffId) as DonarStaffName,ReceiverStaffId," +
                    " DBO.fnGetStaffName(ReceiverStaffId) as ReceiverStaffName, TotalDays as LeaveCount,Case when " +
                    "AA.ApprovalStatusId = 1 then 'PENDING' when AA.ApprovalStatusId = 2 then 'APPROVED' else 'REJECTED' END as " +
                    "Status1 from RequestApplication RA inner join ApplicationApproval AA on RA.Id = AA.ParentId Where RA.StaffId = @StaffId");

                var Obj = context.Database.SqlQuery<LeaveDonationDetails>(Str1.ToString(), new SqlParameter("@StaffId", StaffId)).ToList();
                return Obj;
            }
            catch
            {
                return new List<LeaveDonationDetails>();
            }
        }

        public IndividualLeaveCreditDebit_EmpDetails GetEmployeeDetails(string StaffId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("SELECT DBO.fnGetStaffName('" + StaffId + "') AS StaffName , DBO.fnGetMasterName('" + StaffId + "','DP')" +
          "  AS DEPARTMENT , (SELECT CONVERT ( VARCHAR , LEAVEBALANCE ) FROM LEAVEBALANCE WHERE STAFFID = '" + StaffId + "' AND " +
            "    LEAVETYPEID = 'LV0003') AS CASUALLEAVEBALANCE , (SELECT CONVERT ( VARCHAR , LEAVEBALANCE ) FROM LEAVEBALANCE " +
           " WHERE STAFFID = '" + StaffId + "' AND LEAVETYPEID = 'LV0004') AS SICKLEAVEBALANCE , (SELECT CONVERT ( VARCHAR , LEAVEBALANCE ) " +
          " FROM LEAVEBALANCE  WHERE STAFFID = '" + StaffId + "' AND LEAVETYPEID = 'LV0032') AS BEREAVEMENTLEAVEBALANCE ,(SELECT CONVERT " +
          " ( VARCHAR , LEAVEBALANCE )  FROM LEAVEBALANCE  WHERE STAFFID = '" + StaffId + "' AND LEAVETYPEID = 'LV0002') AS PRIVILEGELEAVEBALANCE , " +
              " (SELECT CONVERT ( VARCHAR , LEAVEBALANCE )  FROM LEAVEBALANCE  WHERE STAFFID = '" + StaffId + "' AND LEAVETYPEID = 'LV0005') AS MATERNITYLEAVEBALANCE ," +
              " (SELECT CONVERT ( VARCHAR , LEAVEBALANCE )  FROM LEAVEBALANCE  WHERE STAFFID = '" + StaffId + "' AND LEAVETYPEID = 'LV0010') AS CHILDADOPTIONLEAVEBALANCE ," +
            " (SELECT CONVERT ( VARCHAR , LEAVEBALANCE ) FROM LEAVEBALANCE WHERE STAFFID = '" + StaffId + "' AND LEAVETYPEID = 'LV0007') AS PATERNITYLEAVEBALANCE ," +
           "(SELECT CONVERT ( VARCHAR , LEAVEBALANCE ) FROM LEAVEBALANCE WHERE STAFFID = '" + StaffId + "' AND LEAVETYPEID = 'LV0035') AS NONCONFIRMEDLEAVEBALANCE");

            try
            {
                var data = context.Database.SqlQuery<IndividualLeaveCreditDebit_EmpDetails>(QryStr.ToString()).Select(d => new IndividualLeaveCreditDebit_EmpDetails()
                {
                    StaffName = d.StaffName,
                    Department = d.Department,
                    CasualLeaveBalance = d.CasualLeaveBalance,
                    SICKLEAVEBALANCE = d.SICKLEAVEBALANCE,
                    BEREAVEMENTLEAVEBALANCE = d.BEREAVEMENTLEAVEBALANCE,
                    // PRIVILEGELEAVEBALANCE = d.PRIVILEGELEAVEBALANCE,
                    MATERNITYLEAVEBALANCE = d.MATERNITYLEAVEBALANCE,
                    PATERNITYLEAVEBALANCE = d.PATERNITYLEAVEBALANCE,
                    //CHILDADOPTIONLEAVEBALANCE = d.CHILDADOPTIONLEAVEBALANCE,

                }).FirstOrDefault();

                if (data == null)
                {
                    return new IndividualLeaveCreditDebit_EmpDetails();
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

        public void ApproveApplication(ClassesToSave CTS, string ReportingManagerId, string ParentType)
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            CommonRepository CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(CTS.RA).Property("IsApproved").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;

                    //Insert into employee leave account.
                    if (CTS.employeeLeaveAccounts != null)
                    {
                        foreach (var rec in CTS.employeeLeaveAccounts)
                        {
                            context.EmployeeLeaveAccount.Add(rec);
                        }
                    }
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            //context.EmailSendLog.Add(l);
                            if (l.To != "-" && l.To != "")
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                    context.SaveChanges();
                    Trans.Commit();
                }
                catch (Exception e)
                {
                    Trans.Rollback();
                    throw e;
                    //new Exception("The Data is not proper or some other problem with network. Please try again later");
                }
            }
        }

        public string GetUniqueId()
        {
            return context.Database.SqlQuery<string>("select convert(varchar,getdate(),112) + replace(convert(varchar,getdate(),114),':','')").First();
        }

        public void SaveLeaveDonationDetails(IndividualLeaveCreditDebit data, string User, DocumentUpload doc, ClassesToSave CTS)
        {
            var ReportingManager = string.Empty;
            var repo = new CommonRepository();
            string BaseAddress = string.Empty;
            string AppName = string.Empty;
            string leaveDonationApprover = string.Empty;
            CommonRepository CR = new CommonRepository();
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    context.RequestApplication.Add(CTS.RA);
                    context.ApplicationApproval.Add(CTS.AA);
                    if (doc.FileContent != null && doc.FileContent.Length > 0)
                    {
                        try
                        {
                            context.DocumentUpload.Add(doc);
                        }
                        catch (Exception err)
                        {
                            throw err;
                        }
                    }
                    context.SaveChanges();
                    trans.Commit();
                    if (CTS.ESL != null)
                    {
                        foreach (var l in CTS.ESL)
                            if (l.To != "-" && string.IsNullOrEmpty(l.To).Equals(false))
                                CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    }
                }
                catch (Exception err)
                {
                    trans.Rollback();
                    throw new Exception(err.Message);
                }
            }
        }
        public void RejectApplication(ClassesToSave CTS)
        {
            var CR = new CommonRepository();
            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    //Update the request application table.
                    context.Entry(CTS.RA).Property("IsRejected").IsModified = true;
                    //Update the application approval table.
                    context.Entry(CTS.AA).Property("ApprovalStatusId").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedBy").IsModified = true;
                    context.Entry(CTS.AA).Property("ApprovedOn").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2statusId").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2By").IsModified = true;
                    context.Entry(CTS.AA).Property("Approval2On").IsModified = true;
                    context.Entry(CTS.AA).Property("Comment").IsModified = true;
                    foreach (var l in CTS.ESL)
                        //context.EmailSendLog.Add(l);
                        if (l.To != "-" && l.To != "")
                            CR.SendEmailMessageForApplication(l.From, l.To, l.CC, l.BCC, l.EmailSubject, l.EmailBody, l.CreatedBy);
                    //
                    context.SaveChanges();
                    Trans.Commit();
                }
                catch
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }
        public ApplicationApproval GetApplicationApproval(string ParentId, string ParentType)
        {
            return context.ApplicationApproval.Where(d => d.ParentId.Equals(ParentId)).FirstOrDefault();
        }
        public RequestApplication GetRequestApplicationDetails(string Id)
        {
            return context.RequestApplication.Where(d => d.Id.Equals(Id)).FirstOrDefault();
        }
    }
}
