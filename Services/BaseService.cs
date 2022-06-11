using capybara_api.Infra;
using Microsoft.AspNetCore.Identity;

namespace capybara_api.Services;

public class BaseService {
    public readonly IHttpContextAccessor httpContextAccessor;
    public readonly IConfiguration configuration;
    public readonly AppDbContext context;
    public readonly UserManager<IdentityUser> userManager;

    public BaseService(IHttpContextAccessor httpContextAccessor,
                       IConfiguration configuration,
                       AppDbContext context,
                       UserManager<IdentityUser> userManager) {
        this.httpContextAccessor = httpContextAccessor;
        this.configuration = configuration;
        this.context = context;
        this.userManager = userManager;
    }

    public BaseService(BaseService baseService) {
        context = baseService.context;
        configuration = baseService.configuration;
        httpContextAccessor = baseService.httpContextAccessor;
        userManager = baseService.userManager;
    }
}
