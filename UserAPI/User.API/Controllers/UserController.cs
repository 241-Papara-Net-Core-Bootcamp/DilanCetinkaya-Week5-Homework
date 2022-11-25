using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using User.Infrastructure.Dtos;
using User.Infrastructure.Interfaces;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Add islemi background worker serviste yapılıyor.
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateAsync(UserDto userDto, int id)
        {
            await _userService.UpdateAsync(userDto, id);
            return Ok();
        }
    }
}
