using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class CardDbContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
        public CardDbContext()
        {
        }

        public CardDbContext(DbContextOptions<CardDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CardScore> CardScores { get; set; }

    }
}
