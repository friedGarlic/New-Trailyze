using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.Models.Models
{
    public class Notification_Model
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string NotifUserId { get; set; }

        //weekly report that needed to be submitted
        //reminders that expired,
        //accepted submission and timein evidence approval
        //admin warnings
    }
}
