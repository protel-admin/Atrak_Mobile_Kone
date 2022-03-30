using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Migrations;
using Attendance.Model;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
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
            SqlParameter[] Param = new SqlParameter[3];
            Param[0] = new SqlParameter("@staffid", staffid);
            Param[1] = new SqlParameter("@fromdate", fromdate);
            Param[2] = new SqlParameter("@todate", todate);
            var qryStr = new StringBuilder();

            qryStr.Clear();

            qryStr.Append("select s.Id , s.ShortName as ShortName, convert ( varchar , ad.ShiftInDate , 103 ) as ShiftInDate , " +
                            "convert ( varchar ( 5 ) , ad.ShiftInTime , 114 ) as ShiftInTime , convert ( varchar ( 5 ) , ad.ShiftOutTime , 114 ) as ShiftOutTime from attendancedata ad inner join ForTemporaryShiftChangeGrid s on s.id = ad.shiftid " +
                            "where staffid = @staffid and " +
                            "shiftindate between convert ( datetime , @fromdate ) and convert ( datetime , @todate) order by ad.ShiftInDate ASC");

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
            catch 
            {
                return new List<OldShift>();
            }
        }

        public void SaveShiftChangeInformation(CustomAttendanceData cattdata, string loggedInUserRole)
        {
          SqlParameter[] Param = new SqlParameter[2];
            Param[0] = new SqlParameter("@cattdata", cattdata);
            Param[1] = new SqlParameter("@loggedInUserRole", loggedInUserRole);
            var repo = new CommonRepository();
            var repo1 = new ShiftChangeRepository();
            double count = (cattdata.ToDate - cattdata.FromDate).TotalDays;
            DateTime currentDate = DateTime.Now;
            DateTime defaultToDate = DateTime.Now.AddDays(-1);
            DateTime expectedWorkingHours = Convert.ToDateTime("1900-01-01 00:00:00.000");

            var dates = new List<DateTime>();
            for (var dt = cattdata.FromDate; dt <= cattdata.ToDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    string loggedInUserId = cattdata.LoggedInUserId;

                    var BaseAddress = string.Empty;
                    var ReportingManager = string.Empty;
                    var selfapproval = false;
                    ReportingManager = repo.GetReportingManager(cattdata.StaffId);

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

                    //StringBuilder builder = new StringBuilder();
                    //builder.Append("select ReportingManager as Approver1,Approver2,ApproverLevel from StaffOfficial where StaffId=@StaffId");
                    //ShiftChangeApprvalModel ApproverData = context.Database.SqlQuery<ShiftChangeApprvalModel>(builder.ToString()).FirstOrDefault();

                    if (loggedInUserRole == "1" || loggedInUserRole == "3" || loggedInUserRole == "4" || loggedInUserRole == "5")
                    {
                        selfapproval = true;

                        if (cattdata.LoggedInUserId == cattdata.StaffId && cattdata.LoggedInUserId == ReportingManager)
                        {
                            selfapproval = true;
                        }
                        else if (cattdata.LoggedInUserId != cattdata.StaffId && cattdata.LoggedInUserId == ReportingManager)
                        {
                            selfapproval = true;
                        }
                        else if (cattdata.LoggedInUserId == cattdata.StaffId && cattdata.StaffId != ReportingManager)
                        {
                            selfapproval = false;
                        }

                        //if ((ApproverData.Approver1 == cattdata.LoggedInUserId && ApproverData.Approver2 == cattdata.LoggedInUserId && ApproverData.ApproverLevel == 2) || (ApproverData.Approver1 == cattdata.LoggedInUserId && ApproverData.ApproverLevel == 1))
                        //{
                        //    selfapproval = true;
                        //}
                        //else
                        //{
                        //    selfapproval = false;
                        //}
                    }
                    //RALeaveApplicationRepository RAL = new RALeaveApplicationRepository();
                    //ApplicationApproval AA = new ApplicationApproval();
                    //AA.Id = RAL.GetUniqueId();
                    //AA.ParentId = sca.Id;

                    //if ((ApproverData.ApproverLevel == 1 && selfapproval == true) || (ApproverData.ApproverLevel == 2 && selfapproval == true))
                    //{
                    //    AA.ApprovalStatusId = 2;
                    //    AA.ApprovedBy = ApproverData.Approver1;
                    //    AA.ApprovedOn = DateTime.Now;
                    //    AA.Comment = "SELF APPROVAL";
                    //    AA.ApprovalOwner = ApproverData.Approver1;
                    //}
                    //else
                    //{
                    //    AA.ApprovalStatusId = 1;
                    //    AA.ApprovedBy = ApproverData.Approver1;
                    //    AA.Comment = "PENDING APPROVAL";
                    //    AA.ApprovalOwner = ApproverData.Approver1;
                    //}
                    
                    //AA.ParentType = "SC";
                    //AA.ForwardCounter = 1;
                    //AA.ApplicationDate = DateTime.Now;
                    //if (ApproverData.Approver1 == cattdata.LoggedInUserId && ApproverData.Approver2 == cattdata.LoggedInUserId && ApproverData.ApproverLevel == 2)
                    //{
                    //    AA.Approval2Owner = cattdata.LoggedInUserId;
                    //    AA.Approval2statusId = 2;
                    //    AA.Approval2On = DateTime.Now;
                    //    AA.Approval2By = cattdata.LoggedInUserId;
                    //}
                    //else if (ApproverData.Approver1 == cattdata.LoggedInUserId && ApproverData.ApproverLevel == 1)
                    //{
                    //    AA.Approval2statusId = 0;
                    //}

                    if (string.IsNullOrEmpty(sca.Id) == true) //if not then...
                    {
                        if (string.IsNullOrEmpty(sca.Id) == true)
                        {
                            var mr = new MasterRepository();
                            lastid = mr.getmaxid("shiftchangeapplication", "id", "SC", "", 10, ref lastid);
                            sca.Id = lastid;
                        }

                        context.ShiftChangeApplication.AddOrUpdate(sca);

                        if (ReportingManager == null)
                        {
                            var QryStr = new StringBuilder();
                            QryStr.Clear();
                            QryStr.Append("Reporting Manager is not configured for given staffid (@StaffId)");
                            context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@StaffId", cattdata.StaffId));
                            throw new Exception("Reporting Manager is not configured for given staffid (@StaffId)");
                        }
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

                    if (selfapproval == false)
                    {
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
                    }
                    else
                    {
                        DateTime toDate = DateTime.Now;
                        if (cattdata.FromDate.Date < currentDate.Date)
                        {
                            if (cattdata.ToDate.Date >= currentDate.Date)
                            {
                                toDate = DateTime.Now.AddDays(-1);
                            }
                            repo.LogIntoIntoAttendanceControlTable(cattdata.StaffId, cattdata.FromDate.Date, cattdata.ToDate.Date, "SC", "");
                        }
                        var ShiftName = string.Empty;
                        var ShiftShortName = string.Empty;
                        DateTime StartTime = DateTime.Now;
                        DateTime EndTime = DateTime.Now;
                        TimeSpan ExpectedWorkingHours;
                        Int64 Id = 0;
                        int IsWeeklyOff = 0;
                    
                        try
                        {
                            if (cattdata.NewShiftId == "LV0011")
                            {
                                ShiftName = "WO";
                                ShiftShortName = "WO";
                                StartTime = Convert.ToDateTime("1900-01-01 00:00:00.000");
                                EndTime = Convert.ToDateTime("1900-01-01 00:00:00.000");
                                ExpectedWorkingHours = EndTime - StartTime;
                                IsWeeklyOff = 1;
                            }
                            else
                            {
                                var lst = new List<ShiftDetailsForShiftChange>();

                                lst = repo.GetShiftDetailsForShiftChange(cattdata.NewShiftId).ToList();

                                if (lst != null)
                                {
                                    foreach (var rec in lst)
                                    {
                                        ShiftName = rec.Name;
                                        ShiftShortName = rec.ShortName;
                                        StartTime = rec.StartTime;
                                        EndTime = rec.EndTime;
                                    }
                                }

                                ExpectedWorkingHours = EndTime - StartTime;
                                if (EndTime > StartTime)
                                {
                                    ExpectedWorkingHours = EndTime - StartTime;
                                }
                                else
                                {
                                    var EndTime1 = EndTime.AddDays(1);
                                    ExpectedWorkingHours = EndTime1 - StartTime;
                                }
                                var startTime = StartTime.ToString("hh:mm:tt");
                                var endTime = EndTime.ToString("hh:mm:tt");
                            }

                            for (var dt = Convert.ToDateTime(cattdata.FromDate); dt <= Convert.ToDateTime(cattdata.ToDate); dt = dt.AddDays(1))
                            {
                                dates.Add(dt);
                            }
                            foreach (var date in dates)
                            {
                                DateTime ShiftOutDate = date;
                                if (EndTime < StartTime)
                                {
                                    ShiftOutDate = date.AddDays(1);
                                }
                          
                                    var QryStr1 = new StringBuilder();
                                QryStr1.Append("Select Id from AttendanceData where StaffId=@StaffId AND CONVERT ( DATETIME , CONVERT ( VARCHAR , ShiftInDate , 106 ) )=@date");
                                context.Database.ExecuteSqlCommand(QryStr1.ToString(), new SqlParameter("@StaffId", cattdata.StaffId), new SqlParameter("@date", date));
                                try
                                {
                                    Id = context.Database.SqlQuery<Int64>(QryStr1.ToString()).FirstOrDefault();
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                                if (Id != 0)
                                {

                                    var QryStr2 = new StringBuilder();
                                    QryStr2.Append("Update AttendanceData set ShiftId=@NewShiftId, ShiftShortName=@ShiftShortName," +
                                        " ShiftInDate = date, ShiftInTime = @StartTime," +
                                        " ShiftOutDate = @ShiftOutDate, IsWeeklyOff=@IsWeeklyOff," +
                                        "ShiftOutTime = @EndTime, ExpectedWorkingHours = @ExpectedWorkingHours, " +
                                        " [IsProcessed] = 0 where  Id= @Id AND  StaffId=@StaffId");
                                    context.Database.ExecuteSqlCommand(QryStr2.ToString(), new SqlParameter("@NewShiftId", cattdata.NewShiftId), new SqlParameter("@ShiftShortName", ShiftShortName),
                                        new SqlParameter("@date", date), new SqlParameter("@StartTime", StartTime), new SqlParameter("@ShiftOutDate", ShiftOutDate),
                                        new SqlParameter("@IsWeeklyOff",IsWeeklyOff), new SqlParameter("@EndTime", EndTime),
                                        new SqlParameter("@ExpectedWorkingHours", ExpectedWorkingHours),new SqlParameter("@Id", Id),
                                        new SqlParameter("@StaffId", cattdata.StaffId));
                                }

                                if (Id == 0)
                                {
                                    var QryStr3 = new StringBuilder();
                                    QryStr3.Append(" Insert into AttendanceData([StaffId],[ShiftId],[ShiftShortName],[ShiftInDate],[ShiftInTime]," +
                                    " [ShiftOutDate],  [ShiftOutTime],[ExpectedWorkingHours],[IsEarlyComing],[IsEarlyComingValid],[IsLateComing]," +
                                   "  [IsLateComingValid],[IsEarlyGoing], [IsEarlyGoingValid],[IsLateGoing],[IsLateGoingValid],[IsOT],[IsOTValid]," +
                                    " [IsManualPunch],[IsSinglePunch],[IsIncorrectPunches],[IsDisputed],[OverRideEarlyComing]," +
                                    " [OverRideLateComing],[OverRideEarlyGoing],[OverRideLateGoing],[OverRideOT],[AttendanceStatus]," +
                                    " [FHStatus],[SHStatus],[AbsentCount],[DayAccount],[IsLeave],[IsLeaveValid],[IsLeaveWithWages],[IsAutoShift]," +
                                    " [IsWeeklyOff],[IsPaidHoliday],[IsProcessed]) VALUES (@StaffId,@NewShiftId,@ShiftShortName," +
                                    " @date,@StartTime,@ShiftOutDate," +
                                    " @EndTime , @ExpectedWorkingHours,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," +
                                    " 0,0,0,0,'-','-','-',0,0,0,0,0,0,@IsWeeklyOff,0,0)");

                                    context.Database.ExecuteSqlCommand(QryStr3.ToString(), new SqlParameter("@StaffId", cattdata.StaffId),
                                        new SqlParameter("@NewShiftId", cattdata.NewShiftId), new SqlParameter("@ShiftShortName", ShiftShortName),
                                        new SqlParameter("@date", date), new SqlParameter("@StartTime", StartTime), new SqlParameter("@ShiftOutDate", ShiftOutDate),
                                        new SqlParameter("@IsWeeklyOff",IsWeeklyOff), new SqlParameter("@EndTime", EndTime),
                                        new SqlParameter("@ExpectedWorkingHours", ExpectedWorkingHours));
                                }
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        //SEND APPROVAL/REJECTION EMAIL TO THE REQUESTER.
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
        public List<OldShift> GetShiftList()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select 'LV0011'  as id , 'WO' as ShortName,'WEEKLY OFF' as Name , '00:00' as ShiftInTime , 'WEEKLY OFF' as ShiftOutTime" +
                           " union  Select Id , ShortName , convert ( varchar ( 5 ) , StartTime , 114 ) ShiftInTime ," +
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
            SqlParameter[] Param = new System.Data.SqlClient.SqlParameter[2];
            Param[0] = new System.Data.SqlClient.SqlParameter("@StaffId", StaffId);
            Param[1] = new System.Data.SqlClient.SqlParameter("@ShiftChangeAccess", ShiftChangeAccess);
            var qryStr = new StringBuilder();

            qryStr.Append("select Id as Id , StaffId , FirstName as StaffName , FromDate as DateFrom ,ToDate as DateTo , " +
                "NewShiftId as NewShiftId , ShiftName as NewShiftName , DeptName as DepartmentName ,Reason as Reason , " +
                "StatusId as StatusId ,Status as StatusName from vwShiftChangeList  where StaffId = @StaffId ORDER BY ID DESC ");

            try
            {
                var lst = context.Database.SqlQuery<ShiftChangeList>(qryStr.ToString(),new SqlParameter("@StaffId",StaffId)).ToList();

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

