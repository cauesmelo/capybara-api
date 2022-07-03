using capybara_api.Infra;
using capybara_api.Models;
using capybara_api.Models.DTO;
using capybara_api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace capybara_api.Services;

public class TaskListService : BaseService {
    public TaskListService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        AppDbContext context,
        UserManager<IdentityUser> userManager)
        : base(httpContextAccessor, configuration, context, userManager) { }

    public List<TaskList> Get() {
        string userId = GetUserId();

        List<TaskList> taskLists = context.taskList
            .Where(n => n.userId == userId)
            .AsNoTracking()
            .OrderByDescending(d => d.updatedAt)
            .ToList();

        return taskLists;
    }

    public TaskList Create(TaskListCreate taskListCreate) {
        string userId = GetUserId();

        TaskList taskList = new() {
            title = taskListCreate.title,
            userId = userId,
            tasks = (List<TaskUnity>) taskListCreate.tasks.Select(t => {
                TaskUnity task = new() { title = t.title, isComplete = false };
                return task;
            }),
        };

        context.taskList.Add(taskList);
        context.SaveChanges();
        return taskList;
    }

    public TaskList Update(TaskList taskListUpdate) {
        string userId = GetUserId();

        TaskList taskList = context.taskList.Find(taskListUpdate.id);

        if(taskList == null)
            throw new HttpResponseException(404, "NOT_FOUND");

        if(taskList.userId != userId)
            throw new HttpResponseException(401, "NOT_OWNED");

        taskList = taskListUpdate;

        context.taskList.Update(taskList);
        context.SaveChanges();
        return taskList;
    }

    public void Delete(int id) {
        string userId = GetUserId();

        TaskList taskList = context.taskList.FirstOrDefault(n => n.id == id);

        if(taskList == null)
            throw new HttpResponseException(404, "NOT_FOUND");

        if(taskList.userId != userId)
            throw new HttpResponseException(401, "NOT_OWNED");

        context.taskList.Remove(taskList);
        context.SaveChanges();
    }

    private string GetUserId() {
        return httpContextAccessor.HttpContext
                        .User.Claims
                        .First(c => c.Type == ClaimTypes.NameIdentifier)
                        .Value;
    }
}
