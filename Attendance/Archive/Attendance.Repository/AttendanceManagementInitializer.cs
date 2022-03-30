using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Attendance.Model;


namespace Attendance.Repository
{
    public class AttendanceManagementContextInitializer:DropCreateDatabaseIfModelChanges<AttendanceManagementContext>
    {
        protected override void Seed(AttendanceManagementContext context)
        {
            var LeaveType = new LeaveType(){Id = "LV0001", Name = "National Holiday", ShortName = "NH",
                                            IsAccountable = true,
                                            IsEncashable = false,
                                            IsPaidLeave = true,
                                            CarryForward = false,
                                            IsActive = true};
            context.LeaveType.Add(LeaveType);
            context.SaveChanges();
            
            base.Seed(context);
        }
    }
}