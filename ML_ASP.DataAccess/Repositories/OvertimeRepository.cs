using Microsoft.EntityFrameworkCore;
using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models.Models.UserDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories
{
    public class OvertimeRepository : Repository<Overtime_Model>, IOvertimeRepository
    {
        private readonly DbContext _dbContext;

        public OvertimeRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
