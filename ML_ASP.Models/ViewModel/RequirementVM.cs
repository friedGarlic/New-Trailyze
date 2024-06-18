using Microsoft.AspNetCore.Mvc.Rendering;
using ML_ASP.Models.Models;
using ML_ASP.Models.Models.UserDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.Models.ViewModel
{
    public class RequirementVM
    {
        public AccountInfo_Model? AccountInfoModel {  get; set; }

        public string? FileName1 { get; set; }
        public string? FileName2 { get; set; }
        public string? FileName3 { get; set; }
        public string? FileName4 { get; set; }
        public string? FileName5 { get; set; }

		public SelectList OptionList1 { get; set; }

		public SelectList OptionList2 { get; set; }

		public IEnumerable<ListModel> ListModel { get; set; }

		public IEnumerable<ListModel2> ListModel2 { get; set; }
	}
}
