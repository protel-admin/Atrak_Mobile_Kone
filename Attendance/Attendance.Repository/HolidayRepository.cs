using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Configuration;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Attendance.Repository
{
    public class HolidayRepository : TrackChangeRepository,IDisposable
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
        //private readonly IsolationLevel qryStr;

        public HolidayRepository()
        {
            context = new AttendanceManagementContext();
        }
        public List<HolidayList> GetHolidayCalendar(string HolidayGroupId)
        {
            //create a parameter
            var param1 = new SqlParameter();
            param1.ParameterName = "@HolidayGroupID";
            param1.SqlDbType = SqlDbType.VarChar;
            param1.Size = 10;
            param1.Direction = ParameterDirection.Input;
            param1.Value = HolidayGroupId;
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("dbo.spGetHolidayCalendar @HolidayGroupID");

            try
            {
                var lst = context.Database.SqlQuery<HolidayList>(qryStr.ToString(), param1).Select(d => new HolidayList()
                {
                    Hid = d.Hid,
                    LeaveTypeId = d.LeaveTypeId,
                    Name = d.Name,
                    HolidayDateFrom = d.HolidayDateFrom,
                    HolidayDateTo = d.HolidayDateTo,
                    IsFixed = d.IsFixed,
                    LeaveYear = d.LeaveYear
                }).ToList();

                if (lst == null)
                {
                    return new List<HolidayList>();
                }
                else
                {
                    return lst;
                }

            }
            catch (Exception)
            {
                return new List<HolidayList>();
            }
        }

        public List<HolidayGroupList> GetHolidayGroups()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select Id , Name , LeaveYear ,IsCurrent , IsActive , CreatedOn , CreatedBy from HolidayGroup");

            try
            {
                var lst = context.Database.SqlQuery<HolidayGroupList>(qryStr.ToString()).Select(d => new HolidayGroupList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    LeaveYear = d.LeaveYear,
                    IsCurrent = d.IsCurrent,
                    IsActive = d.IsActive,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy
                }).ToList();

                if (lst == null)
                {
                    return new List<HolidayGroupList>();
                }
                else
                {
                    return lst;
                }

            }
            catch (Exception)
            {
                return new List<HolidayGroupList>();
            }
        }

        public HolidayGroupList GetHolidayGroupDetails(string id)
        {
            SqlParameter[] Param = new SqlParameter[1];
            Param[0] = new SqlParameter("@Id", id);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id , Name , LeaveYear , " +
                "IsCurrent , IsActive from holidaygroup where id = @Id");

            try
            {
                var hg = context.Database.SqlQuery<HolidayGroupList>(qryStr.ToString(), Param).Select(d => new HolidayGroupList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    LeaveYear = d.LeaveYear,
                    IsCurrent = d.IsCurrent,
                    IsActive = d.IsActive
                }).FirstOrDefault();

                if (hg == null)
                {
                    return new HolidayGroupList();
                }
                else
                {
                    hg.HolidayGroupTxnList = GetHolidayCalendar(hg.Id);
                    return hg;
                }
            }
            catch (Exception)
            {
                return new HolidayGroupList();
            }
        }

        public List<HolidayList> GetHolidayGroupList(int leaveyear)
        {
            SqlParameter[] Param = new SqlParameter[1];
            Param[0] = new SqlParameter("@leaveyear", leaveyear);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select convert ( varchar , Hid ) as Hid, LeaveTypeId , Name , " +
                          "convert ( varchar , IsActive ) as IsActive , isnull ( HolidayDateFrom , '' ) as HolidayDateFrom , " +
                          "isnull ( HolidayDateTo , '' ) as HolidayDateTo , convert ( varchar , IsFixed ) as IsFixed , " +
                          "convert ( varchar , LeaveYear ) as LeaveYear from vwHolidayCalendarView " +
                          "where leaveyear = @leaveyear");

            try
            {
                var lst = context.Database.SqlQuery<HolidayList>(qryStr.ToString(), Param).Select(d => new HolidayList()
                {
                    Hid = d.Hid,
                    LeaveTypeId = d.LeaveTypeId,
                    Name = d.Name,
                    HolidayDateFrom = d.HolidayDateFrom,
                    HolidayDateTo = d.HolidayDateTo,
                    IsFixed = d.IsFixed,
                    LeaveYear = d.LeaveYear
                }).ToList();

                if (lst == null)
                {
                    return new List<HolidayList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<HolidayList>();
            }
        }

        public void SaveHolidayGroupDetails(HolidayGroupList hgl, string loggedInUser)
        {
            SqlParameter[] Param = new SqlParameter[2];
            Param[0] = new SqlParameter("@hgl", hgl);
            Param[1] = new SqlParameter("@loggedInUser", loggedInUser);
            var lastid = string.Empty;
            var subid = string.Empty;

            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    //save holiday group information.
                    var hg = new HolidayGroup();
                    if (hgl.Id == null)
                    {
                        var mr = new MasterRepository();
                        lastid = mr.getmaxid("holidaygroup", "Id", "HG", "", 10, ref lastid);
                        subid = lastid;
                        hg.Id = lastid;
                        hg.Name = hgl.Name;
                        hg.LeaveYear = hgl.LeaveYear;
                        hg.IsCurrent = hgl.IsCurrent;
                        hg.IsActive = hgl.IsActive;
                        hg.CreatedOn = DateTime.Now;
                        hg.CreatedBy = loggedInUser;
                        hg.ModifiedOn = hgl.ModifiedOn;
                        hg.ModifiedBy = hgl.ModifiedBy;
                        context.HolidayGroups.AddOrUpdate(hg);
                        context.SaveChanges();
                    }
                    else
                    {
                        lastid = hgl.Id;
                        subid = lastid;
                        hg.Id = lastid;
                        hg.Name = hgl.Name;
                        hg.LeaveYear = hgl.LeaveYear;
                        hg.IsCurrent = hgl.IsCurrent;
                        hg.IsActive = hgl.IsActive;
                        hg.CreatedOn = hgl.CreatedOn;
                        hg.CreatedBy = hgl.CreatedBy;
                        hg.ModifiedOn = DateTime.Now;
                        hg.ModifiedBy = loggedInUser;
                        context.HolidayGroups.AddOrUpdate(hg);
                        string ActionType = string.Empty;
                        string _ChangeLog = string.Empty;
                        string _PrimaryKeyValue = string.Empty;
                        GetChangeLogString(hg, context, ref _ChangeLog, ref ActionType, ref _PrimaryKeyValue);
                        context.SaveChanges();
                        if (string.IsNullOrEmpty(_ChangeLog.ToString()) == false)
                        {
                            RecordChangeLog(context, loggedInUser, "HOLIDAYGROUP", _ChangeLog, ActionType, _PrimaryKeyValue);
                        }

                    }

                    //delete old holiday data.
                    var qryStr = new StringBuilder();
                    qryStr.Append("delete from holidaygrouptxn where HolidayGroupId = @subid");
                    context.Database.ExecuteSqlCommand(qryStr.ToString(), new SqlParameter("@subid", subid));
                    //save new holiday calendar settings.
                    var hgt = new HolidayGroupTxn();
                    foreach (var l in hgl.HolidayList)
                    {
                        hgt = new HolidayGroupTxn();
                        hgt.HolidayGroupId = subid;
                        hgt.HolidayId = l.HolidayId;
                        hgt.HolidayDateFrom = l.HolidayDateFrom;
                        hgt.HolidayDateTo = l.HolidayDateTo;
                        hgt.IsActive = l.IsActive;
                        context.HolidayGroupTxns.Add(hgt);
                        context.SaveChanges();
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
        public ShiftDetails GetShiftDetails(string ShiftId)
        {
            SqlParameter[] Param = new SqlParameter[1];
            Param[0] = new SqlParameter("@ShiftId", ShiftId);
            var queryString = new StringBuilder();
            var qryStr = new StringBuilder();

            qryStr.Clear();
            qryStr.Append("Select ShortName,Convert(Varchar(8),StartTime,114) as StartTime,Convert(Varchar(8),EndTime,114) as EndTime " +
                "from [Shifts] where Id= @ShiftId");

            try
            {
                var lst = context.Database.SqlQuery<ShiftDetails>(qryStr.ToString(), Param).Select(d => new ShiftDetails()
                {
                    ShortName = d.ShortName,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                }).FirstOrDefault();

                if (lst == null)
                {
                    return new ShiftDetails();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new ShiftDetails();
            }
        }

        public List<HolidayListForMaster> GetAllHolidays()
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append(" Select A.Id , A.LeaveTypeId , A.Name ,A.IsActive , B.Name as LeaveTypeName from Holiday A Inner Join " +
                "LeaveType B ON A.LeaveTypeId = B.Id ");
            try
            {
                var lst = context.Database.SqlQuery<HolidayListForMaster>(queryString.ToString()).Select(d => new HolidayListForMaster()
                {
                    Id = d.Id,
                    Name = d.Name,
                    LeaveTypeId = d.LeaveTypeId,
                    LeaveTypeName = d.LeaveTypeName,
                    IsActive = d.IsActive,
                }).ToList();
                return lst;
            }
            catch
            {
                return new List<HolidayListForMaster>();
            }


        }
        public List<LeaveTypeListForHoliday> GetLeaveTypesForHolidayMaster()
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append(" Select Id , Name from LeaveType where Id in ('LV0001','LV0014','LV0015','LV0013','LV0037','LV0041')");
            try
            {
                var lst = context.Database.SqlQuery<LeaveTypeListForHoliday>(queryString.ToString()).Select(d => new LeaveTypeListForHoliday()
                {
                    Id = d.Id,
                    Name = d.Name,
                }).ToList();
                return lst;
            }
            catch
            {
                return new List<LeaveTypeListForHoliday>();
            }
        }
        public void SaveHolidayDetails(Holiday hm)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Holidays.AddOrUpdate(hm);
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception err)
                {
                    transaction.Rollback();
                    throw err;
                }
            }
        }
        //Holiday Working
        public void SaveHolidayWorkingDetails(HolidayWorking hw, string txnDateforUpdate, string UserType)
        {
            var repo = new CommonRepository();
            string ReportingManager = string.Empty;
            string BaseAddress = string.Empty;
            var EmailStr = string.Empty;
            string ReportingManagerEmailId = string.Empty;
            string StaffEmailId = string.Empty;
            string StaffName = string.Empty;
            string ReportingManagerName = string.Empty;
            string OBRMStaffId = string.Empty;
            string OneAboveReportingManagerEmailId = string.Empty;
            string OneAboveReportingManagerName = string.Empty;
            string HRStaffid = string.Empty;
            string HREmailId = string.Empty;
            var joinstring = string.Empty;
            var queryString1 = new StringBuilder();
            var AAobj = new ApplicationApproval();
            var maxid = string.Empty;
            var lastid = string.Empty;
            var aa = new ApplicationApproval();
            var mr = new MasterRepository();
            int holidayWorkingId = 0;

            using (var trans = context.Database.BeginTransaction())
            {

                try
                {
                    queryString1.Clear();
                    queryString1.Append(" Select Max(Id) from [HolidayWorking]");
                    holidayWorkingId = context.Database.SqlQuery<int>(queryString1.ToString()).FirstOrDefault();
                    holidayWorkingId = holidayWorkingId + 1;
                }
                catch
                {
                    holidayWorkingId = 1;
                }
                try
                {
                    hw.TxnDate = Convert.ToDateTime(txnDateforUpdate);
                    /*
                    =========== THE BELOW CODE WILL BE USED WHEN THERE IS NO APPROVAL HIERARCHY ===========
                    */
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@StaffId";
                    param1.Direction = ParameterDirection.Input;
                    param1.SqlDbType = SqlDbType.VarChar;
                    param1.Value = hw.StaffId;
                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@TxnDate";
                    param2.Direction = ParameterDirection.Input;
                    param2.SqlDbType = SqlDbType.DateTime;
                    param2.Value = hw.TxnDate;
                    //var queryString = new StringBuilder();
                    //queryString.Append("Update HolidayWorking set IsActive= 0 where IsActive = 1 AND StaffId = '" + hw.StaffId + "' AND TxnDate = '" + txnDateforUpdate + "'");
                    //context.Database.ExecuteSqlCommand(queryString.ToString());

                    context.Database.ExecuteSqlCommand(" Update HolidayWorking set IsActive = 0 where IsActive = 1 AND StaffId =  @StaffId " +
                    " AND TxnDate = @TxnDate", new SqlParameter("@StaffId", hw.StaffId), new SqlParameter("@TxnDate", txnDateforUpdate));

                    context.HolidayWorking.Add(hw);
                    ReportingManager = repo.GetReportingManager(hw.StaffId);
                    if (string.IsNullOrEmpty(ReportingManager).Equals(true))
                    {
                        ReportingManager = hw.StaffId;
                        //  throw new ApplicationException("Application cannot be submitted because reporting manager has not been configured.");
                    }
                    //get the emailid of the reporting manager.
                    ReportingManagerEmailId = repo.GetEmailIdOfEmployee(ReportingManager);
                    //get the emailid of the staff who raises the leave application.
                    StaffEmailId = repo.GetEmailIdOfEmployee(hw.StaffId);
                    if (string.IsNullOrEmpty(StaffEmailId).Equals(true))
                    {

                        throw new ApplicationException("Application cannot be submitted because your mail id has not been configured.");
                    }
                    //get the name of the staff.
                    StaffName = repo.GetStaffName(hw.StaffId);
                    //get the name of the reporting manager.
                    ReportingManagerName = repo.GetStaffName(ReportingManager);

                    OBRMStaffId = repo.GetReportingManager(ReportingManager);
                    if (string.IsNullOrEmpty(OBRMStaffId).Equals(true))
                    {
                        OBRMStaffId = ReportingManager;
                    }
                    //get the one above reporting manager email id
                    OneAboveReportingManagerEmailId = repo.GetEmailIdOfEmployee(OBRMStaffId);
                    //get the one above reporting manager name
                    OneAboveReportingManagerName = repo.GetStaffName(OBRMStaffId);

                    //var OBRMStaffId = repo.GetOneAboveManagerFromTeamHierarchy(law.StaffId);
                    if (UserType.Equals("BasicUser"))
                    {
                        maxid = mr.getmaxid("ApplicationApproval", "id", "AA", "", 10, ref lastid);
                        AAobj.Id = maxid;
                        AAobj.ParentId = holidayWorkingId.ToString();
                        AAobj.ParentType = "HW";
                        AAobj.ApplicationDate = DateTime.Now;
                        AAobj.ApprovalStatusId = 1;
                        AAobj.ForwardCounter = 1;
                        AAobj.Comment = "-";
                        AAobj.ApprovalOwner = ReportingManager;
                        AAobj.ApprovedBy = ReportingManager;
                        AAobj.ApprovedOn = DateTime.Now;
                        context.ApplicationApproval.Add(AAobj);
                        context.SaveChanges();
                        trans.Commit();
                        string shiftInDate = string.Empty;
                        string shiftOutDate = string.Empty;
                        string shiftInTime = string.Empty;
                        string shiftOutTime = string.Empty;
                        string shiftIn = string.Empty;
                        string shiftOut = string.Empty;
                        shiftInDate = Convert.ToDateTime(hw.ShiftInDate).ToString("dd-MM-yyyy");
                        shiftOutDate = Convert.ToDateTime(hw.ShiftOutDate).ToString("dd-MM-yyyy");
                        shiftInTime = Convert.ToDateTime(hw.ShiftInTime).ToString("HH:mm");
                        shiftOutTime = Convert.ToDateTime(hw.ShiftOutTime).ToString("HH:mm");
                        shiftIn = string.Concat(shiftInDate, " ", shiftInTime);
                        shiftOut = string.Concat(shiftOutDate, " ", shiftOutTime);
                        BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                        var staffid = hw.StaffId;
                        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a holiday working. Holiday working details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(hw.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required for this application.</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">10.114.76.61:8011</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " &nbsp;(" + staffid + ")</p></body></html>";
                        // send intimation to the reporting manager about the shift extension application.
                        repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Holiday working application of " + StaffName, EmailStr);

                        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + OneAboveReportingManagerName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a holiday working. Holiday working details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(hw.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " &nbsp;(" + staffid + ")</p></body></html>";
                        // send intimation to the reporting manager about the shift extension application.
                        repo.SendEmailMessage(StaffEmailId, OneAboveReportingManagerEmailId, "", "", "Holiday working application of " + StaffName, EmailStr);

                        // send acknowledgement to the staff who raised the shift extension application.
                        EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Holiday working application has been submitted to your Reporting Manager " + ReportingManagerName + " (" + ReportingManager + ") for Approval. Holiday working details given below. <p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(hw.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>";
                        repo.SendEmailMessage("", StaffEmailId, "", "", "Holiday working application sent to " + ReportingManagerName, EmailStr);

                    }
                    else
                    {

                        maxid = mr.getmaxid("ApplicationApproval", "id", "AA", "", 10, ref lastid);
                        AAobj.Id = maxid;
                        AAobj.ParentId = holidayWorkingId.ToString();
                        AAobj.ParentType = "HW";
                        AAobj.ApplicationDate = DateTime.Now;
                        AAobj.ApprovalStatusId = 1;
                        AAobj.ForwardCounter = 1;
                        AAobj.Comment = "-";
                        AAobj.ApprovalOwner = OBRMStaffId;
                        AAobj.ApprovedBy = OBRMStaffId;
                        AAobj.ApprovedOn = DateTime.Now;
                        context.ApplicationApproval.Add(AAobj);

                        context.SaveChanges();
                        trans.Commit();
                        string shiftInDate = string.Empty;
                        string shiftOutDate = string.Empty;
                        string shiftInTime = string.Empty;
                        string shiftOutTime = string.Empty;
                        string shiftIn = string.Empty;
                        string shiftOut = string.Empty;
                        shiftInDate = Convert.ToDateTime(hw.ShiftInDate).ToString("dd-MM-yyyy");
                        shiftOutDate = Convert.ToDateTime(hw.ShiftOutDate).ToString("dd-MM-yyyy");
                        shiftInTime = Convert.ToDateTime(hw.ShiftInTime).ToString("HH:mm");
                        shiftOutTime = Convert.ToDateTime(hw.ShiftOutTime).ToString("HH:mm");
                        shiftIn = string.Concat(shiftInDate, " ", shiftInTime);
                        shiftOut = string.Concat(shiftOutDate, " ", shiftOutTime);
                        BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();

                        if (string.IsNullOrEmpty(OneAboveReportingManagerEmailId) == false)
                        {
                            joinstring = string.Concat(ReportingManagerEmailId, ",", OneAboveReportingManagerEmailId);
                        }

                        var staffid = hw.StaffId;

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

                                EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your holiday working application has been submitted to your Reporting Manager (" + ReportingManagerName + ") for Approval.<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + hw.TxnDate + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(hw.ShiftInDate).ToString("dd-mm-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In Time</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + hw.ShiftInTime + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift Out Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(hw.ShiftOutDate).ToString("dd-mm-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift Out Time</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + hw.ShiftOutTime + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>";
                                //send intimation to the staff stating that his/her leave application has been acknowleged 
                                //function call to get the name of the staff and the reporting manager.
                                //  but the reporting manager does not have a email id so no intimation has been sent to him.
                                repo.SendEmailMessage("", StaffEmailId, "", "", "Holiday working application of " + hw.StaffId + " - " + StaffName, EmailStr);
                            }
                        }
                        else // if the reporting manager has an email id then...
                        {
                            string OneAboveManagerMailTriggerType = string.Empty;
                            string HolidayWorkingnApprovalNotificationForOAM = string.Empty;
                            string OneAboveManagerMailTriggerTypeForHolidayWorking = string.Empty;
                            try
                            {
                                OneAboveManagerMailTriggerType = ConfigurationManager.AppSettings["OneAboveManagerMailTriggerType"].ToString().ToUpper().Trim();
                                HolidayWorkingnApprovalNotificationForOAM = ConfigurationManager.AppSettings["HolidayWorkingnApprovalNotificationForOAM"].ToString().ToUpper().Trim();
                                OneAboveManagerMailTriggerTypeForHolidayWorking = ConfigurationManager.AppSettings["OneAboveManagerMailTriggerTypeForHolidayWorking"].ToString().ToUpper().Trim();
                            }
                            catch
                            {
                                OneAboveManagerMailTriggerType = "cc";
                                HolidayWorkingnApprovalNotificationForOAM = "YES";
                                OneAboveManagerMailTriggerTypeForHolidayWorking = "TO";
                            }
                            if (HolidayWorkingnApprovalNotificationForOAM.Trim().ToUpper().Equals("YES"))
                            {
                                if (OneAboveManagerMailTriggerTypeForHolidayWorking.Trim().ToUpper().Equals("CC"))
                                {
                                    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a Holiday working. Holiday working details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(hw.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required for this application.</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">10.114.76.61:8011</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " &nbsp;(" + staffid + ")</p></body></html>";
                                    // send intimation to the reporting manager about the shift extension application.
                                    repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Holiday working application of " + StaffName, EmailStr);

                                    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + OneAboveReportingManagerName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a Holiday working. Holiday working details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(hw.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In :</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " &nbsp;(" + staffid + ")</p></body></html>";
                                    // Send CC to the one above manager about the shift extension application.
                                    repo.SendEmailMessage(StaffEmailId, OneAboveReportingManagerEmailId, "", "", "Holiday working application of " + StaffName, EmailStr);

                                    // send acknowledgement to the staff who raised the shift extension application.
                                    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your holiday working application has been submitted to your Reporting Manager (" + ReportingManagerName + ") for Approval.<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(hw.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>";
                                    repo.SendEmailMessage("", StaffEmailId, "", "", "Holiday working application sent to " + ReportingManagerName, EmailStr);
                                }
                                else if (OneAboveManagerMailTriggerTypeForHolidayWorking.Trim().ToUpper().Equals("TO"))
                                {
                                    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + OneAboveReportingManagerName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ReportingManagerName + " has applied  a Holiday working for " + StaffName + ". Holiday working details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(hw.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required for this application.</p><p style=\"font-family:tahoma; font-size:9pt;\"><p style=\"font-family:tahoma; font-size:9pt;\">Click the below link to login into LMS.</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">10.114.76.61:8011</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ReportingManagerName + " &nbsp;(" + ReportingManager + ")</p></body></html>";
                                    // send intimation to the one above manager about the shift extension application.
                                    repo.SendEmailMessage(ReportingManagerEmailId, OneAboveReportingManagerEmailId, "", "", "Holiday working application of " + StaffName, EmailStr);

                                    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\"> Holiday working application applied by you for  " + StaffName + " has been sent to your reporting manager " + OneAboveReportingManagerName + " for approval. Holiday working details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(hw.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>";
                                    // Send CC to the reporting manager about the shift extension application.
                                    repo.SendEmailMessage("", ReportingManagerEmailId, "", "", "Holiday working application of " + StaffName, EmailStr);

                                    // send acknowledgement to the staff who raised the shift extension application.
                                    EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Holiday working application applied by your Reporting Manager " + ReportingManagerName + " has been sent to your one above manager " + OneAboveReportingManagerName + " for Approval.<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(hw.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + ReportingManagerName + " &nbsp;(" + ReportingManager + ")</p></body></html>";
                                    repo.SendEmailMessage(ReportingManagerEmailId, StaffEmailId, "", "", "Holiday working application applied by your Reporting Manager " + ReportingManagerName + " has been sent to " + OneAboveReportingManagerName, EmailStr);
                                }
                            }

                            else
                            {
                                EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + ReportingManagerName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " has applied for a holiday working. Holiday working details given below.</p><p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(hw.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Your attention is required for this application.</p><p style=\"font-family:tahoma; font-size:9pt;\"><a href=\"" + BaseAddress + "\">10.114.76.61:8011</a></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards,</p><p style=\"font-family:tahoma; font-size:9pt;\">" + StaffName + " &nbsp;(" + staffid + ")</p></body></html>";
                                // send intimation to the reporting manager about the shift extension application.
                                repo.SendEmailMessage(StaffEmailId, ReportingManagerEmailId, "", "", "Holiday working application of " + StaffName, EmailStr);

                                // send acknowledgement to the staff who raised the shift extension application.
                                EmailStr = "<html><head><title></title></head><body><p style=\"font-family:tahoma; font-size:9pt;\">Dear " + StaffName + ",<br><br>Greetings</p><p style=\"font-family:tahoma; font-size:9pt;\">Your Holiday working application has been submitted to your Reporting Manager (" + ReportingManagerName + ") for Approval.<p style=\"font-family:tahoma; font-size:9pt;\"><table border=\"1\" style=\"width:50%;font-family:tahoma; font-size:9pt;\"><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Name:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + StaffName + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Txn Date:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + Convert.ToDateTime(hw.TxnDate).ToString("dd-MMM-yyyy") + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift In:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftIn + "</td></tr><tr><td style=\"width:20%;font-family:tahoma; font-size:9pt;\">Shift Out:</td><td style=\"width:80%;font-family:tahoma; font-size:9pt;\">" + shiftOut + "</td></tr></table></p><p style=\"font-family:tahoma; font-size:9pt;\">Best Regards</p></body></html>";
                                repo.SendEmailMessage("", StaffEmailId, "", "", "Holiday working application sent to " + ReportingManagerName, EmailStr);
                            }
                        }

                    }
                }
                catch (Exception err)
                {
                    trans.Rollback();
                    throw err;
                }
            }
        }
    }
}