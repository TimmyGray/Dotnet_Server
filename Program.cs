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

builder.Services.AddSingleton<MongoContext>();

var app = builder.Build();

app.UseCors(builder => builder.WithOrigins("http://localhost:4300")
.AllowAnyHeader()
.AllowAnyMethod());

app.MapControllers();
app.MapGet("/", async context => await context.Response.WriteAsync("Hello there"));

//app.MapGet("/postmsg", async context => await context.Response.WriteAsync(context.Request.Body.Length.ToString()));

//string url = "https://localhost:5000";


app.Run();

//Console.WriteLine($"server listen on port: {url}");

