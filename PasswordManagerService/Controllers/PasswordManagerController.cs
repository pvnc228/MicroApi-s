using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManagerService.Interface;
using PasswordManagerService.Models;

namespace PasswordManagerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PasswordManagerController : ControllerBase
    {
        private readonly IPasswordManagerService _passwordManagerService;

        public PasswordManagerController(IPasswordManagerService passwordManagerService)
        {
            _passwordManagerService = passwordManagerService;
        }

        [Authorize] 
        [HttpPost("passwords/store")]
        public async Task<IActionResult> StorePasswordAsync(PasswordModel model)
        {
            
            var username = User.Identity?.Name;

            
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            
            model.Username = username;

            var result = await _passwordManagerService.StorePasswordAsync(model);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [Authorize] 
        [HttpGet("passwords/retrieve")]
        public async Task<IActionResult> RetrievePasswordsAsync()
        {
            
            var username = User.Identity?.Name;

            
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            var passwords = await _passwordManagerService.RetrievePasswordsAsync(username);
            if (passwords == null || !passwords.Any())
            {
                return NotFound();
            }
            return Ok(passwords);
        }

        [Authorize] 
        [HttpPost("media/store")]
        public async Task<IActionResult> StoreMediaAsync(MediaModel model)
        {
            var result = await _passwordManagerService.StoreMediaAsync(model);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [Authorize] 
        [HttpGet("media/retrieve")]
        public async Task<IActionResult> RetrieveMediaAsync(string username)
        {
            var media = await _passwordManagerService.RetrieveMediaAsync(username);
            if (media == null || !media.Any())
            {
                return NotFound();
            }
            return Ok(media);
        }
    }
}
