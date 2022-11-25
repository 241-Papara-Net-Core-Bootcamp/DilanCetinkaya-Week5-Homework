using Microsoft.EntityFrameworkCore;
using User.Core.Interfaces;
using User.Core.Models;
using User.Infrastructure.Context;

namespace User.Infrastructure.Repositories
{
    public class UserRepository : Repository<UserEntity>, IUserRepository
    {
        private readonly DbSet<UserEntity> _users;
        public UserRepository(UserDbContext context) : base(context) 
        {
            _users = context.Set<UserEntity>();
        }
    }
}
