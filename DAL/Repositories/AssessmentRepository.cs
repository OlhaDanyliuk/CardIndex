using DAL.Entities;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly CardDbContext _dbContext;
        public AssessmentRepository(CardDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task AddAsync(Assessment entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Assessment entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Assessment> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Assessment> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Assessment entity)
        {
            throw new NotImplementedException();
        }
    }
}
