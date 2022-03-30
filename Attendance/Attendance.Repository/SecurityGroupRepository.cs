using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using Attendance.Model;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
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
                

                var lstoffstaff = context.Screen.Where(d=>d.ScreenOption == 1).Select(c => new MapSecurityScreentoRoleList()
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


        public List<SecurityGroupTxnsList> GetAllSecurityGroup(string staffid)
        {
            SqlParameter[] Param = new SqlParameter[1];
            Param[0] = new SqlParameter("@staffid", staffid);
            var queryString = new StringBuilder();
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("Select Id,Name,IsActive from SecurityGroup Where IsActive = 1");
            try
            {
                var lstoffstaff = context.Database.SqlQuery<SecurityGroup>(qryStr.ToString(), new SqlParameter("@staffid", staffid))
                        .Select(c => new SecurityGroupTxnsList()
                        {
                            RId = c.Id,
                            RName = c.Name,
                            IsActive = c.IsActive
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
            catch (Exception err)
            {
                return new List<SecurityGroupTxnsList>();
                throw err;
            }
        }

        public List<MapSecurityScreentoRoleList> GetAllSecurityGroupTxns(string id)
        {
            SqlParameter[] Param = new SqlParameter[1];
            Param[0] = new SqlParameter("@Id", id);
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select IsNull(sg.id,0) as Id, a.id as screenid,a.ScreenName,b.Id as RoleID, " +
                         "case when sg.Options = 1 then  cast('True' as bit) else  cast('False' as bit) end as 'Check', b.Name as RoleName " +
                         "from Screen a cross join SecurityGroup b " +
                         "left join SecurityGroupTxns sg on a.id = sg.ScreenID and b.id = sg.roleId " +
                         "where b.IsActive = 1 and b.Id = @Id ");
            try
            {
                var lstoffstaff = context.Database.SqlQuery<MapSecurityScreentoRoleList>(qryStr.ToString(),new SqlParameter ("@Id", id))
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


        public void SaveRoleGroupTxnsInfo(SecurityGroupTxns CompInfo)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {

                    context.SecurityGroupTxns.AddOrUpdate(CompInfo);
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
        #region 
        public List<ScreensModel> GetAllScreens()
        {
            try
            {
                var qryStr = new StringBuilder();
                qryStr.Clear();
                qryStr.Append("select Id,ScreenName,ScreenOption,Level,ParentID From Screen  ");
                var lstoffstaff = context.Database.SqlQuery<ScreensModel>(qryStr.ToString()).ToList();
                if (lstoffstaff == null)
                {
                    return new List<ScreensModel>();
                }
                else
                {
                    return lstoffstaff;
                }
            }
            catch (Exception e)
            {
                return new List<ScreensModel>();
                throw e;
            }
        }
        public ScreensModel GetSelectedScreenIdDetails(string Id)
        {
            try
            {
                int id =Convert.ToInt32(Id);
                var lstoffscreen = context.Screen.Where(d=>d.Id == id).Select(c => new ScreensModel()
                {
                    Id = c.Id,
                    ScreenName = c.ScreenName,
                    ScreenOption = c.ScreenOption,
                    Level = c.Level,
                    ParentID = c.ParentID
                }
                 ).FirstOrDefault();

                if (lstoffscreen == null)
                {
                    return new ScreensModel();
                }
                else
                {
                    return lstoffscreen;
                }
            }
            catch (Exception e)
            {
                return new ScreensModel();
                throw e;
            }
        }
        public string AddorUpdateScreenMaster(ScreensModel Model)
        {
            using (var Trans=context.Database.BeginTransaction())
            {
                try
                {
                    Screen Tbl = new Screen();
                    if (Model.Id !=0)
                    {
                        Tbl.Id = Model.Id;
                    }
                    Tbl.ScreenName = Model.ScreenName;
                    Tbl.ScreenOption = Model.ScreenOption;
                    Tbl.Level = Model.Level;
                    Tbl.ParentID = Model.ParentID;
                    context.Screen.AddOrUpdate(Tbl);
                    context.SaveChanges();
                    Trans.Commit();
                    return "OK";
                }
                catch (Exception e)
                {
                    throw e;
                }
            }                
        }
        #endregion

    }
}
