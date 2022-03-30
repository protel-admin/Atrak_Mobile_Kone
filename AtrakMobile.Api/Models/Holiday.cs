using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtrakMobileApi.Models
{
    public class HolidayCalender
    {
        public string Year { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public Holiday[] Holidays { get; set; }


        public HolidayCalender GetCalendarFor(string location, int year)
        {
            return new HolidayCalender()
            {
                LocationId = "Location1",
                LocationName = location,
                Year = year.ToString(),
                Holidays = new Holiday[]{
                    new Holiday(){Name="Diwali", Date="2018-11-05"},
                    new Holiday(){Name="Christmas", Date="2018-12-25"},
                    new Holiday(){Name="Founder Day", Date="2018-12-31"}

                }

            };
        }

    }
    public class Holiday
    {
        public string Name { get; set; }
        public string Date { get; set; }
    }

   
}