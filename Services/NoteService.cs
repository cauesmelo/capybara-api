using capybara_api.Infra;
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

public class NoteService : BaseService {
    private readonly string cacheKey = "notes@";

    public NoteService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        AppDbContext context,
        UserManager<IdentityUser> userManager,
        IDistributedCache cache)
        : base(httpContextAccessor, configuration, context, userManager, cache) { }

    public List<Note> Get() {
        string userId = GetUserId();
        List<Note> notes;
        byte[] notesCache = cache.Get(GetCacheKey());

        if(notesCache is null) {
            notes = context.note
                            .Where(n => n.userId == userId)
                            .AsNoTracking()
                            .OrderByDescending(d => d.updatedAt)
                            .ToList();

            byte[] serializedNotes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(notes));

            cache.Set(GetCacheKey(), serializedNotes, new DistributedCacheEntryOptions());
        }
        else {
            string serialized = Encoding.UTF8.GetString(notesCache);
            notes = JsonConvert.DeserializeObject<List<Note>>(serialized);
        }

        return notes;
    }

    public Note Create(NoteCreate noteCreate) {
        string userId = GetUserId();

        Note note = new() { content = noteCreate.content, userId = userId };
        context.note.Add(note);
        context.SaveChanges();

        List<Note> notes = new();
        byte[] notesCache = cache.Get(GetCacheKey());

        if(notesCache is null) {
            notes.Add(note);
        }
        else {
            string serialized = Encoding.UTF8.GetString(notesCache);
            notes = JsonConvert.DeserializeObject<List<Note>>(serialized);
            notes.Add(note);
        }

        byte[] serializedNotes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(notes));
        cache.Set(GetCacheKey(), serializedNotes, new DistributedCacheEntryOptions());

        return note;
    }

    public Note Update(int id, NoteUpdate noteUpdate) {
        string userId = GetUserId();

        Note note = context.note.FirstOrDefault(n => n.id == id);

        if(note == null)
            throw new HttpResponseException(404, "NOT_FOUND");

        if(note.userId != userId)
            throw new HttpResponseException(401, "NOT_OWNED");

        note.content = noteUpdate.content;

        context.note.Update(note);
        context.SaveChanges();

        List<Note> notes = new();
        byte[] notesCache = cache.Get(GetCacheKey());

        if(notesCache is not null) {
            string serialized = Encoding.UTF8.GetString(notesCache);
            notes = JsonConvert.DeserializeObject<List<Note>>(serialized);
            notes = notes.Where(n => n.id != id).ToList();
        }
        notes.Add(note);
        byte[] serializedNotes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(notes));
        cache.Set(GetCacheKey(), serializedNotes, new DistributedCacheEntryOptions());

        return note;
    }

    public void Delete(int id) {
        string userId = GetUserId();

        Note note = context.note.FirstOrDefault(n => n.id == id);

        if(note == null)
            throw new HttpResponseException(404, "NOT_FOUND");

        if(note.userId != userId)
            throw new HttpResponseException(401, "NOT_OWNED");

        context.note.Remove(note);
        context.SaveChanges();

        List<Note> notes = new();
        byte[] notesCache = cache.Get(GetCacheKey());

        if(notesCache is not null) {
            string serialized = Encoding.UTF8.GetString(notesCache);
            notes = JsonConvert.DeserializeObject<List<Note>>(serialized);
            notes = notes.Where(n => n.id != id).ToList();
        }
        byte[] serializedNotes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(notes));
        cache.Set(GetCacheKey(), serializedNotes, new DistributedCacheEntryOptions());
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
