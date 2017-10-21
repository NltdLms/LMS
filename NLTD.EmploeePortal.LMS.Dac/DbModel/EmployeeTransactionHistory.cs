using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLTD.EmploeePortal.LMS.Dac.DbModel
{
    public class EmployeeTransactionHistory
    {
        public Int64 TransactionId { get; set; }
        public Int64 UserId { get; set; }
        public DateTime TransactionDate { get; set; }
        public String TransactionType { get; set; }
        public Int64 TransactionBy { get; set; }
        public string Remarks { get; set; }
    }
}
