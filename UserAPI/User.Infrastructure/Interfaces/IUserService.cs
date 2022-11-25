using System.Collections.Generic;
using System.Threading.Tasks;
using User.Infrastructure.Dtos;

namespace User.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task AddAsync(UserDto userDto);
        Task DeleteAsync(int id);
        Task UpdateAsync(UserDto userDto, int id);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(int id);
    }
}
