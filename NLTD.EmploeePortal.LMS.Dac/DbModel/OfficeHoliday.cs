using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmploeePortal.LMS.Dac.DbModel
{
    public class OfficeHoliday
    {
        public Int64 OfficeHolodayId { get; set; }
        public Int64 OfficeId { get; set; }
        public String Title { get; set; }
        public DateTime Holiday { get; set; }

        public Int32 Year { get; set; }
    }
}
