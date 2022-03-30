using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceManagement.Models;

namespace Attendance.BusinessLogic
{
    public class SecuritygroupTxnsBusinessLogic
    {
        public List<MapSecurityScreentoRoleList> GetAllRoleGroups()
        {
            using (SecurityGroupRepository securityGroupRepository = new SecurityGroupRepository())
            {
                return securityGroupRepository.GetAllRoleGroup();
            }
        }

        public List<SecurityGroupTxnsList> GetAllSecurityGroups()
        {
            using (SecurityGroupRepository securityGroupRepository = new SecurityGroupRepository())
            {
                return securityGroupRepository.GetAllSecurityGroup();
            }
        }

        public List<MapSecurityScreentoRoleList> GetAllSecurityGroupTxns(string id)
        {
            using (SecurityGroupRepository securityGroupRepository = new SecurityGroupRepository())
            {
                return securityGroupRepository.GetAllSecurityGroupTxns(id);
            }
        }

        public string SaveRoleGroupTxnsInfos(SecurityGroupTxns sgt, string operationMode)
        {
            using (SecurityGroupRepository securityGroupRepository = new SecurityGroupRepository())
        {
                securityGroupRepository.SaveRoleGroupTxnsInfo(sgt, operationMode);
            }
            return null;
        }

        public string SaveRoleInfos(SecurityGroup sg)
        {
            using (SecurityGroupRepository securityGroupRepository = new SecurityGroupRepository())
            {
                return securityGroupRepository.SaveRoleInfos(sg);
            }
        }
        public List<SecurityGroup> LoadRoleId()
        {
            using (SecurityGroupRepository securityGroupRepository = new SecurityGroupRepository())
            {
                return securityGroupRepository.LoadRoleId();
            }
        }
    }
}
