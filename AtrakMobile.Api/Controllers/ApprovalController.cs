using AtrakMobileApi.Helpers;
using AtrakMobileApi.Models;
using Attendance.BusinessLogic;
using Attendance.Model;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using AtrakMobileApi.Logging;
using LeaveType = AtrakMobileApi.Models.LeaveType;

namespace AtrakMobileApi.Controllers
{

    /// </summary>
    [RoutePrefix("api/Approvals")]

    public class ApprovalController : ApiController
    {
        RALeaveApplicationBusinessLogic leaveObj = new RALeaveApplicationBusinessLogic();
        dynamic GetLeaveApprovedHistory(List<RALeaveApplication> lst, string staffId)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            var leaveApprovedHistory = lst.Take(10).Select(a => new
            {
                id = a.Id,
                staffId = a.StaffId,
                staffName = a.StaffName,
                desc = textInfo.ToTitleCase(a.Type.ToLower()),
                fromDate = textInfo.ToTitleCase(a.StartDate.ToLower()),
                endDate = textInfo.ToTitleCase(a.EndDate.ToLower()),
                totalDaysOrHrs = a.TotalDays,
                reason = a.Remarks,
                applicationType = "Leave Application",
                status = a.Status       //
            });
            return leaveApprovedHistory;
        }


        async Task<List<RAPermissionApplication>> GetPermissionApprovedHistoryAsync(string staffId, string applnType, string roleId)
        {
            var bl = new RAPermissionApplicationBusinessLogic();
            //Rajesh Sep 16
            var lst = bl.GetApprovedPermissions(staffId);
            // var lst = bl.GetAppliedPermissionsForMyTeam(staffId, "", roleId);
            return lst;

        }

        async Task<List<RAODRequestApplication>> GetODApprovedHistoryAsync(string staffId, string applnType, string roleId)
        {
            var bl = new RAOnDutyApplicationBusinessLogic();
            //Rajesh Sep 16
            var lst = bl.GetApprovedOnDutyForMyteam(staffId);
            //var lst = bl.GetAppliedOnDutyRequestForMyTeam(staffId,"",roleId, "OD");
            return lst;

        }

        async Task<List<RAODRequestApplication>> GetBTApprovedHistoryAsync(string staffId, string applnType, string roleId)
        {
            var bl = new RAOnDutyApplicationBusinessLogic();
            //Rajesh sep 16
            var lst = bl.GetApprovedBusinessTravelForMyTeam(staffId);
            //var lst = bl.GetAppliedOnDutyRequestForMyTeam(staffId, "", roleId, "BT");
            return lst;

        }

        async Task<List<RAODRequestApplication>> GetWFHApprovedHistoryAsync(string staffId, string applnType, string roleId)
        {
            var bl = new RAOnDutyApplicationBusinessLogic();
            //Rajesh sep 16
            var lst = bl.GetApprovedWFHForMyTeam(staffId);
            //var lst = bl.GetAppliedOnDutyRequestForMyTeam(staffId, "", roleId, "BT");
            return lst;

        }
        async Task<List<RAManualPunchApplication>> GetManualPunchApprovedHistoryAsync(string staffId, string applnType, string role)
        {
            var bl = new RAManualPunchApplicationBusinessLogic();
            //var lst = bl.GetAppliedManualPunches(staffId);
            //Rajesh Sep 16
            //var lst = bl.GetApprovedManualPunchesForMyTeam(staffId,"",role);
            var lst = bl.GetApprovedManualPunchesForMyTeam(staffId);
            return lst;

        }
        //Rajesh Sep 16 why this methods does not require roleId whereas others require?
        async Task<List<RALeaveApplication>> GetLeaveApprovedHistoryAsync(string staffId, string applnType)
        {
            var bl = new RALeaveApplicationBusinessLogic();
            var lst = bl.GetApprovedLeavesForMyTeam(staffId);
            return lst;

        }

        private async Task<List<RACoffCreditRequestApplication>> GetCOffReqApprovedHistoryAsync(string staffId, string applnType, string roleId)
        {
            var businessLogic = new RACoffCreditApplicationBusinessLogic();
            var lst = businessLogic.GetCoffCreditRequestMappedUnderMe(staffId);
            return lst;
        }

        private async Task<List<RACoffAvailingRequestApplication>> GetCOffAvailingApprovedHistoryAsync(string staffId, string applnType, string roleId)
        {
            var businessLogic = new RACoffAvalingApplicationBusinessLogic();
            var lst = businessLogic.GetCoffAvailRequestMappedUnderMe(staffId);
            return lst;
        }

        // GetAppliedCoffCreditRequestForMyTeam

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("PendingApprovals/All/Count/{staffId}")]
        [Authorize]
        public async Task<IHttpActionResult> GetPendingApprovalsCountAsync(string staffId)
        {
            //var identity = (ClaimsIdentity)User.Identity;
            dynamic returnVal;
            var approvals = await GetAllPendingApprovals(staffId);
            returnVal = new
            {
                leave = approvals.Where(a => a.ParentType == "LA").Count(),
                permission = approvals.Where(a => a.ParentType == "PO").Count(),
                manualPunch = approvals.Where(a => a.ParentType == "MP").Count(),
                onDuty = approvals.Where(a => a.ParentType == "OD").Count(),
                businessTravel = approvals.Where(a => a.ParentType == "BT").Count(),
                workFromHome = approvals.Where(a => a.ParentType == "WFH").Count(),
                coffCrReq = approvals.Where(a => a.ParentType == "CR").Count(),
                coffAvail = approvals.Where(a => a.ParentType == "CO").Count(),
            };

            return Ok(returnVal);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("PendingApprovals/{applnType?}")]
        [Route("PendingApprovals/{applnType}/{staffId}")]
        [Authorize]
        public async Task<IHttpActionResult> GetAllPendingApprovalsAsync(string staffId, string applnType = "All")
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roleId = UserHelper.GetUserClaims(identity).RoleId;

            List<RALeaveApplication> leaveApprovedHistoryList = null;
            List<AllPendingApprovals> approvals = null;
            List<RAPermissionApplication> permissionApprovedHistoryList = null;
            List<RAODRequestApplication> ODApprovedHistoryList = null;
            List<RAODRequestApplication> BTApprovedHistoryList = null;
            List<RAODRequestApplication> WFHApprovedHistoryList = null;
            List<RAManualPunchApplication> manualPunchApprovedHistoryList = null;
            List<RACoffRequestApplication> COffReqApprovedHistoryList = null;
            List<RACoffAvailingRequestApplication> COffAvailingApprovedHistoryList = null;

            if (staffId == string.Empty)
            {
                staffId = UserHelper.GetUserClaims(identity).StaffId;
            }


            if (applnType.ToUpper().Equals("LEAVE"))
            {
                //leaveApprovedHistoryList = await GetLeaveApprovedHistoryAsync(staffId, applnType);
                //approvals = await GetAllPendingApprovals(staffId, "LEAVE");

                var leaveApprovedTask = GetLeaveApprovedHistoryAsync(staffId, applnType);
                var pendingApprovalTask = GetAllPendingApprovals(staffId, "LEAVE");
                await Task.WhenAll(leaveApprovedTask, pendingApprovalTask);
                leaveApprovedHistoryList = leaveApprovedTask.Result;
                approvals = pendingApprovalTask.Result;
            }
            if (applnType.ToUpper().Equals("PERMISSION"))
            {
                permissionApprovedHistoryList = await GetPermissionApprovedHistoryAsync(staffId, applnType, roleId);
                approvals = await GetAllPendingApprovals(staffId, "PERMISSION");
            }
            if (applnType.ToUpper().Equals("MANUALPUNCH"))
            {
                manualPunchApprovedHistoryList = await GetManualPunchApprovedHistoryAsync(staffId, applnType, roleId);       //roleid
                approvals = await GetAllPendingApprovals(staffId, "MANUALPUNCH");
            }
            if (applnType.ToUpper().Equals("ONDUTY"))
            {
                ODApprovedHistoryList = await GetODApprovedHistoryAsync(staffId, "OD", roleId);
                approvals = await GetAllPendingApprovals(staffId, "ONDUTY");
            }
            if (applnType.ToUpper().Equals("BUSINESSTRAVEL"))
            {
                BTApprovedHistoryList = await GetBTApprovedHistoryAsync(staffId, "BT", roleId);
                approvals = await GetAllPendingApprovals(staffId, "BUSINESSTRAVEL");
            }
            if (applnType.ToUpper().Equals("WORKFROMHOME"))
            {
                WFHApprovedHistoryList = await GetWFHApprovedHistoryAsync(staffId, "WFH", roleId);
                approvals = await GetAllPendingApprovals(staffId, "WORKFROMHOME");
            }
            if (applnType.ToUpper().Equals("COFFREQ"))
            {

                List<RACoffCreditRequestApplication> creditApplicationsHistoryMappedUnderMe = await GetCOffReqApprovedHistoryAsync(staffId, "CR", roleId);
                approvals = await GetAllPendingApprovals(staffId, "COFFREQ");
            }
            if (applnType.ToUpper().Equals("COFFAVAIL"))
            {
                COffAvailingApprovedHistoryList = await GetCOffAvailingApprovedHistoryAsync(staffId, "CO", roleId);
                approvals = await GetAllPendingApprovals(staffId, "COFFAVAIL");
            }

            if (applnType.ToUpper().Equals("ALL"))
            {
                var BTApprovedHistory = GetBTApprovedHistoryAsync(staffId, "BT", roleId);
                var ODApprovedHistory = GetODApprovedHistoryAsync(staffId, "OD", roleId);
                var WFHApprovedHistory = GetWFHApprovedHistoryAsync(staffId, "WFH", roleId);
                var manualPunchApprovedHistory = GetManualPunchApprovedHistoryAsync(staffId, applnType, roleId);
                var permissionApprovedHistory = GetPermissionApprovedHistoryAsync(staffId, applnType, roleId);
                var leaveApprovedHistory = GetLeaveApprovedHistoryAsync(staffId, applnType);

                var approvalsList = GetAllPendingApprovals(staffId, applnType);
                await Task.WhenAll(BTApprovedHistory, ODApprovedHistory, WFHApprovedHistory, manualPunchApprovedHistory, permissionApprovedHistory, leaveApprovedHistory);

                BTApprovedHistoryList = BTApprovedHistory.Result;
                ODApprovedHistoryList = ODApprovedHistory.Result;
                manualPunchApprovedHistoryList = manualPunchApprovedHistory.Result;
                permissionApprovedHistoryList = permissionApprovedHistory.Result;
                leaveApprovedHistoryList = leaveApprovedHistory.Result;
                WFHApprovedHistoryList = WFHApprovedHistory.Result;
                approvals = approvalsList.Result;




            }

            //Rajesh Oct 3 - commenting the below line and adding the line for every apln type in the abve if else.  
            // approvals = await GetAllPendingApprovals(staffId, applnType);



            dynamic approvalsPending = approvals;

            if (applnType.ToUpper().Equals("ALL"))
            {
                approvalsPending = new
                {
                    leave =
                    new
                    {
                        approvalPending = GetPendingLeaveApprovals(approvals),
                        approvalHistory = GetLeaveApprovedHistory(leaveApprovedHistoryList, staffId)
                    },

                    permission =
                    new
                    {
                        approvalPending = GetPendingPermissionApproval(approvals),
                        approvalHistory = GetPermissionApprovedHistory(permissionApprovedHistoryList, staffId)
                    },

                    onDuty =
                    new
                    {
                        approvalPending = GetOnDutyApprovals(approvals),
                        approvalHistory = GetODApprovedHistory(ODApprovedHistoryList, staffId)
                    },

                    businessTravel =
                    new
                    {
                        approvalPending = GetBusinessTravelApprovals(approvals),
                        approvalHistory = GetBTApprovedHistory(BTApprovedHistoryList, staffId)
                    },
                    workFromHome =
                    new
                    {
                        approvalPending = GetWorkFromHomeApprovals(approvals),
                        approvalHistory = GetWFHApprovedHistory(WFHApprovedHistoryList, staffId)
                    },

                    manualPunch =
                    new
                    {
                        approvalPending = GetPendingManualPunchApproval(approvals),
                        approvalHistory = GetManualPunchApprovedHistory(manualPunchApprovedHistoryList, staffId)
                    }

                };
            }

            if (applnType.ToUpper().Equals("LEAVE"))
            {
                approvalsPending = new
                {
                    approvalPending = GetPendingLeaveApprovals(approvals),
                    approvalHistory = GetLeaveApprovedHistory(leaveApprovedHistoryList, staffId)
                };

            }
            if (applnType.ToUpper().Equals("PERMISSION"))
            {
                approvalsPending = new
                {
                    approvalPending = GetPendingPermissionApproval(approvals),
                    approvalHistory = GetPermissionApprovedHistory(permissionApprovedHistoryList, staffId)
                };

            }
            if (applnType.ToUpper().Equals("ONDUTY"))
            {
                approvalsPending = new
                {
                    approvalPending = GetOnDutyApprovals(approvals),
                    approvalHistory = GetODApprovedHistory(ODApprovedHistoryList, staffId)
                };
            }
            if (applnType.ToUpper().Equals("BUSINESSTRAVEL"))
            {
                approvalsPending = new
                {
                    approvalPending = GetBusinessTravelApprovals(approvals),
                    approvalHistory = GetBTApprovedHistory(BTApprovedHistoryList, staffId)
                };
            }
            if (applnType.ToUpper().Equals("WORKFROMHOME"))
            {
                approvalsPending = new
                {
                    approvalPending = GetWorkFromHomeApprovals(approvals),
                    approvalHistory = GetWFHApprovedHistory(WFHApprovedHistoryList, staffId)
                };
            }
            if (applnType.ToUpper().Equals("MANUALPUNCH"))
            {
                approvalsPending = new
                {
                    approvalPending = GetPendingManualPunchApproval(approvals),
                    approvalHistory = GetManualPunchApprovedHistory(manualPunchApprovedHistoryList, staffId)
                };
            }



            return Ok(approvalsPending);

        }

        private dynamic GetManualPunchApprovedHistory(List<RAManualPunchApplication> lst, string staffId)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            var ODApprovedHistory = lst.Take(10).Select(a => new
            {
                id = a.Id,
                staffId = a.StaffId,
                staffName = a.StaffName,
                desc = textInfo.ToTitleCase(a.Type.ToLower()),
                startTime = stringNullCheck(a.StartDateTime) != String.Empty ? textInfo.ToTitleCase(a.StartDateTime.ToLower()) : "",
                endTime = stringNullCheck(a.EndDateTime) != String.Empty ? textInfo.ToTitleCase(a.EndDateTime.ToLower()) : "",
                totalDaysOrHrs = string.Empty,
                reason = a.Remarks,
                applicationType = "Manual Punch Application",
                status = a.Status, //getApprovalHistoryStatusDesc(a,staffId,"MP")
                photoUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority) + $"/api/user/GetStaffPhotoImage/{a.StaffId}"
            });
            return ODApprovedHistory; ;
        }



        private dynamic GetBTApprovedHistory(List<RAODRequestApplication> lst, string staffId)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            var BTApprovedHistory = lst.Take(10).Select(a => new
            {
                id = a.Id,
                staffId = a.StaffId,
                staffName = a.StaffName,
                desc = $"{a.ODDuration}",//textInfo.ToTitleCase(a.Type.ToLower()),
                startTime = stringNullCheck(a.StartDate) != String.Empty ? textInfo.ToTitleCase(a.StartDate.ToLower()) : "",
                endTime = stringNullCheck(a.EndDate) != String.Empty ? textInfo.ToTitleCase(a.EndDate.ToLower()) : "",
                totalDaysOrHrs = a.ODDuration.ToUpper().Contains("SINGLE") ? a.TotalHours.Substring(0, 5) + " hrs" : a.TotalDays.ToString() ?? "1 d",
                reason = a.Remarks,
                applicationType = "Business Travel Application",
                status = a.Status //getApprovalHistoryStatusDesc(a,staffId,"BT")  //a.Status //
            });
            return BTApprovedHistory; ;
        }
        private dynamic GetWFHApprovedHistory(List<RAODRequestApplication> lst, string staffId)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            var WFHApprovedHistory = lst.Take(10).Select(a => new
            {
                id = a.Id,
                staffId = a.StaffId,
                staffName = a.StaffName,
                desc = $"{a.ODDuration}",//textInfo.ToTitleCase(a.Type.ToLower()),
                startTime = stringNullCheck(a.StartDate) != String.Empty ? textInfo.ToTitleCase(a.StartDate.ToLower()) : "",
                endTime = stringNullCheck(a.EndDate) != String.Empty ? textInfo.ToTitleCase(a.EndDate.ToLower()) : "",
                totalDaysOrHrs = a.ODDuration.ToUpper().Contains("SINGLE") ? a.TotalHours.Substring(0, 5) + " hrs" : a.TotalDays.ToString() ?? "1 d",
                reason = a.Remarks,
                applicationType = "Work from Home Application",
                status = a.Status //getApprovalHistoryStatusDesc(a,staffId,"BT")  //a.Status //
            });
            return WFHApprovedHistory; ;
        }
        private dynamic GetODApprovedHistory(List<RAODRequestApplication> lst, string staffId)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            var ODApprovedHistory = lst.Take(10).Select(a => new
            {
                id = a.Id,
                staffId = a.StaffId,
                staffName = a.StaffName,
                desc = "OnDuty",


                //startTime = stringNullCheck(a.StartTime)!=String.Empty? textInfo.ToTitleCase(a.StartTime.ToLower()):"",
                //endTime = stringNullCheck(a.EndTime)!=String.Empty ? textInfo.ToTitleCase(a.EndTime.ToLower()):"",
                // totalDaysOrHrs = a.TotalHours,
                //Oct 29 : Multiple days OD should have start and end date alone ( no time attached)

                startTime = (a.ODDuration.ToUpper().Contains("MULTIPLE")) ? textInfo.ToTitleCase(a.StartDate.ToLower())
                            : stringNullCheck(a.StartTime) != String.Empty ? textInfo.ToTitleCase(a.StartTime.ToLower()) : "",
                endTime = (a.ODDuration.ToUpper().Contains("MULTIPLE")) ? textInfo.ToTitleCase(a.EndDate.ToLower())
                            : stringNullCheck(a.EndTime) != String.Empty ? textInfo.ToTitleCase(a.EndTime.ToLower()) : "",



                totalDaysOrHrs = a.ODDuration.ToUpper().Contains("SINGLE") ? stringNullCheck(a.TotalHours) != string.Empty ?
                a.TotalHours.Substring(0, 5) + " hrs" : ""
                : a.TotalDays + " d" ?? "1 d",
                reason = a.Remarks,
                applicationType = "On Duty Application",
                status = a.Status //getApprovalHistoryStatusDesc(a,staffId,"OD")
                //,photoUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority) + $"/api/user/GetStaffPhotoImage/{a.StaffId}"
            });
            return ODApprovedHistory; ;
        }

        private dynamic GetPermissionApprovedHistory(List<RAPermissionApplication> lst, string staffId)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            var perApprovedHistory = lst.Take(10).Select(a => new
            {
                id = a.Id,
                staffId = a.StaffId,
                staffName = a.StaffName,
                desc = textInfo.ToTitleCase(a.Type.ToLower()),
                startTime = textInfo.ToTitleCase(a.Date.ToLower()) + " " + stringNullCheck(a.StartTime),
                endTime = textInfo.ToTitleCase(a.Date.ToLower()) + " " + stringNullCheck(a.EndTime),
                totalDaysOrHrs = a.TotalHours.Substring(0, 5),
                reason = a.Remarks,
                applicationType = "Permission Application",
                status = a.Status //getApprovalHistoryStatusDesc(a,staffId,"PO")  
                //,photoUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority) + $"/api/user/GetStaffPhotoImage/{a.StaffId}"
            });
            return perApprovedHistory; ;
        }

        private dynamic GetOnDutyApprovals(List<AllPendingApprovals> approvals)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            return approvals.Where(a => a.ParentType == "OD").Select(a => new
            {
                id = a.ApplicationId,
                staffId = a.StaffId,
                staffName = a.FirstName,
                desc = $"On Duty - {textInfo.ToTitleCase(a.ODDuration)}",
                fromDate = stringNullCheck(a.StartDate) != String.Empty ? textInfo.ToTitleCase(a.StartDate.ToLower()) : "",
                endDate = stringNullCheck(a.EndDate) != String.Empty ? textInfo.ToTitleCase(a.EndDate.ToLower()) : "",
                //totalDaysOrHrs = a.OD?? "NA",
                totalDaysOrHrs = a.ODDuration.ToUpper().Contains("SINGLE") ?
                                    stringNullCheck(a.TotalHours) != string.Empty ? a.TotalHours.Substring(0, 5) + " hrs" : ""
                                : a.TotalDays + " d" ?? "1 d",
                reason = a.Reason,
                // applicationType = a.ParentType,
                applicationType = $"On Duty - {textInfo.ToTitleCase(a.ODDuration)}",
                status = "Pending", //getApprovalStatusDesc(a)
                photoUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority) + $"/api/user/GetStaffPhotoImage/{a.StaffId}"

            });
        }

        private dynamic GetBusinessTravelApprovals(List<AllPendingApprovals> approvals)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            return approvals.Where(a => a.ParentType == "BT").Select(a => new
            {
                // return approvals.Where(a => a.A == "BT").Select(a => new {
                id = a.ApplicationId,
                staffId = a.StaffId,
                staffName = a.FirstName,
                desc = $"Business Travel - {textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                fromDate = stringNullCheck(a.StartDate) != String.Empty ? textInfo.ToTitleCase(a.StartDate.ToLower()) : "",
                endDate = stringNullCheck(a.EndDate) != String.Empty ? textInfo.ToTitleCase(a.EndDate.ToLower()) : "",
                //totalDaysOrHrs = a.OD ?? "NA",
                totalDaysOrHrs = a.ODDuration.ToUpper().Contains("SINGLE") ? stringNullCheck(a.TotalHours) != String.Empty ? a.TotalHours.Substring(0, 5) + " hrs" : ""
                : a.TotalDays + " d" ?? "1 d",
                reason = a.Reason,
                //applicationType = a.ParentType,
                applicationType = $"Business Travel - {textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                status = "Pending", //getApprovalStatusDesc(a)
                photoUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority) + $"/api/user/GetStaffPhotoImage/{a.StaffId}"

            });
        }

        private dynamic GetWorkFromHomeApprovals(List<AllPendingApprovals> approvals)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            return approvals.Where(a => a.ParentType == "WFH").Select(a => new
            {
                // return approvals.Where(a => a.A == "BT").Select(a => new {
                id = a.ApplicationId,
                staffId = a.StaffId,
                staffName = a.FirstName,
                desc = $"Work From Home - {textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                fromDate = stringNullCheck(a.StartDate) != String.Empty ? textInfo.ToTitleCase(a.StartDate.ToLower()) : "",
                endDate = stringNullCheck(a.EndDate) != String.Empty ? textInfo.ToTitleCase(a.EndDate.ToLower()) : "",
                //totalDaysOrHrs = a.OD ?? "NA",
                totalDaysOrHrs = a.ODDuration.ToUpper().Contains("SINGLE") ? stringNullCheck(a.TotalHours) != String.Empty ? a.TotalHours.Substring(0, 5) + " hrs" : ""
                : a.TotalDays + " d" ?? "1 d",
                reason = a.Reason,
                //applicationType = a.ParentType,
                applicationType = $"Work From Home - {textInfo.ToTitleCase(a.ODDuration.ToLower())}",
                status = "Pending", //getApprovalStatusDesc(a)
                photoUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority) + $"/api/user/GetStaffPhotoImage/{a.StaffId}"

            });
        }

        private dynamic GetPendingManualPunchApproval(List<AllPendingApprovals> approvals)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return approvals.Where(a => a.ParentType == "MP").Select(a => new
            {
                id = a.ApplicationId,
                staffId = a.StaffId,
                staffName = a.FirstName,
                desc = $"Manual Punch for {a.PunchType} punch",
                fromDate = $"{ textInfo.ToTitleCase(stringNullCheck(a.InTime).ToLower())}".Trim(),
                endDate = $"{textInfo.ToTitleCase(stringNullCheck(a.OutTime).ToLower())}".Trim(),
                startTime = $"{ textInfo.ToTitleCase(stringNullCheck(a.InTime).ToLower())}".Trim(),
                endTime = $"{ textInfo.ToTitleCase(stringNullCheck(a.OutTime).ToLower())}".Trim(),
                totalDaysOrHrs = "NA",
                reason = a.Reason,
                //applicationType = a.ParentType,
                applicationType = $"Manual Punch for {a.PunchType} punch",
                status = "Pending", //getApprovalStatusDesc(a)
                photoUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority) + $"/api/user/GetStaffPhotoImage/{a.StaffId}"

            });
        }

        private dynamic GetPendingPermissionApproval(List<AllPendingApprovals> approvals)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return approvals.Where(a => a.ParentType == "PO").Select(a => new
            {
                id = a.ApplicationId,
                staffId = a.StaffId,
                staffName = a.FirstName,
                desc = a.Name,
                fromDate = $"{ a.StartDate} {a.FromTime}:00",
                endDate = $"{ a.StartDate} {a.TimeTo}:00",

                totalDaysOrHrs = a.TotalHours,
                reason = a.Reason,
                //applicationType = a.ParentType,
                applicationType = a.Name,
                status = "Pending", //getApprovalStatusDesc(a)
                photoUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority) + $"/api/user/GetStaffPhotoImage/{a.StaffId}"
            });
        }


        string getApprovalStatusDesc(AllPendingApprovals appln)
        {
            string status = "";

            if (appln.ApprovalStatusName.ToUpper().Equals("REJECTED"))
            {
                //status = "Rejected - Approver";
                status = "Rejected(A)";

            }
            else if (appln.ApprovalStatusName.ToUpper().Equals("PENDING"))
            {
                //status = "Pending - Approver";
                status = "Pending(A)";
            }
            else if (appln.ReviewerStatusName.ToUpper().Equals("REJECTED"))
            {
                //status = "Rejected - Reviewer";
                status = "Rejected(R)";
            }
            else if (appln.ReviewerStatusName.ToUpper().Equals("PENDING"))
                //status = "Pending - Reviewer";
                status = "Pending(R)";
            else
            {
                //status = "Approved - Reviewer";
                status = "Approved(R)";
            }
            return status;
        }

        private dynamic GetPendingLeaveApprovals(List<AllPendingApprovals> approvals)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            var retVal = approvals.Where(a => a.ParentType == "LA").Select(a => new
            {
                id = a.ApplicationId,
                staffId = a.StaffId,
                staffName = a.FirstName,
                desc = textInfo.ToTitleCase(a.LeaveTypeName.ToLower()),
                fromDate = textInfo.ToTitleCase(a.StartDate.ToLower()),
                endDate = textInfo.ToTitleCase(a.EndDate.ToLower()),
                totalDaysOrHrs = a.TotalDays,
                reason = a.Reason,
                //applicationType = a.ParentType,
                applicationType = textInfo.ToTitleCase(a.LeaveTypeName.ToLower()),
                status = "Pending", //getApprovalStatusDesc(a)
                photoUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority) + $"/api/user/GetStaffPhotoImage/{a.StaffId}"
            });
            return retVal;
        }

        string getApprovedHistoryStatusDesc(RAPermissionApplication appln)
        {
            string status = "";

            if (appln.ApproverStatus.ToUpper().Equals("REJECTED"))
            {
                //status = "Rejected - Approver";
                status = "Rejected(A)";

            }
            else if (appln.ApproverStatus.ToUpper().Equals("PENDING"))
            {
                //status = "Pending - Approver";
                status = "Pending(A)";
            }
            else if (appln.ReviewerStatus.ToUpper().Equals("REJECTED"))
            {
                //status = "Rejected - Reviewer";
                status = "Rejected(R)";
            }
            else if (appln.ReviewerStatus.ToUpper().Equals("PENDING"))
                //status = "Pending - Reviewer";
                status = "Pending(R)";
            else
            {
                //status = "Approved - Reviewer";
                status = "Approved(R)";
            }
            return status;
        }
        async Task<List<AllPendingApprovals>> GetAllPendingApprovals(string staffId, string aplnType = "")
        {
            var bl = new LandingPageBusinessLogic();
            var applnTypeShort = GetShortNotationFor(aplnType);
            var lst = bl.GetAllPendingApplications(staffId, applnTypeShort);
            return lst;
        }

        string GetShortNotationFor(string applnType)
        {
            string retVal = "PO,MP,LA,OD,BT,WFH,CR,CO"; //ALL
            if (applnType.ToUpper().Equals("LEAVE"))
            {
                retVal = "LA";

            }
            else if (applnType.ToUpper().Equals("PERMISSION"))
            {
                retVal = "PO";
            }
            else if (applnType.ToUpper().Equals("MANUALPUNCH"))
            {
                retVal = "MP";
            }
            else if (applnType.ToUpper().Equals("ONDUTY"))
            {
                retVal = "OD";
            }
            else if (applnType.ToUpper().Equals("BUSINESSTRAVEL"))
            {
                retVal = "BT";
            }
            else if (applnType.ToUpper().Equals("WORKFROMHOME"))
            {
                retVal = "WFH";
            }
            else if (applnType.ToUpper().Equals("COFFREQ"))
            {
                retVal = "CR";
            }
            else if (applnType.ToUpper().Equals("COFFAVAIL"))
            {
                retVal = "CO";
            }

            return retVal;
        }


        [HttpGet]
        [Route("{ApplicationType}/{Status}/{ReqId}")]
        // [Route("{ApplicationType}/{ReqId}/{Status}")]
        [Authorize]
        public IHttpActionResult ApproveOrRejectApplication(string ApplicationType, string Status, string ReqId)
        {
            var staffId = GetLoggedInUserId();
            var statusCode = GetStatusCode(Status);
            var applnType = GetShortNotationFor(ApplicationType);
            var result = ApproveApplication(ReqId, statusCode, staffId, applnType);
            //OD ,  CR ,SE ,MP ,LA ,CP ,PO ,OT ,BT,
            return Ok(result);
        }
        private string GetStatusCode(string status)
        {
            if (status.Trim().ToUpper().Equals("APPROVE"))
            {
                return "2";
            }
            else if (status.Trim().ToUpper().Equals("REJECT"))
                return "3";
            else
                throw new ApplicationException("Invalid approval status");
        }
        private string GetLoggedInUserId()
        {
            var identity = (ClaimsIdentity)User.Identity;
            UserClaims uc = UserHelper.GetUserClaims(identity);
            return uc.StaffId;
        }
        string stringNullCheck(string input)
        {
            string returnval = string.Empty;
            if (String.IsNullOrEmpty(input))
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

        public string ApproveApplication(string ApprovalId, string ApprovalStatusId, string ApproverId, string ParentType)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var user = UserHelper.GetUserClaims(identity);
                var bl = new LandingPageBusinessLogic();
                var LocationId = user.LocationId;
                int approvalStatusId = Convert.ToInt16(ApprovalStatusId);

                //bl.ApproveApplication(ApprovalId, Convert.ToInt16(ApprovalStatusId), ApproverId);
                RALeaveApplicationBusinessLogic BL = new RALeaveApplicationBusinessLogic();
                RACoffCreditApplicationBusinessLogic CBL = new RACoffCreditApplicationBusinessLogic();
                RAOnDutyApplicationBusinessLogic OBL = new RAOnDutyApplicationBusinessLogic();
                RAManualPunchApplicationBusinessLogic MBL = new RAManualPunchApplicationBusinessLogic();
                RAPermissionApplicationBusinessLogic PBL = new RAPermissionApplicationBusinessLogic();
                OTApplicationBusinessLogic OT = new OTApplicationBusinessLogic();
                string[] values = ApprovalId.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    var apprid = values[i].Trim();
                    if (ApprovalStatusId.Equals("2"))
                    {
                        if (ParentType == "LA")
                        {
                            BL.ApproveApplication(apprid, ApproverId);
                        }
                        else if (ParentType == "PO")
                        {
                            PBL.ApproveApplication(apprid, ApproverId);
                            //PBL.ApproveApplication(apprid, ApproverId, LocationId);
                        }
                        else if (ParentType == "CR")
                        {
                            //CBL.ApproveApplication(apprid, ApproverId, LocationId);
                            CBL.ApproveApplication(apprid, ApproverId);
                        }
                        else if (ParentType == "MP")
                        {
                            //MBL.ApproveApplication(apprid, ApproverId);

                            MBL.ApproveApplication(apprid, ApproverId, LocationId);
                        }
                        else if (ParentType == "OD" || ParentType == "BT" || ParentType == "WFH")
                        {
                            //<>OBL.ApproveApplication(apprid, ApproverId, LocationId, ParentType);
                            OBL.ApproveApplication(apprid, ApproverId, ParentType);
                        }
                        else if (ParentType == "COA")
                        {
                            //<>CBL.ApproveCoffAvaillingApplication(apprid, ApproverId, ""); //Rajesh Session["StaffId"].ToString()
                            CBL.ApproveApplication(apprid, ApproverId);
                        }
                        else if (ParentType == "OT")
                        {
                            OT.ApproveApplication(apprid, ApproverId);
                        }
                        else
                        {
                            bl.ApproveApplication(apprid, Convert.ToInt32(ApprovalStatusId), ApproverId);
                        }
                    }
                    else if (ApprovalStatusId.Equals("3"))
                    {
                        //BL.RejectApplication(ApprovalId, ApproverId);
                        if (ParentType == "LA")
                        {
                            BL.RejectApplication(apprid, ApproverId);
                            //  BL.RejectApplication(apprid, ApproverId, LocationId);
                        }
                        else if (ParentType == "PO")
                        {
                            //PBL.RejectApplication(apprid, ApproverId, LocationId);
                            PBL.RejectApplication(apprid, ApproverId);
                        }
                        else if (ParentType == "CR")
                        {
                            //CBL.RejectApplication(apprid, ApproverId, LocationId);
                            CBL.RejectApplication(apprid, LocationId);
                        }
                        else if (ParentType == "COA")
                        {
                            //<>CBL.RejectCoffAvaillingApplication(apprid, ApproverId);
                            CBL.RejectApplication(apprid, ApproverId);
                        }
                        else if (ParentType == "MP")
                        {
                            MBL.RejectApplication(apprid, ApproverId, LocationId);
                        }
                        else if (ParentType == "OD" || ParentType == "BT")
                        {
                            // OBL.RejectApplication(apprid, ApproverId, LocationId, ParentType);
                            OBL.RejectApplication(apprid, ApproverId, ParentType);
                        }
                        else if (ParentType == "OT")
                        {
                            OT.RejectApplication(apprid, ApproverId);
                        }
                        else
                        {
                            bl.ApproveApplication(apprid, Convert.ToInt32(ApprovalStatusId), ApproverId);
                        }
                    }
                }
                return "OK";
            }
            catch (Exception err)
            {
                return "ERROR! " + err.Message;
            }
        }
    }
}