
using capybara_api.Infra;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration["Database:ConnectionString"],
            ServerVersion.AutoDetect(builder.Configuration["Database:ConnectionString"])));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
