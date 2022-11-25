using Microsoft.EntityFrameworkCore;
using User.Core.Models;

namespace User.Infrastructure.Context
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public DbSet<UserEntity> Users { get; set; }
    }
}
