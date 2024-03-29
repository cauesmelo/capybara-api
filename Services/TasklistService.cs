﻿using capybara_api.Infra;
using capybara_api.Models;
using capybara_api.Models.DTO;
using capybara_api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace capybara_api.Services;

public class TaskListService : BaseService {
    private readonly string cacheKey = "tasklist@";

    public TaskListService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        AppDbContext context,
        UserManager<IdentityUser> userManager,
        IDistributedCache cache)
        : base(httpContextAccessor, configuration, context, userManager, cache) { }

    public List<TaskList> Get() {
        string userId = GetUserId();
        List<TaskList> taskLists;
        byte[] taskListCache = cache.Get(GetCacheKey());

        if(taskListCache is null) {
            taskLists = context.taskList
                .Where(n => n.userId == userId)
                .Include(n => n.tasks)
                .AsNoTracking()
                .OrderByDescending(d => d.updatedAt)
                .ToList();

            byte[] serializedTaskLists = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(taskLists));

            cache.Set(GetCacheKey(), serializedTaskLists, new DistributedCacheEntryOptions());
        }
        else {
            string serialized = Encoding.UTF8.GetString(taskListCache);
            taskLists = JsonConvert.DeserializeObject<List<TaskList>>(serialized);
        }

        return taskLists;
    }

    public TaskList Create(TaskListCreate taskListCreate) {
        string userId = GetUserId();

        TaskList taskList = new() {
            title = taskListCreate.title,
            userId = userId,
            tasks = taskListCreate.tasks.Select(t => {
                TaskUnity task = new() { title = t, isComplete = false };
                return task;
            }).ToList(),
        };

        context.taskList.Add(taskList);
        context.SaveChanges();

        List<TaskList> tasklists = new();
        byte[] tasklistsCache = cache.Get(GetCacheKey());

        if(tasklistsCache is null) {
            tasklists.Add(taskList);
        }
        else {
            string serialized = Encoding.UTF8.GetString(tasklistsCache);
            tasklists = JsonConvert.DeserializeObject<List<TaskList>>(serialized);
            tasklists.Add(taskList);
        }

        byte[] serializedNotes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(tasklists));
        cache.Set(GetCacheKey(), serializedNotes, new DistributedCacheEntryOptions());

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

        List<TaskList> tasklists = new();
        byte[] tasklistsCache = cache.Get(GetCacheKey());

        if(tasklistsCache is not null) {
            string serialized = Encoding.UTF8.GetString(tasklistsCache);
            tasklists = JsonConvert.DeserializeObject<List<TaskList>>(serialized);
            tasklists = tasklists.Where(n => n.id != taskListUpdate.id).ToList();
        }
        tasklists.Add(taskList);

        byte[] serializedTasklist = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(tasklists));
        cache.Set(GetCacheKey(), serializedTasklist, new DistributedCacheEntryOptions());

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

        List<TaskList> tasklists = new();
        byte[] tasklistsCache = cache.Get(GetCacheKey());

        if(tasklistsCache is not null) {
            string serialized = Encoding.UTF8.GetString(tasklistsCache);
            tasklists = JsonConvert.DeserializeObject<List<TaskList>>(serialized);
            tasklists = tasklists.Where(n => n.id != id).ToList();
            byte[] serializedTasklist = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(tasklists));
            cache.Set(GetCacheKey(), serializedTasklist, new DistributedCacheEntryOptions());
        }
    }

    private string GetUserId() {
        return httpContextAccessor.HttpContext
                .User.Claims
                .First(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
        //return "83f6f51f-cd5c-4c6b-a5f4-6ff0fc794d49";
    }

    private string GetCacheKey() {
        return cacheKey + GetUserId();
    }
}
