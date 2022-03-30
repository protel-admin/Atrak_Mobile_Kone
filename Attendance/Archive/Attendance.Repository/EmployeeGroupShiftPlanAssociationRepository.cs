using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository {
    public class EmployeeGroupShiftPlanAssociationRepository : IDisposable
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
        private AttendanceManagementContext context = null;

        public EmployeeGroupShiftPlanAssociationRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<AssociateEmployeeGroupShiftPlan> LoadAssociations()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append( "SELECT convert ( varchar ,A.[Id]) AS EmployeeGroupId ,A.[Name] AS EmployeeGroupName " +
                          ",convert ( varchar ,b.Id ) as Id ,convert ( varchar ,ISNULL ( C.[Id] ,0 )) AS ShiftPlanId ,ISNULL ( C.[Name] ,'-') AS [ShiftPlan] " +
                          ",CASE WHEN B.[IsActive] = 1 THEN 'Yes' WHEN B.[IsActive] = 0 THEN 'No' ELSE '-' END  AS IsActive " +
                          "FROM EmployeeGroup a LEFT JOIN EmployeeGroupShiftPatternTxn b on A.Id = b.EmployeeGroupId " +
                          "LEFT JOIN ShiftPattern C on B.ShiftPatternId = C.Id");

            try
            {
                var lst =
                    context.Database.SqlQuery<AssociateEmployeeGroupShiftPlan>(qryStr.ToString())
                        .Select(d => new AssociateEmployeeGroupShiftPlan()
                        {
                            EmployeeGroupId = d.EmployeeGroupId,
                            EmployeeGroupName = d.EmployeeGroupName,
                            Id = d.Id,
                            ShiftPlanId = d.ShiftPlanId,
                            ShiftPlan = d.ShiftPlan,
                            IsActive = d.IsActive
                        }).ToList();

                if (lst == null)
                {
                    return new List<AssociateEmployeeGroupShiftPlan>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<AssociateEmployeeGroupShiftPlan>();
            }

        }

        public List<ShiftPatternNewList> LoadShiftPatterns()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("SELECT CONVERT ( VARCHAR ,Id ) AS Id ,[Name] FROM [ShiftPattern] WHERE [IsActive] = 1");

            try
            {
                var lst =
                    context.Database.SqlQuery<ShiftPatternNewList>(qryStr.ToString())
                        .Select(d => new ShiftPatternNewList()
                        {
                            Id = d.Id,
                            Name = d.Name
                        }).ToList();

                if (lst == null)
                {
                    return new List<ShiftPatternNewList>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<ShiftPatternNewList>();
            }
        }

        public void SaveInformation( string Id , string EmployeeGroupId , string ShiftPatternId , string IsActive )
        {
            var data = new EmployeeGroupShiftPatternTxn();

            if (string.IsNullOrEmpty(Id) == false && Id != "null"){
                data.Id = Convert.ToInt16(Id);}

            data.EmployeeGroupId = EmployeeGroupId;
            data.ShiftPatternId = Convert.ToInt16(ShiftPatternId);

            if (IsActive == "Yes"){
                data.IsActive = true;}
            else {
                data.IsActive = false;}

            context.EmployeeGroupShiftPatternTxn.AddOrUpdate(data);
            context.SaveChanges();
        }
    }
}
