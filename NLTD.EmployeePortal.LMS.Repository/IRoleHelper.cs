﻿using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using System;
using System.Collections.Generic;

namespace NLTD.EmployeePortal.LMS.Repository
{
    public interface IRoleHelper : IDisposable
    {
        List<DropDownItem> GetAllRoles();
    }
}
