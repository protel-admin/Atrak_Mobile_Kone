﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Attendance.Model;
using Attendance.Repository;
using Newtonsoft.Json;

namespace Attendance.BusinessLogic
{
    public class AssociationBusinessLogic
    {
        public void SaveInformation(string id, string combination, string gender, string workingdaypattern, string priority, string parentid, string parenttype, string isactive)
        {
            using (AssociationRepository associationRepository = new AssociationRepository())
            {
            associationRepository.SaveInformation(id, combination, gender, workingdaypattern, priority, parentid, parenttype, isactive);
     
            }

            //
        }

        public string LoadPolicies( string ParentType )
        {
            string result = string.Empty;
            List<AssociationList> associations = new List<AssociationList>();
            using (AssociationRepository associationRepository = new AssociationRepository())
            {
                associations = associationRepository.LoadPolicies(ParentType);
            }
            result = ConvertPoliciesToJSon(associations);
            return result;
        }

        public string ConvertPoliciesToJSon(List<AssociationList> lst )
        {
            var jsontemp = new StringBuilder();
            var jsonstring = string.Empty;

            foreach (var d in lst)
            {
                jsontemp.Append(JsonConvert.SerializeObject(new AssociationList()
                {
                    Id = d.Id,
                    Combination = d.Combination,
                    ParentId = d.ParentId,
                    ParentType = d.ParentType,
                    Priority = d.Priority,
                    WorkingDayPattern = d.WorkingDayPattern,
                    Gender = d.Gender,
                    IsActive = d.IsActive
                }));
                jsontemp.Append(",");
            }

            jsonstring = jsontemp.ToString();

            if (string.IsNullOrEmpty(jsonstring) == false)
            {
                if (jsonstring.EndsWith(",") == true)
                {
                    jsonstring = jsonstring.Substring(0, jsonstring.Length - 1);
                }
            }
            return "[" + jsonstring + "]";
        }

        public List<SelectListItem> LoadLeaveGroup()
        {
            List<LeaveGroupList> leaveGroups = new List<LeaveGroupList>();

            using (AssociationRepository associationRepository = new AssociationRepository())
            {
                leaveGroups = associationRepository.LoadLeaveGroup();
            }
            var items = new List<SelectListItem>();

                items = leaveGroups.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> LoadHolidayZone()
        {
            List<HolidayZoneList> holidayZones = new List<HolidayZoneList>();
            using (AssociationRepository associationRepository = new AssociationRepository())
            {
                holidayZones = associationRepository.LoadHolidayZone(); 
            }
            var items = new List<SelectListItem>();

            items = holidayZones.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> LoadWorkingDayPattern()
        {
            List<WorkingDayPatternList> workingDayPatterns = new List<WorkingDayPatternList>();
            using (AssociationRepository associationRepository = new AssociationRepository())
            {
                workingDayPatterns = associationRepository.LoadWorkingDayPattern();
            }
            var items = new List<SelectListItem>();

            items = workingDayPatterns.Select(d => new SelectListItem()
            {
                Text = d.WorkingPattern.ToString(),
                Value = d.WorkingPattern.ToString() ,
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> LoadWeeklyOff()
        {
            List<WeeklyOffList> weeklyOffs = new List<WeeklyOffList>();
            using (AssociationRepository associationRepository = new AssociationRepository())
            {
                weeklyOffs = associationRepository.LoadWeeklyOff();
            }
            var items = new List<SelectListItem>();

            items = weeklyOffs.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> LoadPolicy()
        {
            List<RuleGroupList> ruleGroups = new List<RuleGroupList>();
            using (AssociationRepository associationRepository = new AssociationRepository())
            {
                ruleGroups = associationRepository.LoadPolicy();
            }

            var items = new List<SelectListItem>();

            items = ruleGroups.Select(d => new SelectListItem()
            {
                Text = d.name,
                Value = d.id.ToString(),
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> LoadCompany()
        {
            List<CompanyList> companies = new List<CompanyList>();
            using (AssociationRepository associationRepository = new AssociationRepository())
            {
                companies = associationRepository.LoadCompany();
            }

            var items = new List<SelectListItem>();

            items = companies.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> LoadBranch()
        {
            List<BranchList> branches = new List<BranchList>();
            using (AssociationRepository associationRepository = new AssociationRepository())
            {
                branches = associationRepository.LoadBranch();
            }


            var items = new List<SelectListItem>();

            items = branches.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> LoadDepartment()
        {

            List<DepartmentList> departments = new List<DepartmentList>();
            using (AssociationRepository associationRepository = new AssociationRepository())
            {
                departments = associationRepository.LoadDepartment();
            }

            var items = new List<SelectListItem>();

            items = departments.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> LoadDivision()
        {
            List<DivisionList> divisions = new List<DivisionList>();
            using (AssociationRepository associationRepository = new AssociationRepository())
            {
                divisions = associationRepository.LoadDivision();
            }

            var items = new List<SelectListItem>();

            items = divisions.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> LoadDesignation()
        {
            List<DesignationList> designations = new List<DesignationList>();
            using (AssociationRepository associationRepository = new AssociationRepository())
            {
                designations = associationRepository.LoadDesignation();
            }


            var items = new List<SelectListItem>();

            items = designations.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }

        public List<SelectListItem> LoadGrade()
        {

            List<GradeList> grades = new List<GradeList>();
            using (AssociationRepository associationRepository = new AssociationRepository())
            {
                grades = associationRepository.LoadGrade();
            }

            var items = new List<SelectListItem>();

            items = grades.Select(d => new SelectListItem()
            {
                Text = d.Name,
                Value = d.Id,
                Selected = true
            }).ToList();

            return items;
        }
    }
}
