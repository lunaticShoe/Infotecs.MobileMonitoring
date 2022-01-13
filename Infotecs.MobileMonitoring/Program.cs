using System.Reflection;
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
builder.Services.AddOpenApiDocument(doc => 
    doc.PostProcess = document =>
    {
        document.Info.Title = "Infotecs Monitoring Service API";
    });
//builder.Services.AddOpenApiDocument();
//builder.Services.AddSwaggerDocument();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Version = "v1",
//         Title = "Infotecs Monitoring Service",
//         Description = "Infotecs Monitoring Service API",
//     });
//     // Set the comments path for the Swagger JSON and UI.
//     var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//     c.IncludeXmlComments(xmlPath);
// });

builder.Services.AddMapsterConfiguration();
builder.Services.AddSerilogServices(new LoggerConfiguration());
builder.Services.AddSingleton<IStatisticsRepository, StatisticsRepository>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
    // app.UseOpenApi(c =>
    // {
    //     c.
    // });
    app.UseSwagger();
    app.UseSwaggerUi3();
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