var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(builder => builder.WithOrigins("http://localhost:4200", "http://localhost:4300")
.AllowAnyHeader()
.AllowAnyMethod()); 

app.MapGet("/", async context => await context.Response.WriteAsync("GetMessage"));

app.MapGet("/postmsg", async context => await context.Response.WriteAsync(context.Request.Body.Length.ToString()));

//string url = "https://localhost:5000";


app.Run();

//Console.WriteLine($"server listen on port: {url}");

