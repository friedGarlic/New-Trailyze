using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.Models.Models
{
	public class Workload_Model
	{
		[Key]
		public int Id { get; set; }

		public int ModelId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime DueDate { get; set; }

		public string Course { get; set; }
	}
}
