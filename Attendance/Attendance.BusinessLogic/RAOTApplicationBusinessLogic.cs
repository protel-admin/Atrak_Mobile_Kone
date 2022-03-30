using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using System.Web.Mvc;

namespace Attendance.BusinessLogic
{
    public class RAOTApplicationBusinessLogic
    {
        public List<RAOTRequestApplication> GetAppliedPermissions(string StaffId)
        {
            using (RAOTApplicationRepository repo = new RAOTApplicationRepository())
            { 
                var Obj = repo.GetAppliedPermissions(StaffId);
            return Obj;
            }
        }

        public List<PermissionType> GetPermissionTypes()
        {
            using (RAOTApplicationRepository repo = new RAOTApplicationRepository())
            { 
                var Obj = repo.GetPermissionTypes();
            return Obj;
            }
        }

        public List<SelectListItem> ConvertPermissionTypesToListItems(List<PermissionType> objListOfLeaveTypes)
        {
            List<SelectListItem> _ListOfLeaveTypes_ = new List<SelectListItem>();
            foreach (var l in objListOfLeaveTypes)
            {
                _ListOfLeaveTypes_.Add(new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                });
            }

            return _ListOfLeaveTypes_;
        }

        public string GetUniqueId()
        {
            using (var repo = new RAOTApplicationRepository())
                return repo.GetUniqueId();
        }

        public void SaveRequestApplication(ClassesToSave DataToSave)
        {
            using (RAOTApplicationRepository repo = new RAOTApplicationRepository())
                repo.SaveRequestApplication(DataToSave);
        }

        public void RejectApplication(string Id, string ReportingManagerId)
        {
            //Get the leave application details based on the id passed to this function as a parameter.
            using (RAOTApplicationRepository repo = new RAOTApplicationRepository())
            { 
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);
            
            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true))    //if the leave application has been cancelled then...
            {
                //throw exception stating that the cancelled leave application cannot be rejected.
                throw new Exception("Cancelled permission request cannot be rejected.");
            }
            else if (Obj.IsApproved.Equals(true)) //if the leave application has been approved then...
            {
                //throw exception stating that the approved leave application cannot be rejected.
                throw new Exception("Approved permission request cannot be rejected.");
            }
            else if (Obj.IsRejected.Equals(true))  //if the leave application has been rejected then...
            {
                //throw exception stating that the rejected leave application cannot be rejected.
                throw new Exception("Rejected permission request cannot be rejected.");
            }
            else //if the leave application has neither been cancelled, approved or rejected ( i.e. it is in pending state.)
            {
                //reject the application.
                Obj.IsRejected = true;

                AA.ApprovalStatusId = 3;
                AA.ApprovedBy = ReportingManagerId;
                AA.ApprovedOn = DateTime.Now;
                AA.Comment = "PERMISSION REQUEST HAS BEEN REJECTED BY THE REPORTING MANAGER.";

                ClassesToSave CTS = new ClassesToSave();
                CTS.RA = Obj;
                CTS.AA = AA;
                repo.RejectApplication(CTS);
                //send rejected mail to the applicant.
            }
            }
        }

        public void ApproveApplication(string Id, string ReportingManagerId)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            using (RAOTApplicationRepository repo = new RAOTApplicationRepository())
            {
                var Obj = repo.GetRequestApplicationDetails(Id);
            var AA = repo.GetApplicationApproval(Id);

            //Check if the leave application has been cancelled or not.
            if (Obj.IsCancelled.Equals(true)) //if the leave application has been cancelled then...
            {
                //throw exception that a cancelled leave application cannot be approved.
                throw new Exception("Cannot approve a cancelled permission application. Apply for a new leave.");
            }
            else if (Obj.IsApproved.Equals(true)) //if application has already been approved then...
            {
                //throw exception stating that an already approved application cannot be reapproved.
                throw new Exception("Cannot approve already approved permission request.");
            }
            else if (Obj.IsRejected.Equals(true))
            {
                //throw exception stating that an already rejected application cannot be approved.
                throw new Exception("Cannot approve already rejected permission request.");
            }
            else
            {
                ////Get the leave balance based on the employee and the leave type.
                ////var LeaveBalance = repo.GetLeaveBalance(Obj.StaffId, Obj.LeaveTypeId);
                ////Check if the leave balance is more than the total days of leave requested.
                //if (LeaveBalance >= Convert.ToDecimal(Obj.TotalDays)) //if the leave balance is more then...
                //{
                //approve the application.
                Obj.IsApproved = true;

                //update the reporting manager in application Approval.
                //
                //
                AA.ApprovalStatusId = 2;
                AA.ApprovedBy = ReportingManagerId;
                AA.ApprovedOn = DateTime.Now;
                AA.Comment = "APPROVED THE PERMISSION REQUEST.";

                //deduct leave balance from employee leave account table.
                //EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
                //ELA.StaffId = Obj.StaffId;
                //ELA.LeaveTypeId = Obj.LeaveTypeId;
                //ELA.TransactionFlag = 2;
                //ELA.TransactionDate = DateTime.Now;
                //ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays) * -1;
                //ELA.Narration = "Approved the leave application.";
                //ELA.RefId = Obj.Id;

                //send confirmation email to the applicant.
                //
                //

                ClassesToSave CTS = new ClassesToSave();
                CTS.RA = Obj;
                //CTS.ELA = ELA;
                CTS.AA = AA;
                repo.ApproveApplication(CTS);

                //Get the leave balance once again based on the employee and the leave type.
                //var PostApprovalLeaveBalance = repo.GetLeaveBalance(Obj.StaffId, Obj.LeaveTypeId);
                //check if the leave balance is less than 0 or not.
                //if (LeaveBalance < 0)//if the leave balance is less than 0 then...
                //{
                //    //Reject the application
                //    Obj.IsApproved = false;
                //    Obj.IsRejected = true;

                //    AA.ApprovalStatusId = 3;
                //    AA.ApprovedBy = ReportingManagerId;
                //    AA.ApprovedOn = DateTime.Now;
                //    AA.Comment = "LEAVE REQUEST REJECTED DUE TO INSUFFICIENT LEAVE BALANCE.";

                //    //recredit back the total days debited.
                //    ELA.StaffId = Obj.StaffId;
                //    ELA.LeaveTypeId = Obj.LeaveTypeId;
                //    ELA.TransactionFlag = 1;
                //    ELA.TransactionDate = DateTime.Now;
                //    ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays);
                //    ELA.Narration = "APPROVAL REVERSED DUE TO INSUFFICIENT BALANCE.";
                //    ELA.RefId = Obj.Id;

                //    CTS.RA = Obj;
                //    CTS.ELA = ELA;
                //    CTS.AA = AA;
                //    repo.RejectApplication(CTS);

                //    //send sorry email to the applicant.
                //    //
                //    //
                //}
                //}
                //else
                //{
                //    throw new Exception("The applicant does not meet the defined time limit to approve the permission request.");
                //}
            }
            }
        }

        public void CancelApplication(string Id)
        {
            //Get the leave application details based on the Id passed to this function as a parameter.
            using (RAOTApplicationRepository repo = new RAOTApplicationRepository())
            {
                ClassesToSave CTS = new ClassesToSave();
            var Obj = repo.GetRequestApplicationDetails(Id);
            //
            //Check whether the starting date of the leave application is below the current date.
            var IsFutureDate = IsFromDateMoreOrEqualToCurrerntDate(Obj.StartDate, DateTime.Now);
            //
            //If the leave application date is future to the current date.
            if (IsFutureDate == true)
            {
                //Check if the leave application has been approved or not.
                if (Obj.IsApproved.Equals(false))    //If the leave application has not been approved. (i.e. in the pending state) then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in pending state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                        CTS.RA = Obj;
                        repo.CancelApplication(CTS);
                    }
                    else   //If the leave application has already been cancelled then...
                    {
                        //throw exception (first of all the cancel link must not be visible.)
                        throw new Exception("You cannot cancel a permission request that is already been cancelled.");
                    }
                }
                else//If the leave application has been approved then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in approved state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;

                        //Credit back the leave balance that was deducted.
                        //EmployeeLeaveAccount ELA = new EmployeeLeaveAccount();
                        //ELA.StaffId = Obj.StaffId;
                        //ELA.LeaveTypeId = Obj.LeaveTypeId;
                        //ELA.TransactionFlag = 1;
                        //ELA.TransactionDate = DateTime.Now;
                        //ELA.LeaveCount = Convert.ToDecimal(Obj.TotalDays);
                        //ELA.Narration = "Cancelled the pending leave application.";
                        //ELA.RefId = Obj.Id;
                        //
                        CTS.RA = Obj;
                        //CTS.ELA = ELA;
                        repo.CancelApplication(CTS);
                    }
                    else //If the leave application has already been cancelled then...
                    {
                        //throw exception.
                        throw new Exception("You cannot cancel a permission request that is already been cancelled.");
                    }
                }
            }
            else  //If the leave application is a past date then...
            {
                //Check if the leave application has been approved or not.
                if (Obj.IsApproved.Equals(false))    //If the leave application has not been approved. (i.e. in the pending state) then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in pending state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;

                        CTS.RA = Obj;
                        repo.CancelApplication(CTS);
                    }
                    else //If the leave application has already been cancelled then...
                    {
                        //throw exception (first of all the cancel link must not be visible for the application that has already been cancelled.)
                        throw new Exception("You cannot cancel a permission request that is already been cancelled.");
                    }
                }
                else  //If the leave application has been approved then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))//If the leave application has not been cancelled then...
                    {
                        //throw exception informing the user that the leave application has to be cancelled by his supervisor.
                        throw new Exception("You cannot cancel past permission request that have already been approved. It has to be cancelled by your supervisor.");
                    }
                    else  //If the leave application has already been cancelled then...
                    {
                        //throw exception.
                        throw new Exception("You cannot cancel a permission request that is already been cancelled.");
                    }
                }
            }
            }
        }

        public string CancelApprovedApplication(string Id)
        {
            string staffId = string.Empty;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            string applicationId = string.Empty;
            string applicationType = string.Empty;
            string punchtype = string.Empty;
            //Get the leave application details based on the Id passed to this function as a parameter.
            using (RAOTApplicationRepository repo = new RAOTApplicationRepository())
            { 
                ClassesToSave CTS = new ClassesToSave();
            var Obj = repo.GetRequestApplicationDetails(Id);
            //
            //Check whether the starting date of the leave application is below the current date.
            var IsFutureDate = IsFromDateMoreOrEqualToCurrerntDate(Obj.StartDate, DateTime.Now);
            //
            //If the leave application date is future to the current date.
            if (IsFutureDate == true)
            {
                //Check if the leave application has been approved or not.
                if (Obj.IsApproved.Equals(false))    //If the leave application has not been approved. (i.e. in the pending state) then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in pending state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;
                        CTS.RA = Obj;
                        repo.CancelApplication(CTS);
                    }
                    else   //If the leave application has already been cancelled then...
                    {
                        //throw exception (first of all the cancel link must not be visible.)
                        throw new Exception("You cannot cancel a permission request that is already been cancelled.");
                    }
                }
                else//If the leave application has been approved then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in approved state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;

                        CTS.RA = Obj;
                        repo.CancelApplication(CTS);
                    }
                    else //If the leave application has already been cancelled then...
                    {
                        //throw exception.
                        throw new Exception("You cannot cancel a permission request that is already been cancelled.");
                    }
                }
            }
            else  //If the leave application is a past date then...
            {
                //Check if the leave application has been approved or not.
                if (Obj.IsApproved.Equals(true))    //If the leave application has not been approved. (i.e. in the pending state) then...
                {
                    //Check if the leave application has already been cancelled or not.
                    if (Obj.IsCancelled.Equals(false))   //If the leave application has not been cancelled then...
                    {
                        //Cancel the leave application which is in pending state.
                        Obj.IsCancelled = true;
                        Obj.CancelledDate = DateTime.Now;

                        CTS.RA = Obj;
                        repo.CancelApplication(CTS);

                        CommonRepository obj = new CommonRepository();

                        try
                        {
                            var data = obj.GetList(Obj.Id);
                            staffId = data.StaffId;
                            fromDate = data.FromDate;
                            toDate = data.ToDate;
                            applicationType = Obj.RequestApplicationType;
                            applicationId = Obj.Id;
                        }
                        catch (Exception err)
                        {
                            throw err;
                        }
                        if (fromDate.Date < currentDate.Date)
                        {
                            if (toDate.Date >= currentDate.Date)
                            {
                                toDate = DateTime.Now.AddDays(-1);
                            }
                            obj.LogIntoIntoAttendanceControlTable(staffId, fromDate.Date, toDate.Date, applicationType, applicationId);
                        }
                    }
                    else //If the leave application has already been cancelled then...
                    {
                        //throw exception (first of all the cancel link must not be visible for the application that has already been cancelled.)
                        throw new Exception("You cannot cancel a permission request that is already been cancelled.");
                    }
                }
            }

            return "OK";
            }

        }


        private bool IsFromDateMoreOrEqualToCurrerntDate(DateTime? LeaveStartDate, DateTime? CurrentDate)
        {
            //TimeSpan TS1 = new TimeSpan();
            //TS1 = LeaveStartDate;


            if (LeaveStartDate.Value.Date < CurrentDate.Value.Date)
            {
                return false;
            }
            else if (LeaveStartDate.Value.Date >= CurrentDate.Value.Date)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
