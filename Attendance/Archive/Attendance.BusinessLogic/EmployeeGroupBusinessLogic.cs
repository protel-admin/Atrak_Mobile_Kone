﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class EmployeeGroupBusinessLogic
    {
        public EmployeeGroup GetGroupDetails(string id)
        {
            using (EmployeeGroupRepository employeeGroupRepository = new EmployeeGroupRepository())
            {
               return employeeGroupRepository.GetGroupDetails(id);
            }
        }

        public List<EmployeeGroupView> GetAllEmployeeGroups()
        {
            List<EmployeeGroupView> employeeGroupViews = new List<EmployeeGroupView>();
            using (EmployeeGroupRepository employeeGroupRepository = new EmployeeGroupRepository())
            {
                employeeGroupViews =  employeeGroupRepository.GetAllEmployeeGroups();
            }if(employeeGroupViews.Count() > 0)
            {
                return employeeGroupViews;
            }
            else
            {
                return new List<EmployeeGroupView>();
            }
        }

        public List<EmployeeGroupTxnView> GetAllEmployeesInGroup(string id)
        {
            List<EmployeeGroupTxnView> employeeGroupTxns = new List<EmployeeGroupTxnView>();
            using (EmployeeGroupRepository employeeGroupRepository = new EmployeeGroupRepository())
            {
                employeeGroupTxns = employeeGroupRepository.GetAllEmployeesInGroup(id);
            }
            if (employeeGroupTxns.Count() > 0)
            {
                return employeeGroupTxns;
            }
            else
            {
                return new List<EmployeeGroupTxnView>();
            }
        }

        public string SaveGroupList(string groupid, string groupname, string isactive ,string oldstaffs, string newstaffs)
        {
            //rmove common staffs from both the string just to save processing time.
            RemoveCommonStaffs(ref oldstaffs, ref newstaffs);

            //remove the ending coma from the old string.
            if (oldstaffs.EndsWith(","))
                oldstaffs = oldstaffs.Substring(0, oldstaffs.Length - 1);

            //remove the ending coma from the new string.
            if (newstaffs.EndsWith(","))
                newstaffs = newstaffs.Substring(0, newstaffs.Length - 1);

            using (EmployeeGroupRepository employeeGroupRepository = new EmployeeGroupRepository())
            {
                employeeGroupRepository.saveemployeetogroup(oldstaffs, newstaffs, groupid, groupname, isactive);
            }
            return string.Empty;
        }

        public void RemoveCommonStaffs(ref string oldstaff, ref string newstaff)
        {
            //split the old staff string into array.
            var a = oldstaff.Split(',');
            if (a.Length > 0)
            {
                //loop through each item in the array
                foreach ( var b in a ) {
                    //check if the item is found in both the strings.
                    if ( oldstaff.Contains ( b + "," ) == true && newstaff.Contains ( b + "," ) == true )//if found then...
                    {
                        if(b!=string.Empty) { 
                            //remove the item from both the strings.
                            oldstaff = oldstaff.Replace(b + ",",string.Empty);
                            newstaff = newstaff.Replace(b + ",", string.Empty);
                        }
                    }
                }
            }
        }
    }
}
