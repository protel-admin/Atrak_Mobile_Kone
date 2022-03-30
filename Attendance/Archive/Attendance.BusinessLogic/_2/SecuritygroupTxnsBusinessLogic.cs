﻿using Attendance.Model;
using Attendance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.BusinessLogic
{
    public class SecuritygroupTxnsBusinessLogic
    {
        public List<MapSecurityScreentoRoleList> GetAllRoleGroups()
        {
            var CompRepo = new SecurityGroupRepository();
            return CompRepo.GetAllRoleGroup();
        }

        public List<SecurityGroupTxnsList> GetAllSecurityGroups()
        {
            var CompRepo = new SecurityGroupRepository();
            return CompRepo.GetAllSecurityGroup();
        }

        public List<MapSecurityScreentoRoleList> GetAllSecurityGroupTxns(string id)
        {
            var CompRepo = new SecurityGroupRepository();
            return CompRepo.GetAllSecurityGroupTxns(id);
        }

        public string SaveRoleGroupTxnsInfos(SecurityGroupTxns sgt)
        {
            var repo = new SecurityGroupRepository();
            repo.SaveRoleGroupTxnsInfo(sgt);
            return null;
        }

        public string SaveRoleInfos(SecurityGroup sg)
        {
            var repo = new SecurityGroupRepository();
            return repo.SaveRoleInfos(sg);

        }
    }
}
