using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories
{
    public class NotificationRepository : Repository<Notification_Model>, INotificationRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public NotificationRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
