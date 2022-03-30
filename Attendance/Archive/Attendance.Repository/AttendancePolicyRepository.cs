using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using Attendance.Model;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Attendance.Repository
{
    public class AttendancePolicyRepository : IDisposable
    {
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }
            disposed = true;
        }
        private AttendanceManagementContext context;

        public AttendancePolicyRepository()
        {
            context = new AttendanceManagementContext();
        }

        public List<CompanyRule> GetCompany()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("Select  Id,Name from Company where IsActive = 1");
            try
            {
                var lst = context.Database.SqlQuery<CompanyRule>(qryStr.ToString()).Select(d => new CompanyRule()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

                if (lst == null)
                {
                    return new List<CompanyRule>();
                }
                else
                {
                    return lst;
                }
            }
            catch (Exception)
            {
                return new List<CompanyRule>();
            }
        }

        public string GetComapnyName(string StaffId)
        {
          
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append(" select Name from company where id in (select companyid from staffofficial where staffid= @StaffId) ");
            try
            {
                var Result = context.Database.SqlQuery<string>(qryStr.ToString(), new SqlParameter("@StaffId", StaffId)).FirstOrDefault().ToString();

                return Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CompanyList> GetAllCompany()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();
            qryStr.Append("select Id , Name from company");

            try
            {
                var lst = context.Database.SqlQuery<CompanyList>(qryStr.ToString()).Select(d => new CompanyList()
                {
                    Id = d.Id,
                    Name = d.Name,
                    ShortName = d.ShortName,
                    LegalName = d.LegalName,
                    Website = d.Website,
                    RegisterNo = d.RegisterNo,
                    TNGSNo = d.TNGSNo,
                    CSTNo = d.CSTNo,
                    TINNo = d.TINNo,
                    ServiceTaxNo = d.ServiceTaxNo,
                    PANNo = d.PANNo,
                    PFNo = d.PFNo,
                    IsActive = d.IsActive
                }).ToList();

                if (lst == null)
                {
                    return new List<CompanyList>();
                }
                else
                {
                    return lst;
                }

            }
            catch (Exception)
            {
                return new List<CompanyList>();
            }
        }

        public List<RuleGroupTxnsList> GetAllRule()
        {
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select R.id as RId,name as RName ,R.isactive  from [Rule] R where isactive=1");
            try
            {
                var lstoffstaff = context.Database.SqlQuery<RuleGroupTxnsList>(qryStr.ToString())
                        .Select(c => new RuleGroupTxnsList()
                        {

                            RId = c.RId,
                            RName = c.RName,
                            Companyid = c.Companyid,
                            IsActive = true

                        }
                 ).ToList();

                if (lstoffstaff == null)
                {
                    return new List<RuleGroupTxnsList>();
                }
                else
                {
                    return lstoffstaff;
                }
            }
            catch (Exception)
            {
                return new List<RuleGroupTxnsList>();
                throw;
            }
        }

        public List<RuleGroupTxnsList> GetRule(string id)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@id", id);
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("select R.id as RId,name as RName ,R.isactive  from [Rule] R where id in ( @id )");
            try
            {
                var lstoffstaff = context.Database.SqlQuery<RuleGroupTxnsList>(qryStr.ToString(), sqlParameter)
                        .Select(c => new RuleGroupTxnsList()
                        {

                            RId = c.RId,
                            RName = c.RName,
                            Companyid = c.Companyid,
                            IsActive = true

                        }
                 ).ToList();

                if (lstoffstaff == null)
                {
                    return new List<RuleGroupTxnsList>();
                }
                else
                {
                    return lstoffstaff;
                }
            }
            catch (Exception)
            {
                return new List<RuleGroupTxnsList>();
                throw;
            }
        }


        public List<MapRuletoRuleGroupList> GetAllUserRulegroup()
        {

            try
            {


                var lstoffstaff = context.RuleGroup.Select(c => new MapRuletoRuleGroupList()
                {

                    RuleGroupId = c.id,
                    RuleGroupName = c.name
                }
                ).ToList();

                if (lstoffstaff == null)
                {
                    return new List<MapRuletoRuleGroupList>();
                }
                else
                {
                    return lstoffstaff;
                }
            }
            catch (Exception)
            {
                return new List<MapRuletoRuleGroupList>();
                throw;
            }
        }

        public List<MapRuletoRuleGroupList> GetAllRuleGroupTxns(string id, string Staffid)
        {
           
            var qryStr = new StringBuilder();
            qryStr.Clear();

            qryStr.Append("SELECT ISNULL(RGT.id, 0) as Id, A.id as Ruleid, A.name as RuleName, B.id as RuleGroupId, B.name as RuleGroupName,ISNULL(RGT.isactive, 0) as isactive," +
                 "ISNULL(RGT.value, '-') as RuleGroupTxnsvalue,ISNULL(RGT.defaultvalue, '-') as RuleGroupTxnsdefaultvalue,C.id as companyid,C.Name as CompanyName " +
                 "FROM [RULE] A CROSS JOIN RULEGROUP B cross join Location C left join RuleGroupTxn RGT on A.id=RGT.ruleid and B.ID=rgt.rulegroupid and C.Id=RGT.LocationId " +
                 "where A.id=@Id order by  C.id  ");
            try
            {
                var lstoffstaff = context.Database.SqlQuery<MapRuletoRuleGroupList>(qryStr.ToString(), new SqlParameter("@Id", id))
                        .Select(c => new MapRuletoRuleGroupList()
                        {

                            Id = c.Id,
                            RuleGroupId = c.RuleGroupId,
                            companyid = c.companyid,
                            companyName = c.companyName,
                            RuleGroupName = c.RuleGroupName,
                            Ruleid = c.Ruleid,
                            RuleName = c.RuleName,
                            RuleGroupTxnsvalue = c.RuleGroupTxnsvalue,
                            RuleGroupTxnsdefaultvalue = c.RuleGroupTxnsdefaultvalue,
                            isactive = c.isactive

                        }
                 ).ToList();

                if (lstoffstaff == null)
                {
                    return new List<MapRuletoRuleGroupList>();
                }
                else
                {
                    return lstoffstaff;
                }
            }
            catch (Exception)
            {
                return new List<MapRuletoRuleGroupList>();
                throw;
            }
        }

        public List<RuleGroup1> GetAllRuleGroupTxsValues(Int32 RuleId)
        {
            SqlParameter[] sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@RuleId", RuleId);

            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select convert(varchar,id) as Id,RuleId,rulegroupid,Value from RuleGroupTxn where ruleid=@RuleId and IsActive='1'");
            try
            {
                var data = context.Database.SqlQuery<RuleGroup1>(QryStr.ToString(), sqlParameter).Select(d => new RuleGroup1()
                {
                    Id = d.Id,
                    RuleId = d.RuleId,
                    RuleGroupId = d.RuleGroupId,
                    Value = d.Value
                }).ToList();

                if (data.Count == 0)
                {
                    return new List<RuleGroup1>();
                }
                else
                {
                    //if (string.IsNullOrEmpty(data.) == true)
                    //{
                    //    throw new Exception("Employee does not exists.");
                    //}
                    return data;
                }
            }
            catch (Exception e)
            {
                throw e;
                //return new IndividualLeaveCreditDebit_EmpDetails();
            }
        }


        public void SaveRuleGroupTxnsInfo(RuleGroupTxn RuleInfo)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {

                    context.RuleGroupTxn.AddOrUpdate(RuleInfo);
                    context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

    }
}
