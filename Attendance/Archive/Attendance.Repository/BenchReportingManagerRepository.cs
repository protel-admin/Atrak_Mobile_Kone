using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using System.Configuration;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data.Entity.Migrations;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class BenchReportingManagerRepository : IDisposable
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
        string ConStr = string.Empty;

        AttendanceManagementContext context = null;

        public BenchReportingManagerRepository()
        {
            context = new AttendanceManagementContext();
        }

        public BenchReportingManagerModel GetBenchReportingManager(int Id)
        {
            try
            {
                var data = (from a in context.BenchReportingManager
                           join b in context.Staff on a.StaffId equals b.Id
                           join c in context.StaffOfficial on a.StaffId equals c.StaffId
                           join d in context.Department on c.DepartmentId equals d.Id where a.Id.Equals(Id)
                           select new
                           {
                               Id = a.Id,
                               StaffId = b.Id,
                               StaffName = b.FirstName,
                               Department = d.Name,
                               IsActive = a.IsActive,
                               CreatedOn = a.CreatedOn,
                               CreatedBy = a.CreatedBy
                           }).FirstOrDefault();

                BenchReportingManagerModel model = new BenchReportingManagerModel();
                model.Id = data.Id;
                model.StaffId = data.StaffId;
                model.StaffName = data.StaffName;
                model.Department = data.Department;
                model.IsActive = data.IsActive;
                model.CreatedOn = data.CreatedOn.ToString();
                model.CreatedBy = data.CreatedBy;

                return model;
            }
            catch
            {
                throw;
            }

            return null;
        }

        public List<BenchReportingManagerDataTableModel> LoadDataTable()
        {
            List<BenchReportingManagerDataTableModel> _LST_ = null;

            try
            {
                var lst = (from a in context.BenchReportingManager
                           join b in context.Staff on a.StaffId equals b.Id
                           join c in context.StaffOfficial on a.StaffId equals c.StaffId
                           join d in context.Department on c.DepartmentId equals d.Id
                           select new
                           {
                               Id = a.Id,
                               StaffId = b.Id,
                               StaffName = b.FirstName,
                               Department = d.Name,
                               IsActive = a.IsActive
                           }).ToList();

                if(lst.Count > 0)
                {
                    _LST_ = new List<BenchReportingManagerDataTableModel>();
                    foreach (var l in lst)
                    {
                        _LST_.Add(new BenchReportingManagerDataTableModel
                        {
                            Id = l.Id.ToString(),
                            StaffId = l.StaffId,
                            StaffName = l.StaffName,
                            Department = l.Department,
                            IsActive = l.IsActive
                        });
                    }
                    return _LST_;
                }
            }
            catch
            {
                throw;
            }

            return null;
        }

        public void SaveChanges(BenchReportingManager model)
        {
            context.BenchReportingManager.AddOrUpdate(model);
            context.SaveChanges();
        }

        //public List<AttachDetachStaffList> GetStaffListToDetach(string ReportingManager)
        //{
        //    var _LST_ = (from a in context.StaffOfficial
        //                 join b in context.Staff on a.StaffId equals b.Id
        //                 join c in context.Department on a.DepartmentId equals c.Id
        //                 join d in context.Designation on a.DesignationId equals d.Id
        //                 where a.ReportingManager.Equals(ReportingManager)
        //                 select new
        //                 {
        //                     StaffId = a.StaffId,
        //                     StaffName = b.FirstName,
        //                     DepartmentId = a.DepartmentId,
        //                     DepartmentName = c.Name,
        //                     DesignationId = a.DesignationId,
        //                     DesignationName = d.Name
        //                 }).ToList();
        //    if(_LST_ != null)
        //    {
        //        if(_LST_.Count > 0)
        //        {
        //            List<AttachDetachStaffList> LST = new List<AttachDetachStaffList>();
        //            foreach (var l in _LST_)
        //            {
        //                LST.Add(new AttachDetachStaffList
        //                {
        //                    Checked = true,
        //                    StaffId = l.StaffId,
        //                    StaffName = l.StaffName,
        //                    DepartmentId = l.DepartmentId,
        //                    DepartmentName = l.DepartmentName,
        //                    DesignationId = l.DesignationId,
        //                    DesignationName = l.DesignationName
        //                });
        //            }

        //            return LST;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //    return null;
        //}


        public List<AttachDetachList> GetStaffListToDetach(string ReportingManager)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@ReportingManager", ReportingManager);
            StringBuilder QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append(string.Format("SELECT A.Id as StaffId, [DBO].[fnGetStaffName](A.Id) as StaffName FROM Staff A INNER JOIN StaffOfficial B on A.Id = B.StaffId WHERE B.REPORTINGMANAGER = @ReportingManager"));
            var _LST_ = context.Database.SqlQuery<AttachDetachList>(QryStr.ToString(), sqlParameter).Select(d => new AttachDetachList
            {
                StaffId = d.StaffId,
                StaffName = d.StaffName
            }).ToList();
            return _LST_;
        }

        public void DetachEmployees(AttachDetach model)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
            
            foreach (var l in model.AttachDetachList)
            {
                sqlParameter[0] = new SqlParameter("@StaffId", l.StaffId);
                if (l.Checked == true)
                {
                    using (var Trans = context.Database.BeginTransaction())
                    {
                        try
                        {

                            //
                            //context.Database.ExecuteSqlCommand("update staff set IsAttached = 0 where Id = '{0}'", l.StaffId);
                            context.Database.ExecuteSqlCommand("update staff set IsAttached = 0 where Id = @StaffId");
                            //
                            var NewReportingManager = context.Database.SqlQuery<string>("select A.StaffId from benchreportingmanager a inner join staffofficial b on a.Staffid = b.StaffId inner join StaffOfficial C on B.DepartmentID = C.DepartmentId where C.StaffId = @StaffId").FirstOrDefault();
                            //
                            sqlParameter[1] = new SqlParameter("@NewReportingManager", NewReportingManager ?? "");

                            if (string.IsNullOrEmpty(NewReportingManager) == false)
                                context.Database.ExecuteSqlCommand("update staffofficial set ReportingManager = @NewReportingManager where StaffId = @StaffId");
                            else
                                context.Database.ExecuteSqlCommand("update staffofficial set ReportingManager = null where StaffId = @StaffId");

                            //
                            context.Database.ExecuteSqlCommand("insert into AttachDetachLog ( StaffId , IsAttached, ReportingManager, StateChangedOn ) values  ( @StaffId , 1, @NewReportingManager, GetDate() ) ");
                            //
                            Trans.Commit();
                        }
                        catch
                        {
                            //
                            Trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        public List<AttachDetachList> GetStaffListFromCommonPool(string ReportingManager)
        {
            StringBuilder QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append(string.Format("SELECT Id as StaffId, [DBO].[fnGetStaffName](Id) as StaffName FROM STAFF WHERE IsAttached = 0"));
            var _LST_ = context.Database.SqlQuery<AttachDetachList>(QryStr.ToString()).Select(d => new AttachDetachList
            {
                StaffId = d.StaffId,
                StaffName = d.StaffName
            }).ToList();
            return _LST_;
        }

        public void AttachEmployees(AttachDetach model)
        {
            foreach (var l in model.AttachDetachList)
            {
                if (l.Checked == true)
                {
                    using (var Trans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //
                            //context.Database.ExecuteSqlCommand("update staff set IsAttached = 0 where Id = '{0}'", l.StaffId);
                            context.Database.ExecuteSqlCommand("update staff set IsAttached = 1 where Id = '" + l.StaffId + "'");
                            //
                            context.Database.ExecuteSqlCommand("update staffofficial set ReportingManager = '" + model.StaffId + "' where StaffId = '" + l.StaffId + "'");
                            //
                            context.Database.ExecuteSqlCommand("insert into AttachDetachLog ( StaffId , IsAttached, ReportingManager, StateChangedOn ) values  ( {0} , 1, {1}, GetDate() ) ", l.StaffId, model.StaffId);
                            //
                            Trans.Commit();
                        }
                        catch
                        {
                            //
                            Trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }
    }
}
