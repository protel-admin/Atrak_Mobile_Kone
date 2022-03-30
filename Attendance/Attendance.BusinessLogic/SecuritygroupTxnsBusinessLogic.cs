using Attendance.Model;
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

        public List<SecurityGroupTxnsList> GetAllSecurityGroups(string staffid)
        {
            var CompRepo = new SecurityGroupRepository();
            return CompRepo.GetAllSecurityGroup(staffid);
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
        #region
          public List<ScreensModel> GetAllScreens()
          {
              var CompRepo = new SecurityGroupRepository();
              return CompRepo.GetAllScreens();
          }
          public ScreensModel GetSelectedScreenIdDetails(string Id)
          {
              var CompRepo = new SecurityGroupRepository();
              return CompRepo.GetSelectedScreenIdDetails(Id);
          }
          public string AddorUpdateScreenMaster(ScreensModel Model)
          {
              var CompRepo = new SecurityGroupRepository();
              return CompRepo.AddorUpdateScreenMaster(Model);
          }
        #endregion

    }
}
