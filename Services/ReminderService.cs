using capybara_api.Infra;
using capybara_api.Models;
using capybara_api.Models.DTO;
using capybara_api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;

namespace capybara_api.Services;

public class ReminderService : BaseService {
    public ReminderService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        AppDbContext context,
        UserManager<IdentityUser> userManager,
        IDistributedCache cache)
        : base(httpContextAccessor, configuration, context, userManager, cache) { }

    public List<Reminder> Get() {
        string userId = GetUserId();

        List<Reminder> reminders = context.reminder
                            .Where(n => n.userId == userId)
                            .AsNoTracking()
                            .OrderByDescending(d => d.updatedAt)
                            .ToList();

        return reminders;
    }

    public Reminder Create(ReminderCreate reminderCreate) {
        string userId = GetUserId();

        Reminder reminder = new() { title = reminderCreate.title, userId = userId, reminderDate = reminderCreate.reminderDate };
        context.reminder.Add(reminder);
        context.SaveChanges();
        return reminder;
    }

    public void Delete(int id) {
        string userId = GetUserId();

        Reminder reminder = context.reminder.FirstOrDefault(n => n.id == id);

        if(reminder == null)
            throw new HttpResponseException(404, "NOT_FOUND");

        if(reminder.userId != userId)
            throw new HttpResponseException(401, "NOT_OWNED");

        context.reminder.Remove(reminder);
        context.SaveChanges();
    }

    private string GetUserId() {
        return httpContextAccessor.HttpContext
                        .User.Claims
                        .First(c => c.Type == ClaimTypes.NameIdentifier)
                        .Value;
    }
}
