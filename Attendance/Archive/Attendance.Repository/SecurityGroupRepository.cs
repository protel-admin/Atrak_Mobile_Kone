using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using System.Data.SqlClient;
using System.Data.Entity.Migrations;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;


namespace Attendance.Repository
{
    public class SecurityGroupRepository : IDisposable
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
        private AttendanceManagementContext context;

        public SecurityGroupRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<MapSecurityScreentoRoleList> GetAllRoleGroup()
        {
            
            try
            {
                

                var lstoffstaff = context.Screen.Select(c => new MapSecurityScreentoRoleList()
                {
                    
                    screenid = c.Id,
                    ScreenName = c.ScreenName,
                    Check = false

                }
                ).ToList();

                if (lstoffstaff == null)
                {
                    return new List<MapSecurityScreentoRoleList>();
                }
                else
                {
                    return lstoffstaff;
                }
            }
            catch (Exception)
            {
                return new List<MapSecurityScreentoRoleList>();
                throw;
            }
        }


        public List<SecurityGroupTxnsList> GetAllSecurityOption()
        {

            try
            {
                var lstoffstaff = context.Screen.Select(c => new SecurityGroupTxnsList()
                {
                    SId = c.Id,
                    ScreenName = c.ScreenName,
                    ScreenOption = (c.ScreenOption).ToString(),
                    ParentId = (c.ParentID).ToString(),
                    Level = (c.Level).ToString()

                }
                ).ToList();


                if (lstoffstaff == null)
                {
                    return new List<SecurityGroupTxnsList>();
                }
                else
                {
                    return lstoffstaff;
                }
            }
            catch (Exception)
            {
                return new List<SecurityGroupTxnsList>();
                throw;
            }
        }


        public List<SecurityGroupTxnsList> GetAllSecurityGroup()
        {
            try
            {
                var lstoffstaff = context.SecurityGroup.Select(c => new SecurityGroupTxnsList()
                {
                    RId = c.Id,
                    RName = c.Name,
                    IsActive = true
                }
                 ).ToList();

                if (lstoffstaff == null)
                {
                    return new List<SecurityGroupTxnsList>();
                }
                else
                {
                    return lstoffstaff;
                }
            }
            catch (Exception)
            {
                return new List<SecurityGroupTxnsList>();
                throw;
            }
        }

        public List<MapSecurityScreentoRoleList> GetAllSecurityGroupTxns(string id)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select IsNull(sg.id,0) as Id, a.id as screenid,a.ScreenName,b.Id as RoleID, "+
                            "case when sg.Options = 1 then  cast('True' as bit) else  cast('False' as bit) end as 'Check', b.Name as RoleName "+
                            "from Screen a cross join SecurityGroup b "+
                            "left join SecurityGroupTxns sg on a.id = sg.ScreenID and b.id = sg.roleId "+
                            "where b.IsActive = 1 and b.Id = @Id ");
            try
            {
                var lstoffstaff = context.Database.SqlQuery<MapSecurityScreentoRoleList>(qryStr.ToString(),new SqlParameter("@Id",id))
                        .Select(c => new MapSecurityScreentoRoleList()
                        {

                            Id =  c.Id,
                            screenid = c.screenid,
                            ScreenName = c.ScreenName,
                            Roleid = c.Roleid,
                            Check = c.Check,
                            RoleName = c.RoleName

                        }
                 ).ToList();

                if (lstoffstaff == null)
                {
                    return new List<MapSecurityScreentoRoleList>();
                }
                else
                {
                    return lstoffstaff;
                }
            }
            catch (Exception)
            {
                return new List<MapSecurityScreentoRoleList>();
                throw;
            }
        }


        public void SaveRoleGroupTxnsInfo(SecurityGroupTxns CompInfo,string operationMode)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    if(operationMode == "add")
                    {
                        context.SecurityGroupTxns.Add(CompInfo);
                    }
                    else
                    {
                        context.SecurityGroupTxns.AddOrUpdate(CompInfo);
                    }
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public string SaveRoleInfos(SecurityGroup CompInfo)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                   
                    context.SecurityGroup.AddOrUpdate(CompInfo);
                    context.SaveChanges();
                    trans.Commit();
                    var lastId = CompInfo.Id;
                    return (lastId).ToString();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }



        public void SaveScreenInfo(Screen CompInfo)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    
                    context.Screen.AddOrUpdate(CompInfo);
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public List<SecurityGroup> LoadRoleId()
        {
            //fetch all leave durations from the master and send back.
            try
            {
                var ctx = new AttendanceManagementContext();
                var lstRoleId = ctx.SecurityGroup.Where(ld => ld.IsActive == true).ToList();
                return lstRoleId;
            }
            catch (Exception)
            {
                return new List<SecurityGroup>();
            }
        }

    }
}
