using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.Models.Models
{
    public class Reminder_Model
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string IconClass { get; set; }
        public string IconType { get; set; }
        public DateTime? ReminderDateTime { get; set; }

        public double? ReminderDuration { get; set; }
    }
}
