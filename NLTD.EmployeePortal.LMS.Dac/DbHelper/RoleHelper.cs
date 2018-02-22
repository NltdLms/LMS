using NLTD.EmployeePortal.LMS.Dac.Dac;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Dac.DbHelper
{
    public class RoleHelper : IRoleHelper, IDisposable
    {
        public void Dispose()
        {
            //Nothing to implement...
        }

        public List<DropDownItem> GetAllRoles()
        {
            using (var dac = new RoleDac())
            {
                return dac.GetAllRoles();
            }
        }
    }
}
