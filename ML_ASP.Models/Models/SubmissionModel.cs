using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace ML_ASP.Models
{
    public class SubmissionModel
    {
        [Key]
        public int Id { get; set; }

        public string SubmissionUserId { get; set; }

        public string? FileName { get; set; }

        public string? Name { get; set; }

        public DateTime Date { get; set; }

		public string? ApprovalStatus { get; set; }

        public string? Grade { get; set; }

        public string? FolderId { get; set; }

        public bool IsMultipleFile { get; set; } = false;

		//deadline status

		public string? DueStatus { get; set; } //if late or early(depends on days)

		public string? Details { get; set; } //flag if submitted to a model of deadline workload
	}
}
