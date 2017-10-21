using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmployeePortal.LMS.Repository
{
    public interface IRoleHelper : IDisposable
    {
        List<DropDownItem> GetAllRoles();
    }
}
