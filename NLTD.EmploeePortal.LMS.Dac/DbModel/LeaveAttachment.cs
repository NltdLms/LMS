using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmploeePortal.LMS.Dac.DbModel
{
    public class LeaveAttachment
    {
        public Int64 AttachmentId { get; set; }
        public Int64 LeaveId { get; set; }
        public String AttachmentDocumentTitle { get; set; }
        public String AttachmentPath { get; set; }
    }
}
