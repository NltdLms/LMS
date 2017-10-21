using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmploeePortal.LMS.Dac.DbModel
{
    public class PermissionDetail
    {
        public Int64 PermissionDetailId { get; set; }

        public Int64 LeaveId { get; set; }

        public DateTime PermissionDate { get; set; }

        public string TimeFrom { get; set; }

        public string TimeTo { get; set; }

        public string Remarks { get; set; }

        




    }
}
