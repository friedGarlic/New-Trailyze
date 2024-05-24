using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.Models.Models.UserDashboard
{
    public class Overtime_Model
    {
        [Key]
        public int id { get; set; }

        public string UserId { get; set; }

        public string? Description { get; set; }

        public TimeSpan OvertimeEndTime { get; set; }

        public string ApprovalStatus { get; set; } = "Pending";
    }
}
