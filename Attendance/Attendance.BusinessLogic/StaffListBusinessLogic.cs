using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;
using Newtonsoft.Json;

namespace Attendance.BusinessLogic {
    public class StaffListBusinessLogic {
        public List<StaffList> GetStaffList(string criteria, string terminatedcriteriastring, string LocationId, string UserRole)
        {
            var qryStr = new StringBuilder();
            using (var vrepo = new StaffListRepository())
            {
                var Qry = string.Empty;

                Qry = ("Select staffid , DBO.FNGETSTAFFNAME(a.STAFFID) as staffname ," +
                    " DBO.FNGETMASTERNAME(a.STAFFID , 'DP') as Department, DBO.FNGETMASTERNAME(STAFFID , 'L') as" +
                    " Location from StaffOfficial a inner join Staff b on a.staffid = b.Id WHERE 1=1 " + criteria);
                if (UserRole == "3")
                {
                    Qry = Qry + $" and LocationId='{LocationId}'";
                }
                //qryStr.Append("Select staffid , DBO.FNGETSTAFFNAME(STAFFID) as staffname , DBO.FNGETMASTERNAME(STAFFID , 'DP') as Department, DBO.FNGETMASTERNAME(STAFFID , 'L') as Location from StaffOfficial a inner join Staff b on a.staffid = b.id WHERE 1=1 AND b.IsHidden = 0 AND b.StaffStatusId = 1" + criteria);
                if (terminatedcriteriastring != "")
                {
                    Qry += $@" union all Select staffid , DBO.FNGETSTAFFNAME(a.STAFFID) as staffname , DBO.FNGETMASTERNAME(a.STAFFID , 'DP') as Department, DBO.FNGETMASTERNAME(STAFFID , 'L') as
                 Location from StaffOfficial a inner join Staff b on a.staffid = b.Id Where 1=1  {criteria } and StaffStatusId not in (1,2)  and IsHidden = 0 ";
                    if (UserRole == "3")
                    {
                        Qry = Qry + $" and LocationId='{LocationId}'";
                    }
                    Qry += $"{terminatedcriteriastring}";

                }
                var lst = vrepo.GetAllStaffLists(Qry);

                return lst;
            }
        }

        public string ConvertToJsonString(List<StaffList> lst)
        {

            try
            {
                var jsonstring = new StringBuilder();
                var a = "";
                foreach (var v in lst)
                {
                    if (v != null)
                    {
                        jsonstring.Append(Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            staffid = v.StaffId,
                            staffname = v.StaffName,
                            department = v.Department,
                            location = v.Location
                        }));
                    }
                    

                    jsonstring.Append(",");
                }

                if (jsonstring.Length > 0)
                {
                    //jsonstring.remo
                    a = jsonstring.ToString();
                    a = a.Substring(0, a.Length - 1);
                }
                return a;

            }
            catch (Exception e)
            {
                throw e;
            }
            
            //if ( !string.IsNullOrEmpty(jsonstring ) )
            //    if ( jsonstring.EndsWith( "," ) )
            //        jsonstring = jsonstring.Substring( jsonstring.Length - 1 );

            
        }


        public List<SecurityGroupList> GetUserRoleList(string criteria)
        {
            var qryStr = new StringBuilder();
            using (var vrepo = new StaffListRepository())
            { 

                qryStr.Clear();
            qryStr.Append("select id , name as name  from securitygroup where 1 = 1 " + criteria);

            var lst = vrepo.GetAllUserRoleLists(qryStr.ToString());

            return lst;
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

            using (var vrepo = new StaffListRepository())
            { 
            var lst = vrepo.GetStaffBasedonrep(criteria, ReportingMnager);
            return lst;
        }
        }

        public List<StaffList> GetStaffBasedonrep(string ReportingManager)
        {
            using (var vrepo = new StaffListRepository())
            { 
            var lst = vrepo.GetStaffBasedonrep(ReportingManager);
            return lst;
            }
        }
        }
    }

