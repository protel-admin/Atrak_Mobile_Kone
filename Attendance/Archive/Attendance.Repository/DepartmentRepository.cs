using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Data.Entity.Migrations;



namespace Attendance.Repository
{
    public class DepartmentRepository : TrackChangeRepository
    {
        
        AttendanceManagementContext context = null;

        public DepartmentRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<DepartmentList> GetAllDepartments()
        {
            var qryStr = new StringBuilder();
            qryStr.Append("select * from Department Order By Name Asc");

            try
            {
                var lstDept = context.Database.SqlQuery<DepartmentList>(qryStr.ToString()).Select(c => new DepartmentList()
                {
                    Id = c.Id,
                    Name = c.Name,
                    ShortName = c.ShortName,
                    Phone = c.Phone,
                    Fax = c.Fax,
                    Email = c.Email,
                    IsActive = c.IsActive,
                    CreatedOn = c.CreatedOn,
                    CreatedBy = c.CreatedBy
                }
                ).ToList();

                if (lstDept == null)
                {
                    return new List<DepartmentList>();
                }
                else
                {
                    return lstDept;
                }
            }
            catch (Exception)
            {
                return new List<DepartmentList>();
            }
        }

        //public void SaveDepartmentInfo(Department dept)
        //{
        //    using (var trans = context.Database.BeginTransaction())
        //    {
        //        var lastId = string.Empty;
        //        string operationMode = string.Empty;
        //           string CreatedBy =  string.Empty;
        //        try
        //        {
        //            if (string.IsNullOrEmpty(dept.CreatedBy) == true)
        //            {
        //                dept.CreatedBy = "Admin";
        //            }
        //            if (string.IsNullOrEmpty(dept.Id))
        //            {
        //                var mr = new MasterRepository();
        //                string lastid = mr.getmaxid("department", "id", "DP", "", 6, ref lastId);
        //                dept.Id = lastid;
        //                dept.ModifiedOn = null;
        //                dept.ModifiedBy = "-";
        //                var repo = new MasterRepository();
        //                operationMode = "add";
        //               CreatedBy = dept.CreatedBy;
        //                repo.AddOrUpdateInformation(dept, context, operationMode , CreatedBy) ;
        //            }
        //            else
        //            {
        //                var repo = new MasterRepository();
        //                operationMode = "edit";
        //                   CreatedBy = dept.CreatedBy;
        //                repo.AddOrUpdateInformation(dept, context, operationMode , CreatedBy);
        //            }

        //            trans.Commit();
        //        }
        //        catch (Exception)
        //        {
        //            trans.Rollback();
        //            throw;
        //        }
        //    }
        //}

    }
}
