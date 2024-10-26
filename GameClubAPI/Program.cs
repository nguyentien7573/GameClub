using GameClubAPI.BackgroundServices;
using GameClubAPI.DBContext;
using GameClubAPI.Queue;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

#region Config Dbtext
builder.Services.AddDbContext<WriteDbContext>(options =>
    options.UseInMemoryDatabase("WriteDatabase"));

builder.Services.AddDbContext<ReadDbContext>(options =>
    options.UseInMemoryDatabase("ReadDatabase"));
#endregion

builder.Services.AddMemoryCache();

#region Config queue and background job
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<QueuedHostedService>();

builder.Services.AddSingleton<DatabaseSynchronizer>();
builder.Services.AddHostedService<SyncService>();
#endregion


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
