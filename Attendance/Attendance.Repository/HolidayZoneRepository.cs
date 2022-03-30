using System;
using System.Collections.Generic;
using System.Data.Common;
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
    public class HolidayZoneRepository : TrackChangeRepository,IDisposable
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

        public HolidayZoneRepository()
        {
            context = new AttendanceManagementContext();
        }
        StringBuilder builder = new StringBuilder();
        public void SaveHolidayZoneWiseHolidayList(string id, string name, string isactive, string olddata, string newdata, string loggedInUser, string createdOn, string createdBy)
        {
            SqlParameter[] Param = new SqlParameter[8];
            Param[0] = new SqlParameter("@Id", id);
            Param[1] = new SqlParameter("@name", name);
            Param[2] = new SqlParameter("@isactive", isactive);
            Param[3] = new SqlParameter("@olddata", olddata);
            Param[4] = new SqlParameter("@newdata", newdata);
            Param[5] = new SqlParameter("@loggedInUser", loggedInUser);
            Param[6] = new SqlParameter("@createdOn", createdOn);
            Param[7] = new SqlParameter("@createdBy", createdBy);
            using (DbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    HolidayZone HZ = new HolidayZone();
                    if (isactive == "1")
                        HZ.IsActive = true;
                    else if (isactive == "0")
                        HZ.IsActive = false;
                    id = id == "New" ? "" : id;
                    if (string.IsNullOrEmpty(id) == false)
                    {
                        HZ.Id = Convert.ToInt16(id);
                        HZ.Name = name;
                        HZ.CreatedDate = createdOn == "New" ? DateTime.Now : Convert.ToDateTime(createdOn);
                        HZ.CreatedBy = createdBy;
                        HZ.ModifiedDate = DateTime.Now;
                        HZ.ModifiedBy = loggedInUser;
                        context.HolidayZone.AddOrUpdate(HZ);
                        string ActionType = string.Empty;
                        string _ChangeLog = string.Empty;
                        string _PrimaryKeyValue = string.Empty;
                        GetChangeLogString(HZ, context, ref _ChangeLog, ref ActionType, ref _PrimaryKeyValue);
                        context.SaveChanges();
                        if (string.IsNullOrEmpty(_ChangeLog.ToString()) == false)
                        {
                            RecordChangeLog(context, loggedInUser, "HOLIDAYZONE", _ChangeLog, ActionType, _PrimaryKeyValue);
                        }
                    }
                    else
                    {
                        HZ.Name = name;
                        HZ.CreatedDate = DateTime.Now;
                        HZ.CreatedBy = loggedInUser;
                        context.HolidayZone.AddOrUpdate(HZ);
                        context.SaveChanges();
                    }

                    if (string.IsNullOrEmpty(olddata) == true && string.IsNullOrEmpty(newdata) == false)
                    {
                        var a = newdata.Split(',');
                        HolidayZoneTxn hzt = null;
                        foreach (var l in a)
                        {
                            hzt = new HolidayZoneTxn();
                            hzt.HolidayZoneId = HZ.Id;
                            hzt.HolidayId = Convert.ToInt16(l);
                            hzt.IsActive = HZ.IsActive;
                            hzt.CreatedBy = loggedInUser;
                            hzt.CreatedDate = DateTime.Now;
                            context.HolidayZoneTxn.AddOrUpdate(hzt);
                            context.SaveChanges();
                        }
                    }
                    else if (string.IsNullOrEmpty(olddata) == false && string.IsNullOrEmpty(newdata) == true)
                    {
                        var QryStr = new StringBuilder();
                        QryStr.Clear();
                        QryStr.Append("Delete from holidayzonetxn where HolidayZoneId = @id and HolidayId in (@olddata)");
                        context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@id", id),
                        new SqlParameter("@olddata", olddata));

           
                    }
                    else if (string.IsNullOrEmpty(olddata) == false && string.IsNullOrEmpty(newdata) == false)
                    {
                        var QryStr = new StringBuilder();
                        QryStr.Clear();
                        QryStr.Append("Delete from holidayzonetxn where HolidayZoneId = @id and HolidayId in (@olddata)");
                        context.Database.ExecuteSqlCommand(QryStr.ToString(), new SqlParameter("@id", id),
                        new SqlParameter("@olddata", olddata));
                        //context.Database.ExecuteSqlCommand(
                          //  "Delete from holidayzonetxn where HolidayZoneId = " +
                          //  id + " and HolidayId in (" + olddata + ")");

                        var a = newdata.Split(',');
                        HolidayZoneTxn hzt = null;
                        foreach (var l in a)
                        {
                            hzt = new HolidayZoneTxn();
                            hzt.HolidayZoneId = HZ.Id;
                            hzt.HolidayId = Convert.ToInt16(l);
                            hzt.IsActive = HZ.IsActive;
                            hzt.CreatedBy = loggedInUser;
                            hzt.CreatedDate = DateTime.Now;
                            context.HolidayZoneTxn.AddOrUpdate(hzt);
                            context.SaveChanges();
                        }
                    }
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }

        }

        public List<HolidayZoneWiseHolidayList> GetholidayZoneWiseHolidayList(string id, string HolidayGroupid)
        {
            List<HolidayZoneWiseHolidayList> lst = new List<HolidayZoneWiseHolidayList>();
            try
            {
                SqlParameter[] Param = new SqlParameter[2];
                Param[0] = new SqlParameter("@Id", id);
                Param[1] = new SqlParameter("@HolidayGroupid", HolidayGroupid);

                builder = new StringBuilder();
                builder.Append("select convert ( varchar , HolidayId ) as HolidayId , convert ( varchar , LeaveYear ) as LeaveYear , " +
                              "LeaveTypeId , HolidayName , replace ( convert ( varchar , HolidayDateFrom , 106 ) , ' ' , '-' ) as HolidayDateFrom , " +
                              "replace ( convert ( varchar , HolidayDateTo , 106 ) , ' ' , '-' ) as HolidayDateTo , " +
                              "convert ( varchar , IsChecked ) as IsChecked from fnGetHolidayZoneWiseHolidayList ( @Id,@HolidayGroupId ) order by HolidayName asc");
                lst = context.Database.SqlQuery<HolidayZoneWiseHolidayList>(builder.ToString(), Param).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return lst;
        }

        public List<HolidayList> GetAllHolidays()
        {
            var qryStr = new StringBuilder();

            qryStr.Clear();
            qryStr.Append("select id , name from holiday");

            try
            {
                var lst = context.Database.SqlQuery<HolidayList>(qryStr.ToString()).Select(d => new HolidayList()
                {
                    Hid = d.Hid,
                    Name = d.Name
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


        public List<HolidayZoneList> GetHolidayZones()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select convert ( varchar , Id ) as Id , Name , convert ( varchar , IsActive ) as IsActive , CreatedDate , CreatedBy from HolidayZone");

            try
            {
                var lst =
                    context.Database.SqlQuery<HolidayZoneList>(qryStr.ToString()).Select(d => new HolidayZoneList()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        IsActive = d.IsActive,
                        CreatedDate = d.CreatedDate,
                        CreatedBy = d.CreatedBy
                    }).ToList();

                if (lst == null)
                {
                    return new List<HolidayZoneList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<HolidayZoneList>();
            }
        }

        public string SaveHolidayZones()
        {
            return null;
        }
    }
}
