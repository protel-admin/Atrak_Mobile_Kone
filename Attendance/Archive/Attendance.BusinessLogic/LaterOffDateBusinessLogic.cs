using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using Attendance.BusinessLogic;
using Attendance.Repository;

namespace Attendance.BusinessLogic
{
    public class LaterOffDateBusinessLogic
    {
        public void DeleteLaterOffDate(string Id)
        {
            using (LaterOffDateRepository laterOffDateRepository = new LaterOffDateRepository())
            {
                laterOffDateRepository.DeleteLaterOffDate(Id);
            }
        }

        public void SaveLaterOffDate(LaterOffDate data)
        {
            using (LaterOffDateRepository laterOffDateRepository = new LaterOffDateRepository())
            {
                laterOffDateRepository.SaveLaterOffDate(data);
            }
        }

        public int LaterOffAlreadyApplied(string LaterOffReqDate)
        {
            using (LaterOffDateRepository laterOffDateRepository = new LaterOffDateRepository())
            {
                return laterOffDateRepository.LaterOffAlreadyApplied(LaterOffReqDate);
            }
        }

        public int LaterOffAlreadyApplied(int Id)
        {
            using (LaterOffDateRepository laterOffDateRepository = new LaterOffDateRepository())
            {
                return laterOffDateRepository.LaterOffAlreadyApplied(Id);
            }
        }

        public List<LaterOffDateList> GetLaterOffDateList()
        {
            using (LaterOffDateRepository laterOffDateRepository = new LaterOffDateRepository())
            {
                return laterOffDateRepository.GetLaterOffDateList();
            }
        }

        public List<CompanyList> GetCompanyList()
        {
            using (LaterOffDateRepository laterOffDateRepository = new LaterOffDateRepository())
            {
                return laterOffDateRepository.GetCompanyList();
            }
        }

        public LaterOffDate GetData(string Id)
        {
            using (LaterOffDateRepository laterOffDateRepository = new LaterOffDateRepository())
            {
                return laterOffDateRepository.GetData(Id);
            }
        }

    }
}
