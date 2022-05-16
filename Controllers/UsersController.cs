using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using capybara_api.Infra;
using capybara_api.Models;
using capybara_api.Models.DTO;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace capybara_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDTO userRequest)
        {
            var user = new IdentityUser {
                Email = userRequest.Email,
                UserName = userRequest.Email
            };

            var result = await _userManager.CreateAsync(user, userRequest.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors.First());

            var userClaims = new List<Claim> {
            new Claim("Name", userRequest.Name),
            };

            var claimResult = await _userManager
                .AddClaimsAsync(user, userClaims);

            if (!claimResult.Succeeded)
                return BadRequest(result.Errors.First());
            

                return Ok();
        }
    }
}
