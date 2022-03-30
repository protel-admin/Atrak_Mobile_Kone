using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Data.Entity.Migrations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Metadata.Edm;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;


namespace Attendance.Repository
{
    public class TrackChangeRepository : IDisposable
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
        public void RecordChangeLog(AttendanceManagementContext context, string User, string TableName, string _ChangeLog, string ActionType, string PrimaryKeyValue)
        {
            ChangeAuditLog CAL = new ChangeAuditLog();
            CAL.UserId = User;
            CAL.TableName = TableName;
            CAL.ChangeLog = _ChangeLog.ToString();
            CAL.CreatedOn = DateTime.Now;
            CAL.ActionType = ActionType;
            CAL.PrimaryKeyValue = PrimaryKeyValue;
            context.ChangeAuditLog.AddOrUpdate(CAL);
            context.SaveChanges();
        }


     
        public void GetChangeLogString<T>(T _ClassObject, AttendanceManagementContext context, ref string ChangeLogString, ref string ActionType, ref string PrimaryKeyValue) where T : class
        {
            StringBuilder _ChangeLog = new StringBuilder();
            string keyName = string.Empty;

            foreach (var entry in context.ChangeTracker.Entries<T>())
            {
                if (entry.State == System.Data.Entity.EntityState.Modified)
                {
                    string FieldName = string.Empty;
                    string OldValue = string.Empty;
                    string NewValue = string.Empty;
                    string DataType = string.Empty;

                    keyName = entry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0).Name;

                    foreach (var name in entry.OriginalValues.PropertyNames)
                    {
                        FieldName = string.Empty;
                        OldValue = string.Empty;
                        NewValue = string.Empty;
                        DataType = string.Empty;
                        ActionType = "EDIT";
                        FieldName = name;

                        if (entry.OriginalValues[name] != null)
                            OldValue = entry.OriginalValues[name].ToString();

                        if (entry.CurrentValues[name] != null)
                            NewValue = entry.CurrentValues[name].ToString();

                        PrimaryKeyValue = entry.OriginalValues[keyName].ToString();

                        if (!OldValue.Equals(NewValue))
                        {
                            _ChangeLog.Append(string.Format("{0}={1}->{2} || ", FieldName, OldValue, NewValue));
                        }
                    }
                }
                else if (entry.State == System.Data.Entity.EntityState.Added)
                {
                    string FieldName = string.Empty;
                    string OldValue = string.Empty;
                    string NewValue = string.Empty;
                    string DataType = string.Empty;

                    foreach (var name in entry.CurrentValues.PropertyNames)
                    {
                        FieldName = string.Empty;
                        OldValue = string.Empty;
                        NewValue = string.Empty;
                        DataType = string.Empty;
                        ActionType = "ADD";
                        FieldName = name;
                        if (entry.CurrentValues[name] != null)
                            NewValue = entry.CurrentValues[name].ToString();
                        PrimaryKeyValue = entry.CurrentValues[keyName].ToString();
                        if (!OldValue.Equals(NewValue))
                        {
                            _ChangeLog.Append(string.Format("{0}={1} || ", FieldName, NewValue));
                        }
                    }
                }
            }

            ChangeLogString = _ChangeLog.ToString();
        }
    }
}
