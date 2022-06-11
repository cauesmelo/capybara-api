using capybara_api.Attributes;
using capybara_api.Infra;
using capybara_api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseMySql(builder.Configuration["Database:ConnectionString"],
            ServerVersion.AutoDetect(builder.Configuration["Database:ConnectionString"])));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<NoteService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(opt => {
    opt.Filters.Add<HttpResponseExceptionFilter>();
}).AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddMvc();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
