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

namespace Attendance.Repository {
    public class HolidayZoneRepository:TrackChangeRepository
    {

        private AttendanceManagementContext context = null;

        public HolidayZoneRepository()
        {
            context = new AttendanceManagementContext();
        }

        public void SaveHolidayZoneWiseHolidayList(string id, string name, string isactive, string olddata, string newdata, string loggedInUser , DateTime createdOn ,string createdBy)
        {
            using (DbContextTransaction trans = context.Database.BeginTransaction())
            {
                var qrystr = new StringBuilder();

                try
                {
                    HolidayZone HZ = new HolidayZone();
                    if (isactive == "1")
                        HZ.IsActive = true;
                    else if (isactive == "0")
                        HZ.IsActive = false;
                    if (string.IsNullOrEmpty(id) == false)
                    {
                        HZ.Id = Convert.ToInt16(id);
                        HZ.Name = name;
                        HZ.CreatedDate = createdOn;
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
            
           
                    SqlParameter[] sqlParameter = new SqlParameter[2];
                    sqlParameter[0] = new SqlParameter("@olddata", olddata);
                    sqlParameter[1] = new SqlParameter("@id", id);
                  
                    
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
                            hzt.CreatedBy = "user";
                            hzt.CreatedDate = DateTime.Now;
                            context.HolidayZoneTxn.AddOrUpdate(hzt);
                            context.SaveChanges();
                        }
                    }
             

                    else if (string.IsNullOrEmpty(olddata) == false && string.IsNullOrEmpty(newdata) == true)
                    {
                  
                        context.Database.ExecuteSqlCommand("Delete from holidayzonetxn where HolidayZoneId = @id and HolidayId in ( @olddata )", sqlParameter);
                    }
                    else if (string.IsNullOrEmpty(olddata) == false && string.IsNullOrEmpty(newdata) == false)
                    {
                        context.Database.ExecuteSqlCommand("Delete from holidayzonetxn where HolidayZoneId = @id and HolidayId in ( @olddata )", sqlParameter);

                        var a = newdata.Split(',');
                        HolidayZoneTxn hzt = null;
                        foreach (var l in a)
                        {
                            hzt = new HolidayZoneTxn();
                            hzt.HolidayZoneId = HZ.Id;
                            hzt.HolidayId = Convert.ToInt16(l);
                            hzt.IsActive = HZ.IsActive;
                            hzt.CreatedBy = "user";
                            hzt.CreatedDate = DateTime.Now;
                            context.HolidayZoneTxn.AddOrUpdate(hzt);
                            context.SaveChanges();
                        }
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

        public List<HolidayZoneWiseHolidayList> GetholidayZoneWiseHolidayList(string id, string HolidayGroupid)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@HolidayGroupid", HolidayGroupid);
            sqlParameter[1] = new SqlParameter("@id", id);
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select convert ( varchar , HolidayId ) as HolidayId , convert ( varchar , LeaveYear ) as LeaveYear , " +
                          "LeaveTypeId , HolidayName , replace ( convert ( varchar , HolidayDateFrom , 106 ) , ' ' , '-' ) as HolidayDateFrom , " +
                          "replace ( convert ( varchar , HolidayDateTo , 106 ) , ' ' , '-' ) as HolidayDateTo , " +
                          "convert ( varchar , IsChecked ) as IsChecked from fnGetHolidayZoneWiseHolidayList ( @id , @HolidayGroupid )");

            try
            {
                var lst = context.Database.SqlQuery<HolidayZoneWiseHolidayList>(qryStr.ToString(),sqlParameter)
                    .Select(d => new HolidayZoneWiseHolidayList()
                    {
                        HolidayId = d.HolidayId,
                        LeaveYear = d.LeaveYear,
                        LeaveTypeId = d.LeaveTypeId,
                        HolidayName = d.HolidayName,
                        HolidayDateFrom =d.HolidayDateFrom,
                        HolidayDateTo = d.HolidayDateTo,
                        IsChecked = d.IsChecked
                    }).ToList();

                if (lst == null)
                {
                    return new List<HolidayZoneWiseHolidayList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<HolidayZoneWiseHolidayList>();
            }

        }

        public List<HolidayList> GetAllHolidays()
        {
            var qryStr = new StringBuilder();

            qryStr.Clear();
            qryStr.Append("select id , name from holiday");

            try
            {
                var lst = context.Database.SqlQuery<HolidayList>( qryStr.ToString() ).Select( d => new HolidayList() {
                    Hid = d.Hid ,
                    Name = d.Name
                } ).ToList();

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
                        IsActive = d.IsActive , 
                        CreatedDate = d.CreatedDate , 
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

        public string SaveHolidayZones() {
            return null;
        }
    }
}
