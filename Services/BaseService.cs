using capybara_api.Infra;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace capybara_api.Services;

public class BaseService {
    public readonly IHttpContextAccessor httpContextAccessor;
    public readonly IConfiguration configuration;
    public readonly AppDbContext context;
    public readonly UserManager<IdentityUser> userManager;
    public readonly IDistributedCache cache;

    public BaseService(IHttpContextAccessor httpContextAccessor,
                       IConfiguration configuration,
                       AppDbContext context,
                       UserManager<IdentityUser> userManager,
                       IDistributedCache cache
                       ) {
        this.httpContextAccessor = httpContextAccessor;
        this.configuration = configuration;
        this.context = context;
        this.userManager = userManager;
        this.cache = cache;
    }

    public BaseService(BaseService baseService) {
        context = baseService.context;
        configuration = baseService.configuration;
        httpContextAccessor = baseService.httpContextAccessor;
        userManager = baseService.userManager;
        cache = baseService.cache;
    }
}
