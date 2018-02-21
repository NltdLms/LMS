using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Common.DisplayModel
{
    public class EmployeeAttendanceModel
    {
        public string Name { get; set; }
        public Int64? UserID { get; set; }

        public Int64 CardID { get; set; }
        public DateTime InOutDate { get; set; }
        

        public string AttendenceDate { get; set; }
        public string INOutTime { get; set; }
        public string InOut { get; set; }

        public string ErrorMessage { get; set; }

       

        public string requestLevelPerson { get; set; }
    }
}
