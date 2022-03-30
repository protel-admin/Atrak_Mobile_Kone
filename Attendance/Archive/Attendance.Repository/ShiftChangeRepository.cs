using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using Attendance.Model;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class ShiftChangeRepository : IDisposable
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

        public ShiftChangeRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<OldShift> GetOldShifts(string staffid, string fromdate, string todate)
        {
            SqlParameter[] sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@staffid", staffid);
            sqlParameter[1] = new SqlParameter("@fromdate", fromdate);
            sqlParameter[2] = new SqlParameter("@todate", todate);

            var qryStr = new StringBuilder();

            qryStr.Clear();

            qryStr.Append("select s.Id , s.ShortName , convert ( varchar , ad.ShiftInDate , 103 ) as ShiftInDate , " +
                            "convert ( varchar ( 5 ) , ad.ShiftInTime , 114 ) as ShiftInTime , convert ( varchar ( 5 ) , ad.ShiftOutTime , 114 ) as ShiftOutTime from attendancedata ad inner join shifts s on s.id = ad.shiftid " +
                            "where staffid = @staffid  and " +
                            "shiftindate between convert ( datetime , @fromdate ) and convert ( datetime , @todate) order by ad.ShiftInDate ASC");

            try
            {
                var lst = context.Database.SqlQuery<OldShift>(qryStr.ToString(),sqlParameter).Select(d => new OldShift()
                {
                    Id = d.Id,
                    ShortName = d.ShortName,
                    ShiftInDate = d.ShiftInDate,
                    ShiftInTime = d.ShiftInTime,
                    ShiftOutTime = d.ShiftOutTime
                }).ToList();

                if (lst == null)
                {
                    return new List<OldShift>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<OldShift>();
            }
        }

        public void SaveShiftChangeInformation(CustomAttendanceData cattdata)
        {
            var repo = new CommonRepository();
            var repo1 = new ShiftChangeRepository();
            double count = (cattdata.ToDate - cattdata.FromDate).TotalDays;
            string otCoffValidation = string.Empty;
            DateTime currentDate = DateTime.Now;
            DateTime defaultToDate = DateTime.Now.AddDays(-1);
            DateTime expectedWorkingHours = Convert.ToDateTime("1900-01-01 00:00:00.000");

            var dates = new List<DateTime>();
            for (var dt = cattdata.FromDate; dt <= cattdata.ToDate; dt = dt.AddDays(1))
            {
                otCoffValidation = repo.CheckIsOTorCompOffApproved(cattdata.StaffId, dt.ToString("dd-MMM-yyyy"));
                if(otCoffValidation != "OK")
                {
                    throw new Exception(otCoffValidation);
                }

                dates.Add(dt);
            }
           // int count1 = 0;
           // SqlConnection connectionobject = new SqlConnection(ConfigurationManager.ConnectionStrings["AttendanceManagementContext"].ToString());
           // string query = string.Empty;
           // System.Data.SqlClient.SqlCommand CMD = new System.Data.SqlClient.SqlCommand();
           // CMD.Connection = connectionobject;
           // connectionobject.Open();
           // CMD.CommandTimeout = 5;
           //SqlParameter Param1 = new SqlParameter();
           // SqlParameter Param2 = new SqlParameter();
           // SqlParameter Param3 = new SqlParameter();
           // query = " Select Top 1 Staffid from [ShiftChangeApplication] where StaffId = @P1 AND FromDate = @P2 AND ToDate = @P3";
           // CMD.CommandText = query;
           // Param1.ParameterName = "@P1";
           // Param1.SqlDbType = System.Data.SqlDbType.VarChar;
           // Param1.Direction = System.Data.ParameterDirection.Output;
           // Param1.Size = 20;
           // Param1.Value = cattdata.StaffId;
           // Param2.ParameterName = "@P2";
           // Param2.SqlDbType = System.Data.SqlDbType.VarChar;
           // Param2.Direction = System.Data.ParameterDirection.Output;
           // Param2.Size = 30;
           // Param2.Value = cattdata.FromDate;
           // Param3.ParameterName = "@P3";
           // Param3.SqlDbType = System.Data.SqlDbType.VarChar;
           // Param3.Direction = System.Data.ParameterDirection.Output;
           // Param3.Size = 30;
           // Param3.Value = cattdata.ToDate;
           // CMD.Parameters.Add(Param1);
           // CMD.Parameters.Add(Param2);
           // CMD.Parameters.Add(Param3);
           // CMD.Prepare();
           // count1 = (int) CMD.ExecuteScalar();

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string loggedInUserId = cattdata.LoggedInUserId;
                    var BaseAddress = string.Empty;
                    var ReportingManager = string.Empty;
                    var selfapproval = false;
                    var lastid = string.Empty;
                    ShiftChangeApplication sca = new ShiftChangeApplication();
                    sca.Id = cattdata.Id;
                    sca.FromDate = cattdata.FromDate;
                    sca.NewShiftId = cattdata.NewShiftId;
                    sca.Reason = cattdata.Reason;
                    sca.StaffId = cattdata.StaffId;
                    sca.ToDate = cattdata.ToDate;
                    sca.CreatedOn = DateTime.Now;
                    sca.CreatedBy = cattdata.LoggedInUserId;

                    if (string.IsNullOrEmpty(sca.Id) == true) //if not then...
                    {

                        if (string.IsNullOrEmpty(sca.Id) == true)
                        {
                            var mr = new MasterRepository();
                            lastid = mr.getmaxid("shiftchangeapplication", "id", "SC", "", 10, ref lastid);
                            sca.Id = lastid;
                        }

                        context.ShiftChangeApplication.AddOrUpdate(sca);
                        ReportingManager = repo.GetReportingManager(sca.StaffId);
                        //if (string.IsNullOrEmpty(ReportingManager) == true)
                        //{
                        //    ReportingManager = sca.StaffId;
                        //    selfapproval = true;
                        //}

                        repo.SaveIntoApplicationApproval(sca.Id, "SC", loggedInUserId, ReportingManager, selfapproval);

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(sca.Id) == true)
                        {
                            var mr = new MasterRepository();
                            lastid = mr.getmaxid("shiftchangeapplication", "id", "SC", "", 10, ref lastid);
                            sca.Id = lastid;
                        }

                        context.ShiftChangeApplication.AddOrUpdate(sca);
                    }


                    //##############################################################################################################
                    //CODE BLOCK TO SEND EMAIL INTIMATION TO THE REPORTING MANAGER AND AN ACKNOWLEDGEMENT TO THE SENDER WHO RAISED 
                    //  THE APPLICATION.
                    //##############################################################################################################

                    try
                    {
                        //try to get the server ip from the web.config file.
                        BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                        //check if the server ip address has been given or not.
                        if (string.IsNullOrEmpty(BaseAddress) == true) //if the server ip address has not been given then...
                            //throw exception.
                            throw new Exception("BaseAddress parameter is blank in web.config file.");
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    //get the emailid of the reporting manager.
                    var ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
                    //get the emailid of the staff who raises the leave application.
                    var StaffEmailId = repo.GetEmailIdOfEmployee(sca.StaffId);
                    //get the name of the staff.
                    var StaffName = repo.GetStaffName(sca.StaffId);
                    //get the name of the reporting manager.
                    var ReportingManagerName = repo.GetStaffName(ReportingManager);

                    var NewShiftName = repo.GetNewShiftName(sca.NewShiftId);

                    //check if the reporting manager has an email id.
                    if (string.IsNullOrEmpty(ReportingManagerEmailId) == true) //if the reporting manager does not have an email id then...
                    {
                        //check if the staff has an email id.
                        if (string.IsNullOrEmpty(StaffEmailId) == true) //if the staff does not have an email id then...
                        {
                            //do not take any action.
                        }
                        else //if the staff has an email id then...
                        {
                            var EmailStr = string.Empty;
                            EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Shift Change application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(sca.FromDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(sca.ToDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + sca.Reason + "</td></tr></table></p><p>But an intimation email could not be sent to your reporting manager because of a missing email id.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                            //send intimation to the staff stating that his/her Shift Change application has been acknowleged 
                            //function call to get the name of the staff and the reporting manager.
                            //  but the reporting manager does not have a email id so no intimation has been sent to him.
                            repo.SendEmailMessage("", StaffEmailId, "", "", "Shift Change application of " + sca.StaffId + " - " + StaffName, EmailStr);
                        }
                    }
                    else // if the reporting manager has an email id then...
                    {
                        var EmailStr = string.Empty;
                        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a Shift Change. Shift Change details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(sca.FromDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(sca.ToDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">New Shift:</td><td style=\"width:80%;\">" + NewShiftName + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + sca.Reason + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this application.</p><p><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + sca.Id + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + sca.Id + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        // send intimation to the reporting manager about the Shift Change application.
                        repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Shift Change application of " + StaffName, EmailStr);

                        // send acknowledgement to the staff who raised the leave application.
                        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Shift Change application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(sca.FromDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">To Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(sca.ToDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">New Shift:</td><td style=\"width:80%;\">" + NewShiftName + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + sca.Reason + "</td></tr></table></p><p>This application has been sent also to your reporting manager.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
                        repo.SendEmailMessage("", StaffEmailId, "", "", "Shift Change application sent to " + ReportingManagerName, EmailStr);
                    }
                    //##############################################################################################################

                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception err)
                {
                    trans.Rollback();
                    throw err;
                }
            }
        }

        /* METHOD END -----*/
        ////public void SaveShiftChange(ShiftChangeApplication sca, string loggedInUserId)
        ////{
        ////    var BaseAddress = string.Empty;
        ////    var ReportingManager = string.Empty;
        ////    var selfapproval = false;
        ////    var repo = new CommonRepository();

        ////    using (var trans = context.Database.BeginTransaction())
        ////    {
        ////        try
        ////        {
        ////            //check if the id was initially generated.
        ////            if (string.IsNullOrEmpty(sca.Id) == true) //if not then...
        ////            {
        ////                SaveShiftChangeDetails(sca);
        ////                ReportingManager = repo.GetReportingManager(sca.StaffId);
        ////                if (string.IsNullOrEmpty(ReportingManager) == true)
        ////                {
        ////                    ReportingManager = sca.StaffId;
        ////                    selfapproval = true;
        ////                }

        ////                repo.SaveIntoApplicationApproval(sca.Id, "SC", loggedInUserId, ReportingManager, selfapproval);

        ////            }
        ////            else
        ////            {
        ////                SaveShiftChangeDetails(sca);
        ////            }


        ////            //##############################################################################################################
        ////            //CODE BLOCK TO SEND EMAIL INTIMATION TO THE REPORTING MANAGER AND AN ACKNOWLEDGEMENT TO THE SENDER WHO RAISED 
        ////            //  THE APPLICATION.
        ////            //##############################################################################################################

        ////            try
        ////            {
        ////                //try to get the server ip from the web.config file.
        ////                BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
        ////                //check if the server ip address has been given or not.
        ////                if (string.IsNullOrEmpty(BaseAddress) == true) //if the server ip address has not been given then...
        ////                    //throw exception.
        ////                    throw new Exception("BaseAddress parameter is blank in web.config file.");
        ////            }
        ////            catch (Exception)
        ////            {
        ////                throw;
        ////            }

        ////            //get the emailid of the reporting manager.
        ////            var ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
        ////            //get the emailid of the staff who raises the leave application.
        ////            var StaffEmailId = repo.GetEmailIdOfEmployee(sca.StaffId);
        ////            //get the name of the staff.
        ////            var StaffName = repo.GetStaffName(sca.StaffId);
        ////            //get the name of the reporting manager.
        ////            var ReportingManagerName = repo.GetStaffName(ReportingManager);

        ////            var NewShiftName = repo.GetNewShiftName(sca.NewShiftId);

        ////            //check if the reporting manager has an email id.
        ////            if (string.IsNullOrEmpty(ReportingManagerEmailId) == true) //if the reporting manager does not have an email id then...
        ////            {
        ////                //check if the staff has an email id.
        ////                if (string.IsNullOrEmpty(StaffEmailId) == true) //if the staff does not have an email id then...
        ////                {
        ////                    //do not take any action.
        ////                }
        ////                else //if the staff has an email id then...
        ////                {
        ////                    var EmailStr = string.Empty;
        ////                    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Shift Change application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(sca.FromDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(sca.ToDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + sca.Reason + "</td></tr></table></p><p>But an intimation email could not be sent to your reporting manager because of a missing email id.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
        ////                    //send intimation to the staff stating that his/her Shift Change application has been acknowleged 
        ////                    //function call to get the name of the staff and the reporting manager.
        ////                    //  but the reporting manager does not have a email id so no intimation has been sent to him.
        ////                    repo.SendEmailMessage("", StaffEmailId, "", "", "Shift Change application of " + sca.StaffId + " - " + StaffName, EmailStr);
        ////                }
        ////            }
        ////            else // if the reporting manager has an email id then...
        ////            {
        ////                var EmailStr = string.Empty;
        ////                EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a Shift Change. Shift Change details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(sca.FromDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(sca.ToDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">New Shift:</td><td style=\"width:80%;\">" + NewShiftName + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + sca.Reason + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is needed to either approve or reject this application.</p><p><a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + sca.Id + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "LeaveApplicationWabco/ApproveRejectApplication?ApproverId=" + ReportingManager + "&ApplicationApprovalId=" + sca.Id + "&Approve=false\">Reject</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
        ////                // send intimation to the reporting manager about the Shift Change application.
        ////                repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Shift Change application of " + StaffName, EmailStr);

        ////                // send acknowledgement to the staff who raised the leave application.
        ////                EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Shift Change application has been acknowledged.<table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;\">Name:</td><td style=\"width:80%;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(sca.FromDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">From Date:</td><td style=\"width:80%;\">" + Convert.ToDateTime(sca.ToDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;\">New Shift:</td><td style=\"width:80%;\">" + NewShiftName + "</td></tr><tr><td style=\"width:20%;\">Reason:</td><td style=\"width:80%;\">" + sca.Reason + "</td></tr></table></p><p>This application has been sent also to your reporting manager.</p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p></body></html>";
        ////                repo.SendEmailMessage("", StaffEmailId, "", "", "Shift Change application sent to " + ReportingManagerName, EmailStr);
        ////            }
        ////            //##############################################################################################################

        ////            trans.Commit();
        ////        }
        ////        catch (Exception)
        ////        {
        ////            trans.Rollback();
        ////            throw;
        ////        }
        ////    }
        ////}


        ////public void SaveShiftChangeDetails(ShiftChangeApplication sca)
        ////{
        ////    var lastid = string.Empty;
        ////    if (string.IsNullOrEmpty(sca.Id) == true)
        ////    {
        ////        var mr = new MasterRepository();
        ////        lastid = mr.getmaxid("shiftchangeapplication", "id", "SC", "", 10, ref lastid);
        ////        sca.Id = lastid;
        ////    }

        ////    context.ShiftChangeApplication.AddOrUpdate(sca);
        ////    context.SaveChanges();
        ////}

        public List<OldShift> GetShiftList()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select 'LV0011'  as id , 'WO' as ShortName,'WEEKLY OFF' as Name , '00:00' as ShiftInTime , '00:00' as ShiftOutTime" +
                           " union  Select Id , ShortName+'-'+Name as ShortName , convert ( varchar ( 5 ) , StartTime , 114 ) ShiftInTime ," +
                            "convert ( varchar ( 5 ) , EndTime , 114 ) ShiftOutTime , Name from SHIFTS WHERE [IsActive]=1  order by Name asc");

            try
            {
                var lst = context.Database.SqlQuery<OldShift>(qryStr.ToString()).Select(d => new OldShift()
                {
                    Id = d.Id,
                    ShortName = d.ShortName,
                    ShiftInDate = d.ShiftInDate,
                    ShiftInTime = d.ShiftInTime,
                    ShiftOutTime = d.ShiftOutTime
                }).ToList();

                if (lst == null)
                {
                    return new List<OldShift>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<OldShift>();
            }
        }

        public List<ShiftChangeList> GetShiftChangeList(string StaffId, string ShiftChangeAccess)
        {
        
            

            var qryStr = new StringBuilder();
            qryStr.Clear();

            //if (ShiftChangeAccess == "1")
            //{
            //    qryStr.Append("select Id as Id , StaffId as StaffId , FirstName as StaffName , FromDate as DateFrom ,ToDate as DateTo , " +
            //        "NewShiftId as NewShiftId , ShiftName as NewShiftName , DeptName as DepartmentName ,Reason as Reason , " +
            //        "StatusId as StatusId ,Status as StatusName from vwShiftChangeList  ORDER BY ID DESC ");
            //}
            //else if (ShiftChangeAccess == "6")
            //{
            //    StaffListRepository shiftRepo = new StaffListRepository();
            //    var getstafflist = shiftRepo.GetStaffsByReportingManager(StaffId);
            //    qryStr.Append("select Id as Id , StaffId as StaffId , FirstName as StaffName , FromDate as DateFrom ,ToDate as DateTo , " +
            //       "NewShiftId as NewShiftId , ShiftName as NewShiftName , DeptName as DepartmentName ,Reason as Reason , " +
            //       "StatusId as StatusId ,Status as StatusName from vwShiftChangeList where StaffId in ('" + getstafflist.ToList() + "') ORDER BY ID DESC ");
            //}
            //else
            //{

            qryStr.Append("select Id as Id , StaffId as StaffId , FirstName as StaffName , FromDate as DateFrom ,ToDate as DateTo , " +
                "NewShiftId as NewShiftId , ShiftName as NewShiftName , DeptName as DepartmentName ,Reason as Reason , " +
                "StatusId as StatusId ,Status as StatusName from vwShiftChangeList  where StaffId =   @StaffId  ORDER BY ID DESC ");
            //}

            //if (StaffId == "-")
            //{
            //    qryStr.Append("select TOP 50 Id as Id , StaffId as StaffId , FirstName as StaffName , FromDate as DateFrom , " +
            //                    "ToDate as DateTo , NewShiftId as NewShiftId , ShiftName as NewShiftName , DeptName as DepartmentName , " +
            //                    "Reason as Reason , StatusId as StatusId , Status as StatusName from vwShiftChangeList ORDER BY ID DESC ");
            //}
            //else
            //{
            //    qryStr.Append("select TOP 50 Id as Id , StaffId as StaffId , FirstName as StaffName , FromDate as DateFrom , " +
            //                    "ToDate as DateTo , NewShiftId as NewShiftId , ShiftName as NewShiftName , DeptName as DepartmentName , " +
            //                    "Reason as Reason , StatusId as StatusId , Status as StatusName from vwShiftChangeList where staffid = '" + StaffId + "'" +
            //                    "ORDER BY  ID DESC");
            //}

            try
            {
                var lst = context.Database.SqlQuery<ShiftChangeList>(qryStr.ToString(),new SqlParameter("@StaffId",StaffId)).Select(d => new ShiftChangeList()
                {
                    Id = d.Id,
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    DepartmentName = d.DepartmentName,
                    DateFrom = d.DateFrom,
                    DateTo = d.DateTo,
                    NewShiftId = d.NewShiftId,
                    NewShiftName = d.NewShiftName,
                    Reason = d.Reason,
                    StatusId = d.StatusId,
                    StatusName = d.StatusName
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftChangeList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ShiftChangeList>();
            }
        }

        public string ShiftUserRule(string Txtstaffid, string SesstaffId)
        {
            string Message = string.Empty;
            List<string> Staffs = new List<string>();

            StaffListRepository shiftRepo = new StaffListRepository();
            var getstafflist = shiftRepo.GetStaffsByReportingManager(SesstaffId);
            if (getstafflist.Count > 0)
            {
                foreach (var rec1 in getstafflist)
                {
                    Staffs.Add(rec1);
                }
            }
            string txtStaffId = Txtstaffid.ToUpper();
            if (Staffs.Contains(txtStaffId) == true)
            {
                Message = "Success";
            }
            else
            {
                Message = "fail";
            }
            return Message;
        }

        public List<WeeklyOffList> GetWeeklyoff()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select value as Id,name as Name from dbo.IsWeeklyOffList()");

            try
            {
                var lst = context.Database.SqlQuery<WeeklyOffList>(qryStr.ToString()).Select(d => new WeeklyOffList()
                {
                    Id = d.Id,
                    Name = d.Name

                }).ToList();

                if (lst == null)
                {
                    return new List<WeeklyOffList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<WeeklyOffList>();
            }
        }
    }
}

