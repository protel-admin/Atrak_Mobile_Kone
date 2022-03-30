using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    public class AtrakConfiguration
    {
        public string Id { get; set; }
        public string Theme { get; set; }
        public string LogoUrl { get; set; }
        public string PointingUrl { get; set; }
        public CalendarCode[] CalendarCodes { get; set; }
        public MenuOption[] MenuOptions { get; set; }
        public string[] Locations { get; set; }

        public AtrakConfiguration GetConfigurationFor(string CompanyId)
        {
            return new AtrakConfiguration()
            {
                Id = "1001",
                Theme = "#4286f4",
                LogoUrl = "http://127.0.0.1/logo?compid=1002",
                PointingUrl = "http://127.0.0.21/api",
                CalendarCodes = new CalendarCode[] {
                                new CalendarCode(){ Status="Present",   Code="#4286f4"},
                                new CalendarCode(){ Status="Absent",    Code="#f47441"},
                                new CalendarCode(){ Status="WeekOff",   Code="#f441be"},
                                new CalendarCode(){ Status="Halfday",   Code="#4286f4"},
                                new CalendarCode(){ Status="Leave",     Code="#41f4d9"},
                                new CalendarCode(){ Status="Holiday",   Code="#41f4d2"}
                            },
                Locations = new string [] {"Chennai","Mumbai","Vizag"},
                MenuOptions = new MenuOption []
                            {
                                new MenuOption(){ Module="Attendance", Path="/api/Att"},
                                new MenuOption(){ Module="Payroll", Path="/api/Pay"},
                                new MenuOption(){ Module="QMS", Path="/api/Qms"},
                                new MenuOption(){ Module="Canteen", Path="/api/Can"}


                            }

            };
        }
    }
    public class CalendarCode
    {
        public string Status { get; set; }
        public string Code { get; set; }
    }

    public class MenuOption
    {
        public string Module { get; set; }
        public string Path { get; set; }
    }
}