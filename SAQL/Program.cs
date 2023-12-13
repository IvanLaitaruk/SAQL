using Microsoft.EntityFrameworkCore;
using SAQL.Contexts;
using SAQL.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<SAQLContext>(
    item => item.UseSqlServer(
        "Server=dbsaql.database.windows.net;" +
        "Database=SaqlDB;" +
        "Trusted_Connection=False;" +
        "Encrypt=True;" +
        "User Id=sqladmin;" +
        "Password=sql!Admin"
        ));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
SchedulerService scheduler = new SchedulerService();
builder.Services.AddScoped<SchedulerService>();
scheduler.Start();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("*"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
