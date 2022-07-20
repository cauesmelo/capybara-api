using capybara_api.Infra;
using capybara_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace capybara_api.Services;

public class TaskUnityService : BaseService {
    private readonly string cacheKey = "tasklist@";

    public TaskUnityService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        AppDbContext context,
        UserManager<IdentityUser> userManager,
        IDistributedCache cache)
        : base(httpContextAccessor, configuration, context, userManager, cache) { }

    public TaskUnity Update(TaskUnity task) {
        context.taskUnity.Update(task);
        context.SaveChanges();
        cache.Remove(GetCacheKey());
        return task;
    }

    private string GetUserId() {
        return httpContextAccessor.HttpContext
                        .User.Claims
                        .First(c => c.Type == ClaimTypes.NameIdentifier)
                        .Value;
    }

    private string GetCacheKey() {
        return cacheKey + GetUserId();
    }
}
