using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmploeePortal.LMS.Dac.DbModel
{
    public class EmployeeRole
    {
        public Int64 RoleId { get; set; }
        public String Role { get; set; }        
        public Boolean IsAdmin { get; set; }
        public Boolean IsHR { get; set; }
        public Boolean IsActive { get; set; }
    }
}
