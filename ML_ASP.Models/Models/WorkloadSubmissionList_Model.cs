using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.Models.Models
{
    public class WorkloadSubmissionList_Model
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        public int WorkloadId { get; set; }

        public string SubmissionUserID { get; set; }

        public bool IsSubmitted { get; set; }
    }
}
