using System;
using System.Data.SqlClient;
using System.Text;

namespace Attendance.Repository
{
    public class AttendanceReaderConfigRepository
    {
        public AttendanceManagementContext context = null;
        public AttendanceReaderConfigRepository()
        {
            context = new AttendanceManagementContext();
        }
        public void SaveReaderDetails(string Name, string IpAddress, string ReaderType, string ReaderPurpose)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    SqlParameter Param1 = new SqlParameter("@Name", Name);
                    SqlParameter Param2 = new SqlParameter("@IpAddress", IpAddress);
                    SqlParameter Param3 = new SqlParameter("@ReaderType", ReaderType);
                    SqlParameter Param4 = new SqlParameter("@IpAddress1", IpAddress);

                    var Build = new StringBuilder();
                    var Build1 = new StringBuilder();
                    Build.Clear();
                    Build1.Clear();
                    if (ReaderPurpose.Equals("IsAccessReader"))
                    {
                        Build.Append("Insert into SmaxReaders values(@IpAddress,@Name,@ReaderType)");
                        context.Database.ExecuteSqlCommand(Build.ToString(), Param2, Param1, Param3);
                    }
                    else if (ReaderPurpose.Equals("IsAttendanceReader"))
                    {
                        Build.Append("Insert into SmaxReaders values(@IpAddress,@Name,@ReaderType)");
                        Build1.Append("Insert into AttendanceReaders values(@IpAddress1)");
                        context.Database.ExecuteSqlCommand(Build.ToString(), Param2, Param1, Param3);
                        context.Database.ExecuteSqlCommand(Build1.ToString(), Param4);
                    }
                    trans.Commit();
                }
                catch (Exception err)
                {
                    trans.Rollback();
                    throw err;
                }
            }
        }
    }
}
