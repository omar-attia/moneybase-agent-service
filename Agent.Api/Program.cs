using Agent.Dal;
using Agent.Dal.Data;
using Agent.Dal.Data.Interfaces;
using Agent.Dal.Interfaces;
using Agent.Service;
using Agent.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using RabbitMQLibrary;
using RabbitMQLibrary.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AgentDb");
builder.Services.AddDbContext<AgentDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IAgentDbContext, AgentDbContext>();
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

builder.Services.AddSingleton<ConsumerService>();


builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<IAgentAssignmentService, AgentAssignmentService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPendingQueuedSessionService, PendingQueuedSessionService>();

builder.Services.AddScoped<IAgentDal, AgentDal>();
builder.Services.AddScoped<IAgentAssignmentDal, AgentAssignmentDal>();
builder.Services.AddScoped<ITeamDal, TeamDal>();
builder.Services.AddScoped<IPendingQueuedSessionDal, PendingQueuedSessionDal>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Services.GetRequiredService<ConsumerService>().StartConsuming();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();