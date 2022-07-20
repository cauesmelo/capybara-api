using capybara_api.Infra;
using capybara_api.Models.DTO;
using capybara_api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;

namespace capybara_api.Services;

public class ProfileService : BaseService {
    public ProfileService(
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration,
    AppDbContext context,
    UserManager<IdentityUser> userManager, 
    IDistributedCache cache)
    : base(httpContextAccessor, configuration, context, userManager, cache) { }

    public bool UpdateEmail(string email) {
        return true;
    }

    public bool UpdateTheme(string theme) {
        return true;
    }
}
