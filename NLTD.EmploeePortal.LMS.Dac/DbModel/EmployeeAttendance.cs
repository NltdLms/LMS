using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmploeePortal.LMS.Dac.DbModel
{
    public class EmployeeAttendance
    {
        public Int64 ID { get; set; }
        public Int64? UserID { get; set; }
        public Int64 CardID { get; set; }
        public DateTime InOutDate { get; set; }
        public bool InOut { get; set; }
    }
}
