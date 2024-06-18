using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.Models.Models.UserDashboard
{
    public class ListModel2
    {
        [Key]
        public int id { get; set; }

        public string ListName { get; set; }

		public string TimeIn { get; set; }
		public string TimeOut { get; set; }

		public string WorkingDaysStart { get; set; }
		public string WorkingDaysEnds { get; set; }
	}
}