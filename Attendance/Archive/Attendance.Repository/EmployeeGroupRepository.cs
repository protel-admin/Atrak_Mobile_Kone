using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Attendance.Repository;
using Attendance.Model;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using System.Data.SqlClient;

namespace Attendance.Repository
{
    public class EmployeeGroupRepository : IDisposable
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
        private class EmpGroup
        {
            public string EmployeeGroupTxnId { get; set; }
            public string EmployeeGroupId { get; set; }
        }

        AttendanceManagementContext context = null;

        public EmployeeGroupRepository()
        {
            context = new AttendanceManagementContext();
        }

        public EmployeeGroup GetGroupDetails(string id)
        {

            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@id", id);
            var qryStr = new StringBuilder();
            qryStr.Append("select * from EmployeeGroup where id = @id");
            var eg = context.Database.SqlQuery<EmployeeGroup>(qryStr.ToString(),sqlParameter).FirstOrDefault();

            if (eg == null)
            {
                eg = new EmployeeGroup();
                eg.IsActive = true;
                return eg;
            }

            return eg;
        }

        public List<EmployeeGroupView> GetAllEmployeeGroups()
        {
            var qryStr = new StringBuilder();
            qryStr.Append("select Id , Name , StaffCount , " +
                "b.isactive "+
                "from vwemployeegroup a inner join EmployeeGroup b on a.employeegroupid = b.id");

            var lst = context.Database.SqlQuery<EmployeeGroupView>(qryStr.ToString()).ToList();

            if(lst == null)
                return new List<EmployeeGroupView>();

            return lst;
        }

        public List<EmployeeGroupTxnView> GetAllEmployeesInGroup(string id)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@id", id);
            try
            {
                var qryStr = new StringBuilder ( );

                qryStr.Clear ( );

                qryStr.Append ( "select egt.id , egt.employeegroupid , sv.staffid , "+
                    "sv.firstname as staffname, isnull ( sv.deptname , '-' ) as department " +
                    "from EmployeeGroupTxn egt inner join staffview sv on egt.staffid = sv.staffid " +
                    "where egt.employeegroupid = @id and egt.isactive = 1" );

                var lst = context.Database.SqlQuery<EmployeeGroupTxnView> ( qryStr.ToString ( ),sqlParameter ).Select ( d => new EmployeeGroupTxnView ( ) {
                    Id = d.Id ,
                    EmployeeGroupId = d.EmployeeGroupId ,
                    StaffId = d.StaffId ,
                    StaffName = d.StaffName ,
                    Department = d.Department
                } ).ToList ( );

                if ( lst.Count == 0 ) {
                    return new List<EmployeeGroupTxnView> ( );
                } else {
                    return lst;
                }
            }
            catch
            {
                throw;
            }
        }

        public void SaveEmployeeGroup(ref string id, string groupname, string isactive)
        {
            MasterRepository mr = null;
            var lastid = string.Empty;
            var eg = new EmployeeGroup();
            mr = new MasterRepository();
            var mrt = new EmployeeGroupTxn();
            if (string.IsNullOrEmpty(id) == true){
                id = mr.getmaxid("EmployeeGroup", "id", "EG", "", 10, ref lastid);
                eg.Id = id;
            }
            else{
                eg.Id = id;
            }
            eg.Name = groupname;

            if ( isactive.Trim().ToUpper() == "YES")
                eg.IsActive = true;
            else if (isactive.Trim().ToUpper() == "NO")
                eg.IsActive = false;

            context.EmployeeGroup.AddOrUpdate(eg);
            context.SaveChanges();
        }

        public void SaveGroupTxns(string oldstaffs, string newstaffs, string id)
        {
            //check if staffs have to be added.
            if (oldstaffs == string.Empty && newstaffs != string.Empty) //if to be only added then...
            {
                //funciton call to add staffs to the group.
                AddStaffsToGroup(newstaffs,id);
            }
            else if (oldstaffs != string.Empty && newstaffs == string.Empty) //if staffs have to be only removed
            {
                //function call to remove staffs from the group.
                RemoveStaffsFromGroup(oldstaffs,id);
            }
            else if (oldstaffs != string.Empty && newstaffs != string.Empty) //if staffs to be both removed and added.
            {
                //function call to both add and remove from the group.
                RemoveAndAddStaffInGroup(oldstaffs, newstaffs, id);
            }
        }

        public void RemoveStaffsFromGroup(string oldstaffs, string id)
        {
            var a = oldstaffs.Split(',');
            foreach ( var b in a)
            {
                //remove a single staff to the group
                RemoveAStaffFromGroup(b, id);
                //remove shift schedule for a staff.
                RemoveShiftScheduleForAStaff(b, id);
            }
        }

        public void RemoveAStaffFromGroup(string staffid , string id)
        {
            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@staffid", staffid);
            sqlParameter[1] = new SqlParameter("@id", id);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("update EmployeeGroupTxn set isactive = 0 "+
                "where staffid = @staffid and EmployeeGroupID = @id");
            context.Database.ExecuteSqlCommand(qryStr.ToString(),sqlParameter);
        }

        public void RemoveShiftScheduleForAStaff(string staffid , string id)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];              
            sqlParameter[0] = new SqlParameter("@id", id);
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select b.* from EmployeeGroupShiftPatternTxn a "+
                "inner join ShiftPattern b on a.shiftpatternid = b.id"+
                " where a.employeegroupid = @id");

            var lst = context.Database.SqlQuery<ShiftPattern>(qryStr.ToString(),sqlParameter).ToList();

            if(lst != null) //if not null
            {
                SqlParameter[] sqlParameter2 = new SqlParameter[1];
                sqlParameter2[0] = new SqlParameter("@staffid", staffid);
                if (lst.Count > 0) //if having atleast one record.
                {
                    if (lst.FirstOrDefault().UpdatedUntil != null) //if date has been set
                    {
                        if(lst.FirstOrDefault().UpdatedUntil > DateTime.Now) //if the current date is less than updateduntil
                        {
                            //remove the shift schedule from the current date to updateduntil for the given staff.
                            qryStr.Clear();
                            qryStr.Append("delete from attendancedata "+
                                "where shiftindate > getdate() and staffid = @staffid");
                            context.Database.ExecuteSqlCommand(qryStr.ToString(), sqlParameter2);
                        }
                    }
                }
            }
        }

        public void AddStaffsToGroup(string newstaffs, string id)
        {
            var a = newstaffs.Split(',');
            var lastid = string.Empty;
            foreach(var b in a)
            {
                //add a single staff to the group.
                AddAStaffToGroup(b, id, ref lastid);
            }
            //assign shift schedule to the staff.
            AddShiftScheduleForAStaff(newstaffs, id);
        }

        public bool IfStaffPartOfOtherGroup(string staffid)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@staffid", staffid);
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append ( "select * from EmployeeGroupTxn "+
                "where staffid = @staffid and isactive = 1" );

            try
            {
                var egt = context.Database.SqlQuery<EmployeeGroupTxn> ( qryStr.ToString ( ),sqlParameter).FirstOrDefault ( );
                if (egt == null){
                    return false;}
                else{
                    return true;}
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ChangeGroupOfStaff(string staffid, string id)
        {

            SqlParameter[] sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@staffid", staffid);
            sqlParameter[1] = new SqlParameter("@id", id);
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append ( "update EmployeeGroupTxn "+
                "set employeegroupid = @id where staffid = @staffid and isactive = 1" );

            try
            {
                context.Database.ExecuteSqlCommand ( qryStr.ToString ( ),sqlParameter );
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public void AddAStaffToGroup(string staffid, string id, ref string lastid)
        {
            MasterRepository mr = new MasterRepository();
            var egt = new EmployeeGroupTxn();

            //check if the employee is belonging to another group.
            if ( IfStaffPartOfOtherGroup(staffid) == true)//if he is belonging to another group then...
            { 
                //update the new group over the old group.
                ChangeGroupOfStaff(staffid, id);
            } else //if he is not belonging to another group then...
            {
                //get max id.
                var gtid = mr.getmaxid ( "EmployeeGroupTxn" , "id" , "EGT" , "" , 10 , ref lastid );
                //insert the new association.
                var qryStr = new StringBuilder ( );

                egt.Id = gtid;
                egt.EmployeeGroupId = id;
                egt.StaffId = staffid;
                egt.IsActive = true;
                context.EmployeeGroupTxn.AddOrUpdate ( egt );
                context.SaveChanges ( );

                lastid = gtid;
            }
        }

        public void AddShiftScheduleForAStaff(string staffid , string id)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];          
            sqlParameter[0] = new SqlParameter("@id", id);

            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select b.* from EmployeeGroupShiftPatternTxn a " +
                "inner join ShiftPattern b on a.shiftpatternid = b.id" +
                " where a.employeegroupid = @id");

            var lst = context.Database.SqlQuery<ShiftPattern>(qryStr.ToString(),sqlParameter).ToList();

            if (lst != null) //if not null
            {
                if (lst.Count > 0) //if having atleast one record.
                {
                    if (lst.FirstOrDefault().UpdatedUntil != null) //if date has been set
                    {
                        if (lst.FirstOrDefault().UpdatedUntil > DateTime.Now) //if the current date is less than updateduntil
                        {
                            var repo = new ExecuteShiftPatternRepository();
                            repo.ProcessShiftPattern(DateTime.Now.AddDays(1), 
                                Convert.ToDateTime(lst.FirstOrDefault().UpdatedUntil),staffid,id);                            
                        }
                    }
                }
            }
        }

        public void RemoveAndAddStaffInGroup(string oldstaffs, string newstaffs, string id)
        {
            //check if oldstaffs is different from newstaffs
            if (oldstaffs != newstaffs) //if different then...
            {
                //
                RemoveStaffsFromGroup(oldstaffs,id);
                //
                AddStaffsToGroup(newstaffs, id);
            }
        }


        public void saveemployeetogroup(string oldstaffs, string newstaffs, string id, string groupname , string isactive)
        {
            using (var Trans = context.Database.BeginTransaction())
            {
                var qryStr = new StringBuilder();

                try
                {
                    //save employee group.
                    SaveEmployeeGroup(ref id, groupname,isactive);
                    //save group transactions.
                    SaveGroupTxns(oldstaffs, newstaffs, id);

                    Trans.Commit();
                }
                catch (Exception)
                {
                    Trans.Rollback();
                    throw;
                }
            }
        }
    }
}
