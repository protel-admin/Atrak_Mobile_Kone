using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class CalendarBusinessLogic
    {
        public List<CalendarDays> GetCalendarDays(int month, int year, string staffid)
        {
            //start date will be the 1st of the given month and the year.
            var StartDate = new DateTime(year, month, 1);

            //get the shift calendar for the given month and the year and of a given staff.
            using (var crepo = new CalendarRepository())
            { 
                var lst = crepo.GetShiftCalendar(month, year, staffid);
                var lstCD = DrawCalendar(lst, StartDate);
                return lstCD;
            }

            //bring the shift calendar into a proper calendar.
          
        }

        public List<CalendarDays> GetMyAttendanceForMobile(string staffid)
        {
            //get the shift calendar for the given month and the year and of a given staff.
            using (var crepo = new CalendarRepository())
            { 
            var lst = crepo.GetMyAttendanceForMobile(staffid);
            return lst;
            }
        }
        public List<CalendarColorModel> GetCalendarColor()
        {
            using (CalendarRepository calendarRepository = new CalendarRepository())

            { return calendarRepository.GetCalendarColor(); }
        }
        public Dictionary<int, string> GetMonths()
        {
            //get all the months.
            var dct = new Dictionary<int, string>();
            var arr = new string[12] {"January", "February","March","April",
                                        "May","June","July","August",
                                        "September","October","November","December"};
            for (int i = 0; i < arr.Length; i += 1)
                dct.Add(i + 1, arr[i]);

            return dct;
        }

        public Dictionary<int, int> GetYears()
        {
            //get previous, current and next year.
            var dct = new Dictionary<int, int>();
            var yr = 0;
            //previous year
            yr = DateTime.Now.AddYears(-1).Year;
            dct.Add(yr, yr);

            //current year
            yr = DateTime.Now.Year;
            dct.Add(yr, yr);

            //next year
            yr = DateTime.Now.AddYears(1).Year;
            dct.Add(yr, yr);

            return dct;
        }

        private List<CalendarDays> DrawCalendar(List<CalendarDays> lst, DateTime StartDate)
        {

            //DRAW A PROPER CALENDAR WITH SHIFT INDICATORS IN IT.
            var d = 0;
            var loadday = true;
            var lstCD = new List<CalendarDays>();

            //run loop for 7 days and 5 weeks i.e. 35 days.
            for (d = 1; d <= 42; d += 1)
            {
                var Cd = new CalendarDays();

                Cd.Id = d.ToString(); //assign day id.

                if (d >= (Convert.ToInt16(StartDate.DayOfWeek) + 1))
                {
                    if (loadday == true)
                    {
                        Cd.Day = StartDate.Day.ToString();

                        //find shift for a given date.
                        var c = lst.Find(a => (a.Day == Cd.Day));

                        if (c != null)
                        { //if shift found for the day then...

                            //get week number and shift shortname.
                            Cd.ActualDate = c.ActualDate;
                            Cd.WeekNumber = c.WeekNumber;
                            Cd.ShortName = c.ShortName;
                            Cd.AttendanceStatus = c.AttendanceStatus;
                            Cd.AttendanceStatusCode = c.AttendanceStatusCode;
                            Cd.FHStatusCode = c.FHStatusCode;
                            Cd.SHStatusCode = c.SHStatusCode;
                            Cd.FHStatus = c.FHStatus;
                            Cd.SHStatus = c.SHStatus;

                            if (c.ActualDate != null)
                                Cd.ActualDate = c.ActualDate;
                            else
                                Cd.ActualDate = "--";

                            //null handling of in time
                            if (c.InTime != null)
                                Cd.InTime = c.InTime;
                            else
                                Cd.InTime = "--";

                            if (c.ActualInTime != null)
                                Cd.ActualInTime = c.ActualInTime;
                            else
                                Cd.ActualInTime = "--";

                            //null handling of out time.
                            if (c.OutTime != null)
                                Cd.OutTime = c.OutTime;
                            else
                                Cd.OutTime = "--";

                            if (c.ActualOutTime != null)
                                Cd.ActualOutTime = c.ActualOutTime;
                            else
                                Cd.ActualOutTime = "--";

                            if (c.ActualWorkedHours != null)
                                Cd.ActualWorkedHours = c.ActualWorkedHours;
                            else
                                Cd.ActualWorkedHours = "--";

                            Cd.DayName = c.DayName;
                            Cd.IsCurrentDay = c.IsCurrentDay;
                            if (string.IsNullOrEmpty(c.EarlyGoing) == true)
                                Cd.EarlyGoing = "00:00";
                            else
                                Cd.EarlyGoing = c.EarlyGoing;
                            if (string.IsNullOrEmpty(c.LateComing) == true)
                                Cd.LateComing = "00:00";
                            else
                                Cd.LateComing = c.LateComing;

                        }
                        else
                        { //if shift not found then...
                            //put place holders.
                            Cd.ActualDate = "--";
                            Cd.WeekNumber = "--";
                            Cd.ShortName = "--";
                            Cd.Day = "--";
                            Cd.DayName = "--";
                            Cd.InTime = "--";
                            Cd.OutTime = "--";
                            Cd.ActualInTime = "--";
                            Cd.ActualOutTime = "--";
                            Cd.ActualWorkedHours = "--";
                            Cd.LateComing = "--";
                            Cd.EarlyGoing = "--";
                            Cd.FHStatus = "--";
                            Cd.SHStatus = "--";
                            Cd.AttendanceStatus = "--";
                        }
                    }

                    StartDate = StartDate.AddDays(1);
                    if (StartDate.Day == 1)
                        loadday = false;
                }
                else
                {
                    Cd.ActualDate = "--";
                    Cd.WeekNumber = "--";
                    Cd.ShortName = "--";
                    Cd.Day = "--";
                    Cd.DayName = "--";
                    Cd.InTime = "--";
                    Cd.OutTime = "--";
                    Cd.ActualInTime = "--";
                    Cd.ActualOutTime = "--";
                    Cd.ActualWorkedHours = "--";
                    Cd.LateComing = "--";
                    Cd.EarlyGoing = "--";
                    Cd.FHStatus = "--";
                    Cd.SHStatus = "--";
                    Cd.AttendanceStatus = "--";
                }
                lstCD.Add(Cd);
            }
            return lstCD;
        }
        public string GetCategoryById(string StaffId)
        {
            using (var Repo = new CalendarRepository())
            { 
                var Result = Repo.GetCategoryById(StaffId);
            return Result;
            }
        }
    }
}
