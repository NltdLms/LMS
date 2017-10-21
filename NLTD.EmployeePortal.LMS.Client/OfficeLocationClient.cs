using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmploeePortal.LMS.Dac.DbHelper;

namespace NLTD.EmployeePortal.LMS.Client
{
    public class OfficeLocationClient : IOfficeLocationHelper
    {
        public void Dispose()
        {
            //Nothing to implement...
        }

        public List<DropDownItem> GetAllOfficeLocations()
        {
            IOfficeLocationHelper helper = new OfficeLocationHelper();
            return helper.GetAllOfficeLocations();
        }
    }
}
