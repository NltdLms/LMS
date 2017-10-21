using System.Collections.Generic;

namespace NLTD.EmployeePortal.LMS.Common.DisplayModel
{
    public class DashBoardModel
    {
        public IList<LeaveSummary> lstLeaveSummary { get; set; }

        public IList<HolidayModel> lstHolidayModel { get; set; }

        public IList<DropDownItem> lstWeekOffs { get; set; }

        public int PendingApprovalCount { get; set; }

        public bool IsLMSApprover { get; set; }


        
    }
}
