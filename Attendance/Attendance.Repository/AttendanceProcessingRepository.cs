using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Configuration;

namespace Attendance.Repository
{
    public class AttendanceProcessingRepository
    {
         private AttendanceManagementContext context = null;

         public AttendanceProcessingRepository()
        {
            context = new AttendanceManagementContext();
        }

         //public string GenerateAttendance(string StaffId, DateTime? FromDate, DateTime? ToDate)
         //{
         //    var qryStr = new StringBuilder();
         //    qryStr.Clear();
         //    qryStr.Append("exec [GenerateAttendance] '" + StaffId + "','" + FromDate + "','" + ToDate + "'");
         //    var str = (context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault()).ToString();
         //    return str;
         //}

        public List<ShiftList1> GetDurationList()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id , Name , isactive from Shifts where isactive = 1");
            try
            {
                var lst = context.Database.SqlQuery<ShiftList1>(qryStr.ToString()).Select(d => new ShiftList1()
                {
                    Id = d.Id,
                    Name = d.Name,
                    isactive = d.isactive
                }).ToList();

                if (lst == null)
                {
                    return new List<ShiftList1>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ShiftList1>();
            }
        }

        public string ProcessAttendance(string StaffId, string FromDate, string ToDate)
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append(" exec [AttendanceProcessingV1A] '" + StaffId + "','" + FromDate + "','" + ToDate + "' ");
            try
            {
                var Result = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault().ToString();

                return Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ProcessBacklogAttendance()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append(" exec [BackLogAttendanceProcessing]  ");
            try
            {
                var Result = context.Database.SqlQuery<string>(qryStr.ToString()).FirstOrDefault().ToString();

                return Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //self
        public string SaveEmpAttendanceProcess(string StaffId, string FromDate, string ToDate,string CreatedById)
        {
            string Msg = "";
            var StaffList = StaffId.Split(',');
            int StaffListLength = StaffList.Length;

            using (var Trans = context.Database.BeginTransaction())
            {
                try
                {
                    AttendanceControlTable Tbl = new AttendanceControlTable();
                    for (int i = 0; i < StaffListLength; i++)
                    {
                        Tbl.StaffId = StaffList[i];
                        Tbl.FromDate = Convert.ToDateTime(FromDate).Date;
                        Tbl.ToDate = Convert.ToDateTime(ToDate).Date;
                        Tbl.IsProcessed = false;
                        Tbl.CreatedOn = DateTime.Now;
                        Tbl.CreatedBy = CreatedById;
                        Tbl.ApplicationType = "BDAP";
                        Tbl.ApplicationId = null;
                        context.AttendanceControlTable.Add(Tbl);
                        context.SaveChanges();
                    }
                    Trans.Commit();
                    Msg = "OK";

                }
                catch (Exception e)
                {
                    throw e;
                }
                return Msg;
            }
        }

    }
}
