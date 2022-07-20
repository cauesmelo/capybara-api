using capybara_api.Infra;
using capybara_api.Models.DTO;
using capybara_api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;

namespace capybara_api.Services;

public class UserService : BaseService {
    public UserService(
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration,
    AppDbContext context,
    UserManager<IdentityUser> userManager, 
    IDistributedCache cache)
    : base(httpContextAccessor, configuration, context, userManager, cache) { }

    public bool Create(UserCreate user) {
        IdentityUser identityUser = new() {
            UserName = user.email,
            Email = user.email,
        };

        IdentityResult result = userManager.CreateAsync(identityUser, user.password).Result;

        if(!result.Succeeded)
            throw new HttpResponseException(404, result.Errors.First().Code);

        List<Claim> userClaims = new() {
            new Claim("name", user.name),
        };

        userManager.AddClaimsAsync(identityUser, userClaims).Wait();

        return true;
    }
}
