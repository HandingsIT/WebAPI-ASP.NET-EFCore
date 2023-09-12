global using WebAPISever.Models;
global using WebAPISever.Data;
global using WebAPISever.RequestBody;
global using WebAPISever.ResponseBody;
global using WebAPISever.Manager;
global using System.Net;
using WebAPISever.Services.UserService;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API Server Using ASP.Net Core EF Core", Version = "v1" });
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<DataContext>();


var app = builder.Build();

//Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API Server");
    });
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
