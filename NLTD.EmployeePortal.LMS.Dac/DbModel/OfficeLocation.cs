using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Dac.DbModel
{
    public class OfficeLocation
    {
        public Int64 OfficeId { get; set; }
        public String OfficeName { get; set; }
        public Boolean IsActive { get; set; }
    }
}
