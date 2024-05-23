using Microsoft.EntityFrameworkCore;
using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories
{
    public class RequirementFormRepository : Repository<RequirementForm_Model>, IRequirementFormRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public RequirementFormRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void UpdateFormState(RequirementFile_Model formModel, int formNumber)
        {
            var objFromDb = _dbContext.RequirementForm.FirstOrDefault(u => u.UserId == formModel.UserId && u.FormNumber == formNumber);
            if(objFromDb != null)
            {
                objFromDb.FileName = formModel.FileName;
                objFromDb.FileId = formModel.FileId;
            }

        }
    }
}
