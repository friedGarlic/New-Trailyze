using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ML_ASP.Models;
using ML_ASP.Models.Models;
using ML_ASP.Models.Models.UserDashboard;

namespace ML_ASP.DataAccess
{
    public class ApplicationDBContext : IdentityDbContext<Account_Model>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

        public DbSet<Account_Model> Accounts { get; set; }
        public DbSet<Trainee_Model> Trainees { get; set; }
        public DbSet<SubmissionModel> Submissions { get; set; }
        public DbSet<LogModel> Logs { get; set; }
        public DbSet<Reminder_Model> Reminders { get; set; }
        public DbSet<QRModel> QR { get; set; }
        public DbSet<Workload_Model> Workloads { get; set; }
        public DbSet<WorkloadSubmissionList_Model> WorkloadsSubmissionList { get; set;}
        public DbSet<Notification_Model> Notification { get; set; }
        public DbSet<RequirementFile_Model> RequirementFile { get; set; }
        public DbSet<RequirementForm_Model> RequirementForm { get; set; }
        public DbSet<Overtime_Model> Overtime { get; set; }
    }
}
