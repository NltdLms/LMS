using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Common.QueryModel
{
    public class YearwiseLeaveSummaryQueryModel
    {
        public string Name { get; set; }

        public string ErrorMsg { get; set; }

        public int Year { get; set; }

        public bool OnlyReportedToMe { get; set; }
    }
}
