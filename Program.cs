using Aspnet_server.mail_sender;
using Aspnet_server.Infrastructure;
using BuyingLibrary.Actions;
using BuyingLibrary.AppSettings;
using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services
    .AddHealthChecks()
    .AddCheck<MongoHealthCheck>("mongodb")
    .AddCheck<MailHealthCheck>("mail");

builder.Services
    .AddOptions<ConnectionStringsOptions>()
    .Bind(builder.Configuration.GetSection(ConnectionStringsOptions.ConnectionStrings))
    .Validate(opt => !string.IsNullOrWhiteSpace(opt.ClientUrl), "ConnectionStrings:ClientUrl is required")
    .ValidateOnStart();

builder.Services
    .AddOptions<DataBaseOptions>()
    .Bind(builder.Configuration.GetSection(DataBaseOptions.DataBaseSettings))
    .Validate(opt => !string.IsNullOrWhiteSpace(opt.DataBaseConnection), "DataBaseSettings:DataBaseConnection is required")
    .Validate(opt => !string.IsNullOrWhiteSpace(opt.DataBase), "DataBaseSettings:DataBase is required")
    .ValidateOnStart();

builder.Services
    .AddOptions<MailOptions>()
    .Bind(builder.Configuration.GetSection(MailOptions.EmailSettings))
    .ValidateOnStart();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientCors", policy =>
    {
        var clientUrl = builder.Configuration[$"{ConnectionStringsOptions.ConnectionStrings}:ClientUrl"];
        if (!string.IsNullOrWhiteSpace(clientUrl))
        {
            policy.WithOrigins(clientUrl)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

builder.Services.AddSingleton<MongoContext>();
builder.Services.AddScoped<IService<Buy>, BuyingService>();
builder.Services.AddScoped<IService<Client>, ClientService>();
builder.Services.AddScoped<IService<Coil>, CoilService>();
builder.Services.AddScoped<IService<Connector>, ConnectorService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<PriceService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<DeserAction>();
builder.Services.AddScoped<IMailSender, MailSender>();

var app = builder.Build();

app.UseExceptionHandler();
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("ClientCors");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
app.MapHealthChecks("/health");
app.MapGet("/", () => Results.Ok(new { message = "Buying server is running" }));

app.Run();

public partial class Program;
