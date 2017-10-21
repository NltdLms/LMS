using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Common.QueryModel
{
    public class EmployeeProfileSearchModel
    {
        public string Name { get; set; }

        public string RequestLevelPerson { get; set; }

        public bool OnlyReportedToMe { get; set; }

        public bool HideInactiveEmp { get; set; }
    }
}
