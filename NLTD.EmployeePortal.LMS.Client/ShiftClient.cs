using NLTD.EmployeePortal.LMS.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using NLTD.EmployeePortal.LMS.Common.DisplayModel;
using NLTD.EmploeePortal.LMS.Dac.DbHelper;
using NLTD.EmployeePortal.LMS.Common.QueryModel;

namespace NLTD.EmployeePortal.LMS.Client
{
    public class ShiftClient : IShiftHelper
    {
        public List<ShiftAllocation> GetShiftAllocation(Int64 userId, string RequestMenuUser)
        {
            using (IShiftHelper helper = new ShiftHelper())
            {
                return helper.GetShiftAllocation(userId, RequestMenuUser);
            }
        }
        public AddShiftEmployee GetShiftDetailsForUsers(Int64 userId, Int64 ShiftMappingId, string RequestMenuUser)
        {
            using (IShiftHelper helper = new ShiftHelper())
            {
                return helper.GetShiftDetailsForUsers(userId, ShiftMappingId, RequestMenuUser);
            }
        }
        public string SaveEmployeeShift(List<Int64> UserId, int Shift, DateTime FromDate, DateTime ToDate, Int64 MgrId, Int64? ShiftMappingID)
        {
            using (IShiftHelper helper = new ShiftHelper())
            {
                return helper.SaveEmployeeShift(UserId, Shift, FromDate, ToDate, MgrId, ShiftMappingID);
            }
        }
        public List<Shifts> GetShiftMaster()
        {
            using (IShiftHelper helper = new ShiftHelper())
            {
                return helper.GetShiftMaster();
            }
        }


       
        public Shifts GetShiftMasterWithId(Int64 shiftId)
        {
            using (IShiftHelper helper = new ShiftHelper())
            {
                return helper.GetShiftMasterWithId(shiftId);
            }
        }

       
        public string SaveShiftMaster(int shiftId, string shiftName, TimeSpan fromTime, TimeSpan toTime, Int64 MgrId)
        {
            using (IShiftHelper helper = new ShiftHelper())
            {
                return helper.SaveShiftMaster(shiftId, shiftName, fromTime, toTime, MgrId);
            }
        }

        public void Dispose()
        {
            //Nothing to impement...
        }
    }
}
