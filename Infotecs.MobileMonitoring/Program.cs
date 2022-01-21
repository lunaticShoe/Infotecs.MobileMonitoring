using System.Reflection;
using Infotecs.MobileMonitoring.Data;
using Infotecs.MobileMonitoring.Extensions;
using Infotecs.MobileMonitoring.Factories;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Repositories;
using Infotecs.MobileMonitoring.Services;
using Infotecs.MobileMonitoring.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => 
{
    //options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});
builder.Services.AddRouting(options => options.LowercaseUrls = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddNSwag();
builder.Services.AddMongoDbContext(builder.Configuration["mongodbConnection"]);
builder.Services.AddMongoDbMigrations();
builder.Services.AddMapsterConfiguration();
builder.Services.AddSerilogServices(
    new LoggerConfiguration(), 
    builder.Configuration["seqConnection"]);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ISessionContainerFactory, SessionContainerFactory>();
builder.Services.AddSingleton<IMongoDbReplicaSetWorker>(s => 
    new MongoDbReplicaSetWorker(
        builder.Configuration["mongodbConnection"], 
        s.GetRequiredService<ILogger>()));

//builder.Services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
//builder.Services.AddScoped<IStatisticsRepository, StatisticsRepository>();
//builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddTransient<IEventDictionaryService, EventDictionaryService>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();
builder.Services.AddTransient<IEventService, EventService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerInfrastructure();
}

var migrator = app.Services.GetRequiredService<IMongoDbMigrator>();
var replicaSetWorker = app.Services.GetRequiredService<IMongoDbReplicaSetWorker>();

replicaSetWorker.InitReplicaSet();
migrator.Migrate();
//app.UseHttpsRedirection();

app.UseRouting();
app.UseCors(
    options => options
        //.WithOrigins("http://localhost:5000")
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );

app.UseAuthorization();

app.MapControllers();

app.Run();