using DAL.Entities;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CardDbContext _dbContext;
        public UserRepository(CardDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task AddAsync(User entity)
        {
            _dbContext.Users.AddAsync(entity);
            return _dbContext.SaveChangesAsync();
        }

        public void Delete(User entity)
        {
            _dbContext.Users.Remove(entity);
            _dbContext.SaveChanges();
        }

        public Task DeleteByIdAsync(long id)
        {
            _dbContext.Users.Remove(_dbContext.Users.Find(id));
            return _dbContext.SaveChangesAsync();
        }

        public IQueryable<User> GetAll()
        {
            return _dbContext.Users.AsQueryable();
        }
        public Task<User> GetByIdAsync(long id)
        {
            return _dbContext.Users.FindAsync(id).AsTask();
        }


        public void Update(User entity)
        {
            _dbContext.Users.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
