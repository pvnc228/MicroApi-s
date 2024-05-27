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

        [HttpPost("passwords/store")]
        public async Task<IActionResult> StorePasswordAsync(PasswordModel model)
        {
            var result = await _passwordManagerService.StorePasswordAsync(model);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpGet("passwords/retrieve")]
        public async Task<IActionResult> RetrievePasswordsAsync(string username)
        {
            var passwords = await _passwordManagerService.RetrievePasswordsAsync(username);
            if (passwords == null || !passwords.Any())
            {
                return NotFound();
            }
            return Ok(passwords);
        }

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
