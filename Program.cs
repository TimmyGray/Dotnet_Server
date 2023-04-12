using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.Configure<Settings>(options=> {
    options.ConnectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
    options.DataBase = builder.Configuration.GetSection("ConnectionStrings:DataBase").Value;
});
string url = builder.Configuration.GetConnectionString("AppUrl");
builder.Services.AddSingleton<MongoContext>();

var app = builder.Build();

app.UseCors(builder => builder.WithOrigins("http://localhost:4300")
.AllowAnyHeader()
.AllowAnyMethod());

app.MapControllers();
app.MapGet("/", async context => await context.Response.WriteAsync("Hello there"));

app.Run(url);

