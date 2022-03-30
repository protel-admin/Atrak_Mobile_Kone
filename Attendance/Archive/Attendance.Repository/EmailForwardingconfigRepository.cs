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
using System.Data.SqlClient;

namespace Attendance.Repository
{
    public class EmailForwardingconfigRepository : IDisposable
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
        AttendanceManagementContext context = null;
        public EmailForwardingconfigRepository()
        {
            context = new AttendanceManagementContext();
        }
        StringBuilder builder = new StringBuilder();
        string Message = string.Empty;

        public string SaveEmailForwardData(Emailforwordconfigmodel model)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    EmailForwardingconfig tbl = new EmailForwardingconfig();
                    if (model.Id != 0)
                    {
                        tbl.Id = model.Id;
                    }
                    tbl.ScreenID = model.ScreenID;
                    tbl.Fromaddress = model.Fromaddress;
                    tbl.Toaddress = model.Toaddress;
                    tbl.CCaddress = model.CCaddress;
                    tbl.LocationId = model.LocationId;
                    // tbl.CreatedOn = DateTime.Now;
                    // tbl.CreatedBy = model.CreatedBy;
                    tbl.ModifiedOn = DateTime.Now;
                    tbl.ModifiedBy = model.ModifiedBy;
                    context.EmailForwardingconfig.AddOrUpdate(tbl);
                    context.SaveChanges();
                    trans.Commit();
                    Message = "Success";
                }

                catch (Exception e)
                {
                    Message = e.Message;
                    trans.Rollback();
                    throw;
                }
            }
            return Message;
        }
        public List<Getemailforwaordconfigmodel> GetDetails()
        {
            List<Getemailforwaordconfigmodel> lst = new List<Getemailforwaordconfigmodel>();
            try
            {
                builder.Clear();
                builder.Append("select  ID,ScreenId,LocationId,(select name from Location where id=LocationId) as LocationName from EmailForwardingconfig");
                lst = context.Database.SqlQuery<Getemailforwaordconfigmodel>(builder.ToString()).ToList();
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return lst;
        }
        public Getemailforwaordconfigmodel Edit(string ID)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@ID", ID);
            Getemailforwaordconfigmodel lst = new Getemailforwaordconfigmodel();
            try
            {
                builder.Clear();
                builder.Append("select ID,ScreenId,ScreenId as ScreenName,LocationId,(select name from Location where id=locationid) as LocationName,Fromaddress as Fromadd, toaddress as Toadd ,CCaddress as CCadd from EmailForwardingconfig where Id= @ID");
                lst = context.Database.SqlQuery<Getemailforwaordconfigmodel>(builder.ToString(), sqlParameter).FirstOrDefault();
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return lst;
        }

        public List<LeaveTypeListForHoliday> GetLeaveTypesForHolidayMaster()
        {
            var queryString = new StringBuilder();
            queryString.Clear();
            queryString.Append(" Select Id , Name from LeaveType where Id in ('LV0001','LV0014','LV0015','LV0013')");
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

        public List<LeaveTypeListForHoliday> GetLocationList()
        {
            var querystring = new StringBuilder();
            querystring.Clear();
            querystring.Append("Select Id , Name from Location");
            try
            {
                var lst = context.Database.SqlQuery<LeaveTypeListForHoliday>(querystring.ToString()).Select(d => new LeaveTypeListForHoliday()
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
    }
}