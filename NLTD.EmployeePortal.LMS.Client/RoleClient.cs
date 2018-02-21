using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Dac.DbHelper;

namespace NLTD.EmployeePortal.LMS.Client
{
    public class RoleClient : IRoleHelper
    {
        public void Dispose()
        {
            //Nothing to implement...
        }

        public List<DropDownItem> GetAllRoles()
        {
            using (IRoleHelper helper = new RoleHelper())
            {
                return helper.GetAllRoles();
            }
        }
    }
}
