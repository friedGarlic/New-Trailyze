using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.Models.Models.UserDashboard
{
    public class ListModel
    {
        [Key]
        public int id { get; set; }

        public string ListName { get; set; }
    }
}
