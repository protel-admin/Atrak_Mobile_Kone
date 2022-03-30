using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using Attendance.Model;


namespace Attendance.Repository
{
    public class SecurityGroupRepository
    {
        private AttendanceManagementContext context;

        public SecurityGroupRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<MapSecurityScreentoRoleList> GetAllRoleGroup()
        {
            //var qryStr = new StringBuilder();
            //qryStr.Clear();
            //qryStr.Append("select Id , Name , ShortName , " +
            //"case when IsActive = 1 then 'Yes' else 'No' end as IsActive " +
            //"from grade");

            try
            {
                //var lstComp = context.RoleGroupTxns.Select(c => new RoleGroupTxnsList()
                ////var lstComp = context.Database.SqlQuery<GradeList>(qryStr.ToString()).Select(c => new GradeList()
                //{
                //    Id = (c.Id).ToString(),
                //    RoleID = (c.RoleID).ToString(),
                //    ScreenID = (c.ScreenID).ToString(),
                //    Options = c.Options
                //}
                //).ToList();

                //var lststaff = context.Role.Select(c => new RoleGroupTxnsList()
                //{
                //    RId=c.Id,
                //    Name = c.Name,
                //    IsActive = true
                //}
                //).ToList();

                var lstoffstaff = context.Screen.Select(c => new MapSecurityScreentoRoleList()
                {
                    //SId=c.Id,
                    //ScreenName=c.ScreenName,
                    //ScreenOption=(c.ScreenOption).ToString(),
                    //ParentId=(c.ParentID).ToString(),
                    //Level=(c.Level).ToString()
                    screenid = c.Id,
                    ScreenName = c.ScreenName,
                    Check = false

                }
                ).ToList();


                //var lst = lstComp.Union(lststaff).ToList();
                //var lst1 = lst.Union(lstoffstaff).ToList();

                //if (lst1 == null)
                //{
                //    return new List<RoleGroupTxnsList>();
                //}
                //else
                //{
                //    return lst1;
                //}



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
            if (id == "-")
            {

                qryStr.Append("select convert(varchar,sgt.Id) as Id, convert(varchar,scr.Id) as ScreenId,sg.id as Roleid, sg.Name, scr.ScreenName, convert(varchar, sgt.Options) as Options,convert(varchar, scr.Level) as Level, convert(varchar,scr.parentid) as ParentId,convert(varchar,scr.screenoption) as ScreenOption from securitygrouptxns sgt inner join " +
                                " securitygroup sg on sgt.roleid = sg.id inner join screen scr " +
                                "on sgt.screenid = scr.id where sgt.roleid= sg.id");
            }
            else
            {
                //qryStr.Append("select convert(varchar,sgt.Id) as id,convert(varchar,scr.Id) as ScreenId,sg.id as Roleid, sg.Name, scr.ScreenName,convert(varchar, sgt.Options) as Options,convert(varchar, scr.Level) as Level, convert(varchar,scr.parentid) as ParentId,convert(varchar,scr.screenoption) as ScreenOption  from securitygrouptxns sgt inner join " +
                //                " securitygroup sg on sgt.roleid = sg.id inner join screen scr " +
                //                "on sgt.screenid = scr.id where sgt.roleid= '" + id + "'");

                qryStr.Append("select sg.Id, s.Id as screenid ,s.ScreenName,sg.RoleID,case when sg.Options=1 then  cast('True' as bit) else  cast('False' as bit) end as 'Check',sgr.Name as RoleName from Screen s left join SecurityGroupTxns sg on s.id=sg.ScreenID join securitygroup as sgr on  sg.roleid=sgr.id where sg.RoleID='" + id + "'");
            }

            try
            {
                var lstoffstaff = context.Database.SqlQuery<MapSecurityScreentoRoleList>(qryStr.ToString())
                        .Select(c => new MapSecurityScreentoRoleList()
                        {

                            //Id =  c.Id,
                            //RName = c.Name,
                            //SId = Convert.ToInt16(c.ScreenID),
                            //ScreenName = c.ScreenName,
                            //ScreenOption = c.ScreenOption,
                            //Options=c.Options,
                            //ParentId = c.ParentId,
                            //Level = c.Level
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

                    //if (string.IsNullOrEmpty(CompInfo.Id))
                    //{
                    //    string maxid = getmaxid("rolegrouptxns", "id", "rg", "", 6);
                    //    CompInfo.Id = maxid;
                    //}

                    //var lastid = string.Empty;
                    //if (string.IsNullOrEmpty((CompInfo.Id).ToString()) == true)
                    //{
                    //    var mr = new CommonRepository();
                    //    lastid = mr.getmaxid("securitygrouptxns", "Id", "", "", 6, ref lastid);
                    //    CompInfo.Id = Convert.ToInt16(lastid);
                    //}


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
                    //var lastid = string.Empty;
                    //if ((string.IsNullOrEmpty((CompInfo.Id).ToString()) == true) || ((CompInfo.Id).ToString() == "0") )
                    //{
                    //    var mr = new CommonRepository();
                    //    lastid = mr.getmaxid("securitygroup", "Id", "", "", 10, ref lastid);
                    //    CompInfo.Id = Convert.ToInt16(lastid);
                    //}

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
                    //if (string.IsNullOrEmpty(CompInfo.Id))
                    //{
                    //    string maxid = getmaxid("staff", "id", "st", "", 6);
                    //    CompInfo.Id = maxid;
                    //}
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
