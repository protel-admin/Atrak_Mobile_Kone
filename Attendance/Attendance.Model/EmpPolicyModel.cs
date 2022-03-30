using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Model
{
   public class EmpPolicyModel
    {
      public int Id { get; set; }
      public string Policyname { get; set; }
      public string FileType { get; set; }
      public byte[] FileExtension { get; set; }

      public string StrFileExtension { get; set; }
    }
}
