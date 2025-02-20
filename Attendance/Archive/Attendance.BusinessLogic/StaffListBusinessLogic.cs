﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using Newtonsoft.Json;

namespace Attendance.BusinessLogic {
    public class StaffListBusinessLogic {
        public List<StaffList> GetStaffList(string criteria,string role,string location)
         {
            var qryStr = new StringBuilder();

            qryStr.Clear();
            if(role=="3"||role=="5")
              {
            qryStr.Append("Select staffid , DBO.FNGETSTAFFNAME(STAFFID) as staffname , DBO.FNGETMASTERNAME(STAFFID , 'DP') as Department, DBO.FNGETMASTERNAME(STAFFID , 'L') as Location from StaffOfficial a inner join Staff b on a.staffid = b.id WHERE 1=1 AND b.IsHidden = 0 " + criteria);
              }
            else
            {
                qryStr.Append("Select staffid , DBO.FNGETSTAFFNAME(STAFFID) as staffname , DBO.FNGETMASTERNAME(STAFFID , 'DP') as Department, DBO.FNGETMASTERNAME(STAFFID , 'L') as Location from StaffOfficial a inner join Staff b on a.staffid = b.id WHERE 1=1 AND b.IsHidden = 0 AND LocationId='" + location + "' " + criteria);
            }
            using (StaffListRepository staffListRepository = new StaffListRepository())
            {
                 return staffListRepository.GetAllStaffLists(qryStr.ToString());
            }

        }

        public string ConvertToJsonString(List<StaffList> lst)
        {
            var jsonstring = new StringBuilder();
            var a = "";
            foreach ( var v in lst ) {
                jsonstring.Append(JsonConvert.SerializeObject ( new {
                    staffid = v.StaffId ,
                    staffname = v.StaffName ,
                    department = v.Department ,
                    location = v.Location
                }));

                jsonstring.Append(",");
            }

            if (jsonstring.Length > 0)
            {
                //jsonstring.remo
                a = jsonstring.ToString();
                a = a.Substring (0, a.Length - 1 );
            }

            //if ( !string.IsNullOrEmpty(jsonstring ) )
            //    if ( jsonstring.EndsWith( "," ) )
            //        jsonstring = jsonstring.Substring( jsonstring.Length - 1 );

            return a;
        }

        public List<SecurityGroupList> GetUserRoleList(string criteria)
        {
            var qryStr = new StringBuilder();

            qryStr.Clear();
            qryStr.Append("select id , name as name  from securitygroup where 1 = 1 " + criteria);
            using (StaffListRepository staffListRepository = new StaffListRepository())
            {
                return staffListRepository.GetAllUserRoleLists(qryStr.ToString());
            }

        }

        public string ConvertToJsonStringUserRole(List<SecurityGroupList> lst)
        {
            var jsonstring = new StringBuilder();
            var a = "";
            foreach (var v in lst)
            {
                jsonstring.Append(JsonConvert.SerializeObject(new
                {
                    id = v.Id,
                    name = v.Name,
                }));

                jsonstring.Append(",");
            }

            if (jsonstring.Length > 0)
            {
                //jsonstring.remo
                a = jsonstring.ToString();
                a = a.Substring(0, a.Length - 1);
            }

            //if ( !string.IsNullOrEmpty(jsonstring ) )
            //    if ( jsonstring.EndsWith( "," ) )
            //        jsonstring = jsonstring.Substring( jsonstring.Length - 1 );

            return a;
        }

        public List<StaffList> GetStaffBasedonrep(string criteria, string ReportingMnager)
        {
          
            using (StaffListRepository staffListRepository = new StaffListRepository())
            {
                return staffListRepository.GetStaffBasedonrep(criteria, ReportingMnager);
            }
        }

        public List<StaffList> GetStaffBasedonrep(string ReportingManager)
        {
            using (StaffListRepository staffListRepository = new StaffListRepository())
            {
                return staffListRepository.GetStaffBasedonrep(ReportingManager);

            }
        }
        }
    }

