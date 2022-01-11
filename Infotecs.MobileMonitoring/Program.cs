using Infotecs.MobileMonitoring.Extensions;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Repositories;
using Infotecs.MobileMonitoring.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSerilogServices(new LoggerConfiguration());
builder.Services.AddSingleton<IStatisticsRepository, StatisticsRepository>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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