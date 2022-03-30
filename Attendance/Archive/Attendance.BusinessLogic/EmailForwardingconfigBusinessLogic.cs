using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class EmailForwardingconfigBusinessLogic
    {
       // EmailForwardingconfigRepository objREPO = new EmailForwardingconfigRepository();

        public string SaveDetails(Emailforwordconfigmodel model)
        {
            using (EmailForwardingconfigRepository emailForwardingconfigRepository = new EmailForwardingconfigRepository())
            {
                return emailForwardingconfigRepository.SaveEmailForwardData(model);
            }
        }
        public List<Getemailforwaordconfigmodel> GetDetails()
        {
            List<Getemailforwaordconfigmodel> getemailforwaordconfigmodels = new List<Getemailforwaordconfigmodel>();

            using (EmailForwardingconfigRepository emailForwardingconfigRepository = new EmailForwardingconfigRepository())
            {
                getemailforwaordconfigmodels =  emailForwardingconfigRepository.GetDetails();
            }
            if(getemailforwaordconfigmodels.Count > 0)
            {
                return getemailforwaordconfigmodels;
            }
            else
            {
                return new List<Getemailforwaordconfigmodel>();
            }
        }
        public Getemailforwaordconfigmodel Edit(string ID)
        {
            using (EmailForwardingconfigRepository emailForwardingconfigRepository = new EmailForwardingconfigRepository())
            {
                return emailForwardingconfigRepository.Edit(ID);
            }
        }

        public List<LeaveTypeListForHoliday> GetLeaveTypesForHolidayMaster()
        {
            List<LeaveTypeListForHoliday> leaveTypeListForHolidays = new List<LeaveTypeListForHoliday>();

            using (EmailForwardingconfigRepository emailForwardingconfigRepository = new EmailForwardingconfigRepository())
            {
                leaveTypeListForHolidays =  emailForwardingconfigRepository.GetLeaveTypesForHolidayMaster();
            }
            if (leaveTypeListForHolidays.Count > 0)
            {
                return leaveTypeListForHolidays;
            }
            else
            {
                return new List<LeaveTypeListForHoliday>();
            }

        }

        public List<LeaveTypeListForHoliday> GetLocationList()
        {
            List<LeaveTypeListForHoliday> leaveTypeListForHolidays = new List<LeaveTypeListForHoliday>();

            using (EmailForwardingconfigRepository emailForwardingconfigRepository = new EmailForwardingconfigRepository())
            {
                leaveTypeListForHolidays = emailForwardingconfigRepository.GetLocationList();
            }
            if (leaveTypeListForHolidays.Count > 0)
            {
                return leaveTypeListForHolidays;
            }
            else
            {
                return new List<LeaveTypeListForHoliday>();
            }
        }
    }
}
