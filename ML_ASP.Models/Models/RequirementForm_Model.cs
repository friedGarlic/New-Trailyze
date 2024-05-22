using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.Models.Models
{
    public class RequirementForm_Model
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public bool IsSubmitted { get; set; }

        public string FileName { get; set; }

        public int FormNumber { get; set; }

        public string? FileId { get; set; }
    }
}
