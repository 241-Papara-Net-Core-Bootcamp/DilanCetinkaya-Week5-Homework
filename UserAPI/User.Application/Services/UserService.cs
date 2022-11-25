using AutoMapper;
using Hangfire;
using System.Collections.Generic;
using System.Threading.Tasks;
using User.Core.Interfaces;
using User.Core.Models;
using User.Infrastructure.Dtos;
using User.Infrastructure.Interfaces;

namespace User.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserEntity> _repository;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        const string userCacheKey = "allusers";

        public UserService(IRepository<UserEntity> repository, IMapper mapper, ICacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task AddAsync(UserDto userDto)
        {
            var user = _mapper.Map<UserEntity>(userDto);
            await _repository.AddAsync(user);
            BackgroundJob.Enqueue(() => RefreshCache());

        }

        public async Task RefreshCache()
        {
            _cacheService.Remove(userCacheKey);
            var cachedList = await _repository.GetAllAsync();
            _cacheService.Set(userCacheKey, cachedList);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(user);
            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task UpdateAsync(UserDto userDto, int id)
        {
            var updatedUser = _mapper.Map<UserEntity>(userDto);
            updatedUser.Id = id;
            await _repository.UpdateAsync(updatedUser);
            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var isExist = _cacheService.TryGet(userCacheKey, out List<UserDto> usersDto);
            if (!isExist)
            {
                var users = await _repository.GetAllAsync();
                usersDto = _mapper.Map<List<UserDto>>(users);
                _cacheService.Set(userCacheKey, usersDto);
            }

            return usersDto;
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }
    }
}