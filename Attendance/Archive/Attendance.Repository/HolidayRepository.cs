using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;

namespace Attendance.Repository
{
    public class HolidayRepository : TrackChangeRepository
    {
        public AttendanceManagementContext context = null;

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
                    IsActive = d.IsActive , 
                    CreatedOn = d.CreatedOn , 
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
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@id", id);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id , Name , LeaveYear , " +
                "IsCurrent , IsActive from holidaygroup where id = @id");

            try
            {
                var hg = context.Database.SqlQuery<HolidayGroupList>(qryStr.ToString(),sqlParameter).Select(d => new HolidayGroupList()
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
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@leaveyear", leaveyear);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select convert ( varchar , Hid ) as Hid, LeaveTypeId , Name , " +
                          "convert ( varchar , IsActive ) as IsActive , isnull ( HolidayDateFrom , '' ) as HolidayDateFrom , " +
                          "isnull ( HolidayDateTo , '' ) as HolidayDateTo , convert ( varchar , IsFixed ) as IsFixed , " +
                          "convert ( varchar , LeaveYear ) as LeaveYear from vwHolidayCalendarView " +
                          "where leaveyear = @leaveyear");

            try
            {
                var lst = context.Database.SqlQuery<HolidayList>(qryStr.ToString(),sqlParameter).Select(d => new HolidayList()
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
                        hg.CreatedOn =DateTime.Now;
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
                            RecordChangeLog(context,loggedInUser, "HOLIDAYGROUP", _ChangeLog, ActionType, _PrimaryKeyValue);
                        }

                    }

                    SqlParameter[] sqlParameter = new SqlParameter[1];
                    sqlParameter[0] = new SqlParameter("@subid", subid);

                    //delete old holiday data.
                    var qryStr = "delete from holidaygrouptxn where HolidayGroupId = @subid";
                    context.Database.ExecuteSqlCommand(qryStr, sqlParameter);

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
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@ShiftId", ShiftId);
            var qryStr = new StringBuilder();

            qryStr.Clear();
            qryStr.Append("Select ShortName,Convert(Varchar(8),StartTime,114) as StartTime,Convert(Varchar(8),EndTime,114) as EndTime from [Shifts] where Id= @ShiftId");

            try
            {
                var lst = context.Database.SqlQuery<ShiftDetails>(qryStr.ToString(), sqlParameter).Select(d => new ShiftDetails()
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
            queryString.Append(" Select A.Id , A.LeaveTypeId , A.Name ,A.IsActive , B.Name as LeaveTypeName from Holiday A Inner Join LeaveType B ON A.LeaveTypeId = B.Id ");
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
            queryString.Append(" Select Id , Name from LeaveType Where IsCommon = 1");
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
    }
}
