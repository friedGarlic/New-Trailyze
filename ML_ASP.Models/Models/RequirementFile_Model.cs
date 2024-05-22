using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.Models.Models
{
    public class RequirementFile_Model
    {
        [Key]
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string UserId { get; set; }

        public string FileId { get; set; }

        public string FileName { get; set; }

        public string? Title {  get; set; }

        public string? Description { get; set; }

		public string? ApprovalStatus { get; set; }
	}
}