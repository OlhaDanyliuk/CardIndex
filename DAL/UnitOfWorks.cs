using DAL.Entities;
using DAL.Interface;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWorks : IUnitOfWork
    {
        private readonly CardDbContext _context;
        private CardRepository cardRepository;
        private CategoryRepository categoryRepository;
        private CardScoreRepository cardScoreRepository;

        private UserRepository userRepository;
        public UnitOfWorks(CardDbContext context)
        {
            _context = context;
        }

        public ICardRepository CardRepository => cardRepository = cardRepository ?? new CardRepository(_context);
        public ICategoryRepository CategoryRepository => categoryRepository = categoryRepository ?? new CategoryRepository(_context);
        public ICardScoreRepository CardScoreRepository =>cardScoreRepository = cardScoreRepository ?? new CardScoreRepository(_context);
        public IUserRepository UserRepository => userRepository = userRepository ?? new UserRepository(_context);

        public UserManager<User> UserManager { get; }

        public RoleManager<Role> RoleManager { get; }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
