using AtrakMobileApi.Helpers;
using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using Attendance.Model;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
//using AtrakMobileApi.ExceptionLog;
using Newtonsoft.Json;
using AtrakMobileApi.Logging;

namespace AtrakMobileApi.Controllers
{
    /// <summary>
    /// Provides API's for the dashboard for the logged in user.
    /// </summary>
    /// 
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : ApiController
    {
        /// <summary>
        /// Get the list of approvals pending from the reportees of the logged in user. 
        /// Requires staffid of the logged in user as input parameter
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns>A JSON object with list of approval types and count of application pending for approval</returns>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [Route("HolidayCalendar/{staffId}/{year}")]
        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> GetHolidayCalendarAsync(string staffId, int year)
        {
            if (staffId == string.Empty)
            {
                var identity = (ClaimsIdentity)User.Identity;
                staffId = UserHelper.GetUserClaims(identity).StaffId;
            }
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }
            var holidayCalendar = await GetHolidayCalendar(staffId);
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            var result = holidayCalendar.Select(h => new
            {
                HolidayName = textInfo.ToTitleCase(h.HolidayName.ToLower()),
                h.HolidayDateFrom,
                h.HolidayDateTo
            });
            return Ok(result);
        }

        private async Task<List<HolidayGroupTxn1>> GetHolidayCalendar(string staffId)
        {

            //Not required . need to remove once impl is over. aug 15

            var vrepo = new LandingPageBusinessLogic();
            var holidays = vrepo.GetHolidayCalendar(staffId);
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            var result = holidays.Select(h => new HolidayGroupTxn1
            {
                HolidayName = textInfo.ToTitleCase(h.HolidayName.ToLower()),
                HolidayDateFrom = textInfo.ToTitleCase(h.HolidayDateFrom),
                HolidayDateTo = textInfo.ToTitleCase(h.HolidayDateTo)
            }).ToList();

            
            return (result);
        }

        /// <summary>                   
        /// 
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("HeadCount/{deptId}/{role}")]
        public IHttpActionResult GetHeadCountBy(string deptId, string role)
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("HeadCountSummary")]
        public IHttpActionResult GetHeadCountSummary()
        {

            return Ok(

                new[]
                {
                       new{
                                        Id= "DEP0001",
                                        Name= "Engineering",
                                        Shift= "Shift A",
                                        ApprovedHC= "30",
                                        PresentHC= "30",
                                        AbsentHC= "30",
                                        PresentPercent= "95",
                                        AbsentPercent= "5",
                                        AvgLateComingMins= "7",
                                        AvgEarlyGoingMins= "2"


                        },
                       new{
                                        Id= "DEP0001",
                                        Name= "Engineering",
                                        Shift= "Shift B",
                                        ApprovedHC= "20",
                                        PresentHC= "19",
                                        AbsentHC= "1",
                                        PresentPercent= "95",
                                        AbsentPercent= "5",
                                        AvgLateComingMins= "3",
                                        AvgEarlyGoingMins= "5"

                            },
                        new{
                                        Id= "DEP0001",
                                        Name= "Engineering",
                                        Shift= "Shift C",
                                        ApprovedHC= "30",
                                        PresentHC= "30",
                                        AbsentHC= "0",
                                        PresentPercent= "100",
                                        AbsentPercent= "0",
                                        AvgLateComingMins= "7",
                                        AvgEarlyGoingMins= "2"
                                        }
                }
                );
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("attendance/{staffId}/{year}/{month}")]
        public async Task<IHttpActionResult> GetMonthlyAttendanceAsync(int month, int year, string staffId)
        {
            if ((month > 0 && month <= 12))
            {
                if ((year >= DateTime.Now.Year - 1 && year <= DateTime.Now.Year))
                {
                    List<CalendarDays> lstCD = await GetMonthlyAttendance(staffId);
                    var result = lstCD.Select(c => new
                    {
                        c.ActualDate,
                        c.DayName,
                        c.Id,
                        c.WeekNumber,
                        c.Day,
                        c.ShortName,
                        c.InTime,
                        c.OutTime,
                        ActualInTime = stringNullCheck(c.ActualInTime),
                        ActualOutTime = stringNullCheck(c.ActualOutTime),
                        c.FHStatus,
                        c.SHStatus,
                        c.AttendanceStatus,
                        ActualWorkedHours = stringNullCheck(c.ActualWorkedHours),
                        c.LateComing,
                        c.EarlyGoing
                    });
                    return Ok(result);
                }
                else
                    return BadRequest("Invalid Year");
            }
            else
                return BadRequest("Invalid month");
        }



        string stringNullCheck(string input)
        {
            string returnval = string.Empty;
            if (string.IsNullOrEmpty(input))
            {
                returnval = string.Empty;

            }
            else if (input.Equals("-") || input.Equals("--"))
            {
                returnval = string.Empty;
            }
            else
            {
                returnval = input;
            }
            return returnval;
        }

        async Task<List<CalendarDays>> GetMonthlyAttendance(string staffId)
        {
            var Cd = new CalendarBusinessLogic();
            var lstCD = Cd.GetMyAttendanceForMobile(staffId.Trim());
            return lstCD;
        }

        [HttpGet]
        [Authorize]
        [Route("attendance/team/{workedDate}")]
        public async Task<IHttpActionResult> GetTeamAttendanceFor(DateTime workedDate)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            var identity = (ClaimsIdentity)User.Identity;
            var managerId = UserHelper.GetUserClaims(identity).StaffId;

            List<TeamAttendance> teamAttendance = null;
            try
            {
                var Cd = new UserLandingPageBusinessLogic();

                var wkdDate1 = workedDate.ToString("yyyy-MMM-dd");
                var wkdDate2 = workedDate.ToString("yyyy-MMM-dd");
                teamAttendance = Cd.GetTeamAttendanceFor(managerId, wkdDate1, wkdDate2);

            }
            catch (Exception e)
            {
                //TODO Log exception
                throwExceptionResult(e);

            }
            var teamAtt = teamAttendance.Select(a =>
                 new
                 {
                     staffId = a.StaffId,
                     staffName = a.Name,
                     totalHoursWorked = a.TotalHoursWorked,
                     shiftName = a.ShiftShortName,
                     shiftTime = a.ShiftName == null ? "-" : a.ShiftName.Substring(0, 11),
                     inTime = a.InTime == null ? "-" : a.InTime.Equals("-") ? string.Empty : a.InTime.Substring(0, 5),
                     outTime = a.OutTime == null ? "-" : a.OutTime.Equals("-") ? string.Empty : a.OutTime.Substring(0, 5),
                     attendanceStatus = a.AttendanceStatus,
                     otTime = string.Empty
                     // otTime = a.OTTime==null ? "-":a.OTTime.Substring(0, 5)
                     //desc=$"Late:{a.late}"

                 });
            return Ok(teamAtt);

        }

        [HttpGet]
        [Authorize]
        [Route("PunchesOfDay/{staffId}/{dateOfPunch}")]
        public async Task<IHttpActionResult> GetPunchOfDay(string staffId, DateTime dateOfPunch)
        {
            dynamic punchDetail = null; ;
            try
            {
                punchDetail = await GetPunchesOfTheDay(staffId, dateOfPunch);
                //return Ok(punchDetail);
            }
            catch (Exception e)
            {
                //TODO Log exception
                throwExceptionResult(e);

            }
            return Ok(punchDetail);
        }

        async Task<PunchOfTheDay> GetPunchesOfTheDay(string staffId, DateTime dateOfPunch)
        {
            PunchOfTheDay punchDetail = null;
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            try
            {
                var Cd = new UserLandingPageBusinessLogic();
                var pOfDay = Cd.GetTodaysPunchDashBoardForMobile(staffId.Trim(), dateOfPunch);

                TinyMapper.Bind<TodaysPunchesDashBoardForMobile, PunchOfTheDay>();

                punchDetail = TinyMapper.Map<PunchOfTheDay>(pOfDay);
                var punchin = punchDetail.SwipeIn.Equals("-") ? string.Empty : textInfo.ToTitleCase(punchDetail.SwipeIn.ToLower());
                var punchout = punchDetail.SwipeOut.Equals("-") ? string.Empty : textInfo.ToTitleCase(punchDetail.SwipeOut.ToLower());
                punchDetail.SwipeIn = punchin;
                punchDetail.SwipeOut = punchout;
                punchDetail.ShiftIn = textInfo.ToTitleCase(punchDetail.ShiftIn.ToLower());
                punchDetail.ShiftOut = textInfo.ToTitleCase(punchDetail.ShiftOut.ToLower());
                punchDetail.SlideMode = punchDetail.SlideMode;

            }
            catch (Exception e)
            {
                throwExceptionResult(e);
            }
            return punchDetail;
        }

        [HttpPost]
        [Route("attendance/mobilePunch")]
        [Authorize]
        public async Task<IHttpActionResult> PostMobilePunch(MobileSwipeTransactionDto mobilePunchDto)
        {
            CustomLogging.LogMessage(TracingLevel.INFO, JsonConvert.SerializeObject(mobilePunchDto));
            CommonBusinessLogic cbl = new CommonBusinessLogic();
            int transactionTypeId = 20;
            string punchingFromLocation = string.Empty;
            CommonBusinessLogic commonBusinessLogic = new CommonBusinessLogic();
            string unqiueId = commonBusinessLogic.GetUniqueId();

            //var logger = new ExceptionManagerApi();
            if (string.IsNullOrEmpty(mobilePunchDto.PunchMode).Equals(true))
            {
                mobilePunchDto.PunchMode = "IN";
            }
            if (mobilePunchDto.PunchMode.ToUpper().Equals("IN"))
            {
                transactionTypeId = 20;
            }
            else
            {
                transactionTypeId = 36;
            }
            if (mobilePunchDto.punchOption == 1)
            {
                punchingFromLocation = "WFH";
            }
            else if (mobilePunchDto.punchOption == 2)
            {
                punchingFromLocation = "Office";
            }
            else
            {
                punchingFromLocation = "OD";
            }
            DashboardSwipes dashboardSwipes = new DashboardSwipes()
            {
                Id = unqiueId,
                StaffId = mobilePunchDto.StaffId,
                TransactionTime = DateTime.Now,
                TransactionType = mobilePunchDto.PunchMode,
                TransactionTypeId = transactionTypeId,
                IpAddress = "Mobile Swipe",
                Lattitude = mobilePunchDto.Lattitude,
                Longitude = mobilePunchDto.Longitude,
                PunchLocation = punchingFromLocation
            };

            PunchTypeHistory punchTypeHistory = new PunchTypeHistory()
            {
                StaffId = mobilePunchDto.StaffId,
                LastPunchType = mobilePunchDto.PunchMode
            };

            cbl.SaveMobilePunch(dashboardSwipes);
            cbl.UpdateLastPunchType(punchTypeHistory);
            
          var t = await GetPunchesOfTheDay(mobilePunchDto.StaffId, DateTime.Now);
            PunchOfTheDay pod = new PunchOfTheDay()
            {
                EarlyOut = t.EarlyOut,
                InReaderName = t.InReaderName,
                LateIn = t.LateIn,
                OutReaderName = t.OutReaderName,
                ShiftIn = t.ShiftIn,
                ShiftOut = t.ShiftOut,
                StaffId = t.StaffId,
                SwipeIn = t.SwipeIn,
                SwipeOut = t.SwipeOut,
                SlideMode = t.SlideMode
            };
            return Ok(pod);
        }

        [HttpGet]
        [Route("Application/Init")]
        [Authorize]
        public async Task<IHttpActionResult> InitalizeApplicationAsync()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var staffId = UserHelper.GetUserClaims(identity).StaffId;

            var punches = await GetPunchesOfTheDay(staffId, DateTime.Now.Date);
            var calendar = await GetHolidayCalendar(staffId);
            //  var myMonthlyAttendance = await GetMonthlyAttendance(DateTime.Now.Month, DateTime.Now.Year, staffId);
            var myMonthlyAttendance = await GetMonthlyAttendance(staffId);
            var userDet = await GetUserStaffDetail(staffId);
            var menus = await GetPullUpMenuList();
            //TODO dynamic generation of pullup menus based of staffid/role
            var leaveBal = await GetLeaveBalance(staffId);

            // stringNullCheck
            var userGeoData = new object();
            userGeoData = GetGeoLocationsFor(staffId);

            if (staffId.Equals("00112"))
            {
                userGeoData = new
                {
                    geoType = "geoLocation",
                    geoCoordinates = new[]{
                 new { latitude=11.1683, longitude=79.1489,radius=50} ,
                 new { latitude=11.1697, longitude=79.1406,radius=50},
                 new { latitude=12.980430, longitude=80.204820,radius=50},
                 new { latitude=12.983190, longitude=80.251687,radius=50}

                    },
                };
            }
            else if (staffId.Equals("00139"))
            {
                userGeoData = new
                {
                    geoType = "geoLocation",
                    geoCoordinates = new[]{
                 new { latitude=11.1683, longitude=79.1489,radius=50} ,
                 new { latitude=11.1697, longitude=79.1406,radius=50},
                 new { latitude=27.80, longitude=-191.493300,radius=50} ,
                      new { latitude=12.983190, longitude=80.251687,radius=50}

                    },
                };

            }
            else
            {
                userGeoData = new
                {
                    geoType = "geoTagging",
                    geoCoodinates = ""

                };
            }
            var ret = new
            {
                holidayCalender = calendar,
                punchOfTheDay = punches,
                monthlyAttendance = myMonthlyAttendance.Select(a => new
                { Id = a.Id, WeekNo = a.WeekNumber, Day = a.Day, InTime = stringNullCheck(a.ActualInTime), OutTime = stringNullCheck(a.ActualOutTime), CurDate = a.ActualDate, WorkedHrs = a.ActualWorkedHours }),
                userProfile = userDet,
                pullupMenu = menus,
                leaveBalance = leaveBal,
                geoData = userGeoData
            };
            return Ok(ret);

        }

        private object GetGeoLocationsFor(string staffId)
        {
            return null;
        }

        [HttpGet]
        [Route("Init")]
        [Authorize]
        public async Task<IHttpActionResult> InitalizeDashboardAsync()
        {
            dynamic ret = null;
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var staffId = UserHelper.GetUserClaims(identity).StaffId;

                var punches = await GetPunchesOfTheDay(staffId, DateTime.Now.Date);

                //TODO dynamic generation of pullup menus based of staffid/role
                var leaveBal = await GetLeaveBalance(staffId);

                ret = new
                {
                    punchOfTheDay = punches,
                    leaveBalance = leaveBal
                };
            }
            catch (Exception e)
            {
                var m = throwExceptionResult(e);
                throw new HttpResponseException(m);

            }
            return Ok(ret);

        }
        private static HttpResponseMessage throwExceptionResult(Exception e)
        {
            //TODO Log exception
            HttpResponseMessage message = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new StringContent(e.Message)
            };
            return (message);
        }

        string getStatusDesc(AllApplicationHistory r)
        {
            //TODO RAJESH SEP  6
            string status = "Approved";

            if (r.IsCancelled)
            {
                status = "Cancelled";
            }
            else if (r.IsReviewerCancelled)
            {
                status = "Cancelled(R)";
            }
            else if (r.IsApproverCancelled)
            {
                status = "Cancelled(A)";
            }
            else if (r.ApproverStatus.ToUpper().Equals("REJECTED"))
            {
                status = "REJECTED(A)";
            }
            else if (r.ApproverStatus.ToUpper().Equals("PENDING"))
            {
                status = "Pending(A)";
            }
            return status;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        /// 



        async Task<UserProfileDto> GetUserStaffDetail(string staffId)
        {

            var staffBl = new StaffBusinessLogic();

            var staffInfo = staffBl.GetStaffOfficialInformationForApi(staffId);
            //TODO Convert the staffOfficialInfo into UserProfileDto
            //return UserProfileDto

            var userDto = new UserProfileDto()
            {
                UserFullName = staffInfo.UserFullName,
                StaffId = staffInfo.StaffId,
                DepartmentId = staffInfo.DepartmentId,
                DesignationId = staffInfo.DesignationId,
                BranchId = staffInfo.BranchId,
                DateOfJoining = staffInfo.DateOfJoining?.ToString(),
                Email = staffInfo.Email,
                PolicyId = staffInfo.PolicyId,
                LeaveGroupId = staffInfo.LeaveGroupId,
                UserRole = staffInfo.UserRole,
                UserRoleId = staffInfo.UserRoleId,
                ReportingManagerName = staffInfo.ReportingManagerName,
                ReportingManagerEmailId = staffInfo.ReportingManagerEmailId,
                ReportingManagerId = staffInfo.ReportingManagerId,
                Phone = staffInfo.Phone,
                // Photo = staffInfo.PhotoB64String,
                Fax = staffInfo.Fax,
                DivisionName = staffInfo.DivisionName,
                DepartmentName = staffInfo.DepartmentName,
                LocationName = staffInfo.LocationName,
                GradeName = staffInfo.GradeName,
                DesignationName = staffInfo.DesignationName
            };

            return userDto;
        }


        async Task<Dictionary<string, string>> GetPullUpMenuList()
        {
            string userRole = string.Empty;
            // userRole = UserClaims.
            Dictionary<string, string> menuList = new Dictionary<string, string>
            {
                { "Dashboard", "dashboard/index" },
                { "My Attendance", "attendance/myattendance" },
                { "Holiday List", "holiday/holidayCalendar" },
                { "Leave Apply", "application/leave" },
                { "Shift Change", "shifts/index" },
                { "Permission", "application/permission" },
                { "On Duty", "application/onduty" },
                { "Over Time", "application/overtime" },
                { "Comp Off Credit", "application/compoffCreditreq" },
                { "Comp Off Req", "application/compoffavail" },
                { "Manual Punch", "application/manualpunch" },
                { "Team Attendance", "attendance/team" },
                { "My Approvals", "approvals/list" },
                { "My Profile", "profile/index" },
                { "Log Out", "logout" }
            };


            return menuList;
        }

        async Task<List<LeaveBalanceList>> GetLeaveBalance(string staffId)
        {

            var bl = new UserLandingPageBusinessLogic();
            var lst = bl.ShowLeaveBalanceTable(staffId);
            return lst;
        }

        async Task<List<AllApplicationHistory>> GetAllApplicationHistory(string staffId, string applnType = "ALL")
        {
            var bl = new LandingPageBusinessLogic();
            var lst = bl.GetAllApplicationHistory(staffId, 20, applnType);
            return lst;

        }

        [HttpGet]
        [Authorize]
        [Route("applicationHistory/{applnType}/{staffId}")]
        public async Task<IHttpActionResult> ApplicationHistory(string applnType, string staffId)
        {
            //var applnHistory = await GetAllApplicationHistory(staffId);

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            dynamic returnHistory = null;
            applnType = applnType.ToUpper();
            string apln = string.Empty;
            if (applnType.ToUpper().Equals("LEAVE"))
            {

                //StartDate = d.StartDate + " " + d.StartTime,
                //        EndDate = d.EndDate + " " + d.EndTime,
                apln = "LA";
                var applnHistory = await GetAllApplicationHistory(staffId, apln);
                returnHistory = applnHistory.Where(a => a.RequestApplicationType == "LA").Select(a => new
                {
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = textInfo.ToTitleCase(a.Type.ToLower()),
                    fromDate = textInfo.ToTitleCase(a.StartDate.ToLower()).Replace(' ', '-') + " " + a.StartTime,
                    endDate = textInfo.ToTitleCase(a.EndDate.ToLower()).Replace(' ', '-') + " " + a.EndTime,
                    totalDaysOrHrs = a.TotalDays,
                    reason = a.Remarks ?? "-",
                    // applicationType = a.RequestApplicationType,
                    applicationType = textInfo.ToTitleCase(a.Type.ToLower()),
                    status = getStatusDesc(a)
                });

            }
            else if (applnType.ToUpper().Equals("PERMISSION"))
            {
                apln = "PO";
                var applnHistory = await GetAllApplicationHistory(staffId, apln);
                returnHistory = applnHistory.Where(a => a.RequestApplicationType == "PO").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = textInfo.ToTitleCase(a.Type.ToLower()),
                    fromDate = $"{textInfo.ToTitleCase(a.StartDate.ToLower())} {a.StartTime}",
                    endDate = $"{textInfo.ToTitleCase(a.EndDate.ToLower())} {a.EndTime}",
                    totalDaysOrHrs = $"{a.TotalHours.Substring(0, 5)} Hrs",
                    reason = a.Remarks,
                    //applicationType = a.RequestApplicationType,
                    applicationType = textInfo.ToTitleCase(a.Type.ToLower()),
                    status = getStatusDesc(a)

                });
            }
            else if (applnType.ToUpper().Equals("ONDUTY"))
            {
                apln = "OD";
                var applnHistory = await GetAllApplicationHistory(staffId, apln);
                returnHistory = applnHistory.Where(a => a.RequestApplicationType == "OD").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                    fromDate = textInfo.ToTitleCase(a.StartDate.ToLower()),
                    endDate = textInfo.ToTitleCase(a.EndDate.ToLower()),
                    totalDaysOrHrs = a.ODDuration.ToUpper().Equals("SINGLE DAY") ? a.TotalHours.Substring(0, 5) + " Hrs" : $"{a.TotalDays} day(s)",
                    reason = a.Remarks,
                    //applicationType = a.RequestApplicationType,
                    applicationType = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                    status = getStatusDesc(a)
                });
            }
            else if (applnType.ToUpper().Equals("MANUALPUNCH"))
            {
                apln = "MP";
                var applnHistory = await GetAllApplicationHistory(staffId, apln);
                returnHistory = applnHistory.Where(a => a.RequestApplicationType == "MP").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = $"{a.PunchType} Punch",
                    fromDate = a.PunchType == "In" ? textInfo.ToTitleCase(a.StartDate.ToLower()) : a.PunchType == "InOut" ? textInfo.ToTitleCase(a.StartDate.ToLower()) : string.Empty,
                    endDate = a.PunchType == "Out" ? textInfo.ToTitleCase(a.EndDate.ToLower()) : a.PunchType == "InOut" ? textInfo.ToTitleCase(a.EndDate.ToLower()) : string.Empty,

                    //fromDate = a.PunchType == "In" ? textInfo.ToTitleCase(a.StartDate.ToLower()) : textInfo.ToTitleCase(a.EndDate.ToLower()),
                    //endDate = a.PunchType.ToUpper().Equals("IN") ? $"In {a.StartTime.Substring(0, 5)}" : a.PunchType.ToUpper().Equals("INOUT") ? $"In { a.StartTime.Substring(0, 5)} - Out {a.EndTime.Substring(0, 5)}" : $"Out {a.EndTime}",


                    totalDaysOrHrs = "NA",
                    reason = a.Remarks,
                    //applicationType = a.RequestApplicationType,
                    applicationType = $"{a.PunchType} Punch",
                    status = getStatusDesc(a),
                    punchTime = a.PunchType.ToUpper().Equals("IN") ? $"In {a.StartTime.Substring(0, 5)}" : a.PunchType.ToUpper().Equals("INOUT") ? $"In { a.StartTime.Substring(0, 5)} - Out {a.EndTime.Substring(0, 5)}" : $"Out {a.EndTime}"

                });
            }
            else if (applnType.ToUpper().Equals("BUSINESSTRAVEL"))
            {
                apln = "BT";
                var applnHistory = await GetAllApplicationHistory(staffId, apln);
                returnHistory = applnHistory.Where(a => a.RequestApplicationType == "BT").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                    fromDate = textInfo.ToTitleCase(a.StartDate.ToLower()),
                    endDate = textInfo.ToTitleCase(a.EndDate.ToLower()),
                    totalDaysOrHrs = a.ODDuration.ToUpper().Equals("SINGLE DAY") ? a.TotalHours.Substring(0, 5) + " Hrs" : $"{a.TotalDays} day(s)",
                    reason = a.Remarks,
                    //applicationType = a.RequestApplicationType,
                    applicationType = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",

                    status = getStatusDesc(a)

                });

            }
            else if (applnType.ToUpper().Equals("WORKFROMHOME"))
            {
                apln = "WFH";
                var applnHistory = await GetAllApplicationHistory(staffId, apln);
                returnHistory = applnHistory.Where(a => a.RequestApplicationType == "WFH").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                    fromDate = textInfo.ToTitleCase(a.StartDate.ToLower()),
                    endDate = textInfo.ToTitleCase(a.EndDate.ToLower()),
                    totalDaysOrHrs = a.ODDuration.ToUpper().Equals("SINGLE DAY") ? a.TotalHours.Substring(0, 5) + " Hrs" : $"{a.TotalDays} day(s)",
                    reason = a.Remarks,
                    //applicationType = a.RequestApplicationType,
                    applicationType = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",

                    status = getStatusDesc(a)


                });
            }
            else if (applnType.ToUpper().Equals("COMPOFFCREDITREQ"))
            {
                apln = "CR";
                var applnHistory = await GetAllApplicationHistory(staffId, apln);
                returnHistory = applnHistory.Where(a => a.RequestApplicationType == "CR").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                    fromDate = textInfo.ToTitleCase(a.StartDate.ToLower()),
                    endDate = textInfo.ToTitleCase(a.EndDate.ToLower()),
                    //totalDaysOrHrs = a.ODDuration.ToUpper().Equals("SINGLE DAY") ? a.TotalHours.Substring(0, 5) + " Hrs" : $"{a.TotalDays} day(s)",
                    totalDaysOrHrs = $"{a.TotalDays} day(s)",
                    reason = a.Remarks,
                    //applicationType = a.RequestApplicationType,
                    applicationType = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",

                    status = getStatusDesc(a)


                });
            }
            else if (applnType.ToUpper().Equals("COMPOFFAVAIL"))
            {
                apln = "CO";
                var applnHistory = await GetAllApplicationHistory(staffId, apln);
                returnHistory = applnHistory.Where(a => a.RequestApplicationType == "CO").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                    fromDate = textInfo.ToTitleCase(a.StartDate.ToLower()),
                    endDate = textInfo.ToTitleCase(a.EndDate.ToLower()),
                    //totalDaysOrHrs = a.ODDuration.ToUpper().Equals("SINGLE DAY") ? a.TotalHours.Substring(0, 5) + " Hrs" : $"{a.TotalDays} day(s)",
                    totalDaysOrHrs = $"{a.TotalDays} day(s)",

                    reason = a.Remarks,
                    //applicationType = a.RequestApplicationType,
                    applicationType = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",

                    status = getStatusDesc(a)


                });
            }
            else
            {
                var allHistory = await GetAllApplicationHistory(staffId);
                var leaveHistory = allHistory.Where(a => a.RequestApplicationType == "LA").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {
                    //--
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = textInfo.ToTitleCase(a.Type.ToLower()),
                    fromDate = textInfo.ToTitleCase(a.StartDate.ToLower()).Replace(' ', '-') + " " + a.StartTime,
                    endDate = textInfo.ToTitleCase(a.EndDate.ToLower()).Replace(' ', '-') + " " + a.EndTime,
                    totalDaysOrHrs = a.TotalDays,
                    reason = a.Remarks ?? "-",
                    //applicationType = a.RequestApplicationType,
                    applicationType = textInfo.ToTitleCase(a.Type.ToLower()),
                    status = getStatusDesc(a)

                });

                var permissionHistory = allHistory.Where(a => a.RequestApplicationType == "PO").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = textInfo.ToTitleCase(a.Type.ToLower()),
                    fromDate = $"{textInfo.ToTitleCase(a.StartDate.ToLower())} {a.StartTime}",
                    endDate = $"{textInfo.ToTitleCase(a.EndDate.ToLower())} {a.EndTime}",
                    totalDaysOrHrs = $"{a.TotalHours.Substring(0, 5)} Hrs",
                    reason = a.Remarks,
                    // applicationType = a.RequestApplicationType,
                    applicationType = textInfo.ToTitleCase(a.Type.ToLower()),
                    status = getStatusDesc(a)

                });

                var manualPunchHistory = allHistory.Where(a => a.RequestApplicationType == "MP").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {

                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = $"{a.PunchType} Punch",
                    fromDate = a.PunchType == "In" ? textInfo.ToTitleCase(a.StartDate.ToLower()) : a.PunchType == "InOut" ? textInfo.ToTitleCase(a.StartDate.ToLower()) : string.Empty,
                    endDate = a.PunchType == "Out" ? textInfo.ToTitleCase(a.EndDate.ToLower()) : a.PunchType == "InOut" ? textInfo.ToTitleCase(a.EndDate.ToLower()) : string.Empty,
                    totalDaysOrHrs = "NA",
                    reason = a.Remarks,
                    //applicationType = a.RequestApplicationType,
                    applicationType = $"{a.PunchType} Punch",
                    status = getStatusDesc(a),
                    punchTime = a.PunchType.ToUpper().Equals("IN") ? $"In {a.StartTime.Substring(0, 5)}" : a.PunchType.ToUpper().Equals("INOUT") ? $"In { a.StartTime.Substring(0, 5)} - Out {a.EndTime.Substring(0, 5)}" : $"Out {a.EndTime}"
                });

                var businessTravelHistory = allHistory.Where(a => a.RequestApplicationType == "BT").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                    fromDate = textInfo.ToTitleCase(a.StartDate.ToLower()),
                    endDate = textInfo.ToTitleCase(a.EndDate.ToLower()),
                    totalDaysOrHrs = a.ODDuration.ToUpper().Equals("SINGLE DAY") ? a.TotalHours.Substring(0, 5) + " Hrs" : $"{a.TotalDays} day(s)",
                    reason = a.Remarks,
                    //applicationType = a.RequestApplicationType,
                    applicationType = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                    status = getStatusDesc(a)

                });

                var workFromHomeHistory = allHistory.Where(a => a.RequestApplicationType == "WFH").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {

                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                    fromDate = textInfo.ToTitleCase(a.StartDate.ToLower()),
                    endDate = textInfo.ToTitleCase(a.EndDate.ToLower()),
                    totalDaysOrHrs = a.ODDuration.ToUpper().Equals("SINGLE DAY") ? a.TotalHours.Substring(0, 5) + " Hrs" : $"{a.TotalDays} day(s)",
                    reason = a.Remarks,
                    //applicationType = a.RequestApplicationType,
                    applicationType = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                    status = getStatusDesc(a)

                });
                var onDutyHistory = allHistory.Where(a => a.RequestApplicationType == "OD").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {
                    //--
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                    fromDate = textInfo.ToTitleCase(a.StartDate.ToLower()),
                    endDate = textInfo.ToTitleCase(a.EndDate.ToLower()),
                    totalDaysOrHrs = a.ODDuration.ToUpper().Equals("SINGLE DAY") ? a.TotalHours.Substring(0, 5) + " Hrs" : $"{a.TotalDays} day(s)",
                    reason = a.Remarks,
                    // applicationType = a.RequestApplicationType,
                    applicationType = $"{textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                    status = getStatusDesc(a)


                });

                var compOffCreditReqHistory = allHistory.Where(a => a.RequestApplicationType == "CR").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = textInfo.ToTitleCase(a.Type.ToLower()),
                    fromDate = $"{textInfo.ToTitleCase(a.StartDate.ToLower())} {a.StartTime}",
                    endDate = $"{textInfo.ToTitleCase(a.EndDate.ToLower())} {a.EndTime}",
                    totalDaysOrHrs = $"{a.TotalDays} day(s)",
                    reason = a.Remarks,
                    // applicationType = a.RequestApplicationType,
                    applicationType = textInfo.ToTitleCase(a.Type.ToLower()),
                    status = getStatusDesc(a)

                });

                var compOffAvailReqHistory = allHistory.Where(a => a.RequestApplicationType == "CO").OrderByDescending(a => a.ApplicationDate).Select(a => new
                {
                    id = a.Id,
                    staffId = a.StaffId,
                    staffName = a.StaffName,
                    desc = textInfo.ToTitleCase(a.Type.ToLower()),
                    fromDate = $"{textInfo.ToTitleCase(a.StartDate.ToLower())} {a.StartTime}",
                    endDate = $"{textInfo.ToTitleCase(a.EndDate.ToLower())} {a.EndTime}",
                    totalDaysOrHrs = $"{a.TotalDays} day(s)",
                    reason = a.Remarks,
                    // applicationType = a.RequestApplicationType,
                    applicationType = textInfo.ToTitleCase(a.Type.ToLower()),
                    status = getStatusDesc(a)

                });
                returnHistory = new
                {
                    leaveHistory = leaveHistory,
                    permissionHistory = permissionHistory,
                    onDutyHistory = onDutyHistory,
                    businessTravelHistory = businessTravelHistory,
                    workFromHomeHistory = workFromHomeHistory,
                    manualPunchHistory = manualPunchHistory,
                    compOffCreditReqHistory = compOffCreditReqHistory,
                    compOffAvailReqHistory = compOffAvailReqHistory

                };
            }

            return Ok(returnHistory);
        }


        private IHttpActionResult GetGeoLocation(string staffId)
        {
            var geoJson = new[]{
                 new { latitude=37.80, longitude=-121.493300,radius=50} ,
                  new { latitude=57.80, longitude=-161.493300,radius=50},
                    new { latitude=77.80, longitude=-191.493300,radius=50}

            };
            return Ok(geoJson);


        }


    }
}

 
