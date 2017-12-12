using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using NLTD.EmploeePortal.LMS.Dac.DbModel;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmployeePortal.LMS.Common.QueryModel;

namespace NLTD.EmploeePortal.LMS.Dac.DbHelper
{
    public class ShiftHelper : IShiftHelper, IDisposable
    {
        public void Dispose()
        {
            //Nothing to dispose...
        }

        public List<ShiftAllocation> GetShiftAllocation(Int64 UserId, string RequestMenuUser)
        {
            using (var dac = new ShiftDac())
            {
                return dac.GetShiftAllocation(UserId, RequestMenuUser);
            }
        }


        public AddShiftEmployee GetShiftDetailsForUsers(Int64 userId, Int64 EditUserId, string RequestMenuUser)
        {
            using (var dac = new ShiftDac())
            {
                return dac.GetShiftDetailsForUsers(userId, EditUserId, RequestMenuUser);
            }
        }

        public string SaveEmployeeShift(List<Int64> UserId, int Shift, DateTime FromDate, DateTime ToDate, Int64 MgrId, Int64? ShiftMappingID)
        {
            using (var dac = new ShiftDac())
            {
                return dac.SaveEmployeeShift(UserId, Shift, FromDate, ToDate, MgrId, ShiftMappingID);
            }
        }

        public string SaveShiftMaster(int shiftId, string shiftName, TimeSpan fromTime, TimeSpan ToTime, Int64 MgrId)
        {
            using (var dac = new ShiftDac())
            {
                return dac.SaveShiftMaster(shiftId, shiftName, fromTime, ToTime, MgrId);
            }
        }
        public List<Shifts> GetShiftMaster()
        {
            using (var dac = new ShiftDac())
            {
                return dac.GetShiftMaster();
            }
        }
        public Shifts GetShiftMasterWithId(Int64 shiftId)
        {
            using (var dac = new ShiftDac())
            {
                return dac.GetShiftMasterWithId(shiftId);
            }
        }
    }
}
