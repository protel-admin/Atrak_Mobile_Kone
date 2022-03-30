using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class BenchReportingManagerBusinessLogic
    {
        public BenchReportingManagerModel GetBenchReportingManager(int Id)
        {
            using (BenchReportingManagerRepository benchReportingManagerRepository = new BenchReportingManagerRepository())
            {                
                return benchReportingManagerRepository.GetBenchReportingManager(Id);
            }
        }

        public List<BenchReportingManagerDataTableModel> LoadDataTable()
        {
            using (BenchReportingManagerRepository benchReportingManagerRepository = new BenchReportingManagerRepository())
            {
                return benchReportingManagerRepository.LoadDataTable();
            }
        }

        public void SaveChanges(BenchReportingManagerModel model)
        {
            BenchReportingManager _BRM_ = new BenchReportingManager();
            _BRM_.Id = model.Id;
            _BRM_.StaffId = model.StaffId;
            _BRM_.IsActive = model.IsActive;
            _BRM_.CreatedOn = Convert.ToDateTime(model.CreatedOn);
            _BRM_.CreatedBy = model.CreatedBy;
            using (BenchReportingManagerRepository benchReportingManagerRepository = new BenchReportingManagerRepository())
            {
                    benchReportingManagerRepository.SaveChanges(_BRM_);
            }
        }

        //public List<AttachDetachStaffList> GetStaffListToDetach(string ReportingManager)
        //{
        //    var repo = new BenchReportingManagerRepository();
        //    return repo.GetStaffListToDetach(ReportingManager);
        //}


        public List<AttachDetachList> GetStaffListToDetach(string ReportingManager)
        {
            using (BenchReportingManagerRepository benchReportingManagerRepository = new BenchReportingManagerRepository())
            {
                return benchReportingManagerRepository.GetStaffListToDetach(ReportingManager);
            }
        }

        public void DetachEmployees(AttachDetach model)
        {
            using (BenchReportingManagerRepository benchReportingManagerRepository = new BenchReportingManagerRepository())
            {
                benchReportingManagerRepository.DetachEmployees(model);
            }
        }
        public List<AttachDetachList> GetStaffListFromCommonPool(string ReportingManager)
        {
            using (BenchReportingManagerRepository benchReportingManagerRepository = new BenchReportingManagerRepository())
            {
                return benchReportingManagerRepository.GetStaffListFromCommonPool(ReportingManager);
            }
        }

        public void AttachEmployees(AttachDetach model)
        {
            using (BenchReportingManagerRepository benchReportingManagerRepository = new BenchReportingManagerRepository())
            {
                benchReportingManagerRepository.AttachEmployees(model);
            }
        }

    }
}
