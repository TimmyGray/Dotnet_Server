using BuyingLibrary.Contexts;
using BuyingLibrary.AppSettings;
using Aspnet_server.mail_sender;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.Configure<ConnectionStringsOptions>(
    builder.Configuration.GetSection(ConnectionStringsOptions.ConnectionStrings));

builder.Services.Configure<DataBaseOptions>(
    builder.Configuration.GetSection(DataBaseOptions.DataBaseSettings));

builder.Services.Configure<MailOptions>(
    builder.Configuration.GetSection(MailOptions.EmailSettings));

//builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWTSettings"));
//var jwtsettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddSingleton<MongoContext>();
builder.Services.AddSingleton<MailSender>();

JWTOptions jWTOptions = new JWTOptions(builder.Configuration.GetSection("JWTSettings"));
builder.Services.AddSingleton(jWTOptions);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        //var jwtsettings = builder.Configuration.GetSection("JWTSettings");
        
        //JWTOptions options = new JWTOptions(jwtsettings);
        Console.WriteLine(jWTOptions.ISSUER);
        Console.WriteLine(jWTOptions.KEY);
        Console.WriteLine(jWTOptions.AUDIENCE);
       
        opt.TokenValidationParameters = new TokenValidationParameters {

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jWTOptions.ISSUER,
            ValidAudience = jWTOptions.AUDIENCE,
            IssuerSigningKey = jWTOptions.GetSymmetricKey(),

        };

    });

//builder.Services.AddSingleton<JWTOptions>();

var app = builder.Build();
var conopt = app.Services.GetService<IOptions<ConnectionStringsOptions>>();
app.UseCors((builder) => builder.WithOrigins(conopt.Value.ClientUrl)
.AllowAnyHeader()
.AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/",async (context) =>
{
    var conf = app.Configuration;
    var appurl = conopt.Value.AppUrl;
    var clienturl = conopt.Value.ClientUrl;
    var email = conf["EmailSettings:Email"];
    if (email=="")
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("The Email provider does not setup! Clients will not recieve message when the order makes");
        Console.ResetColor();
    }
    //var sender = app.Services.GetService<MailSender>();
    //Console.WriteLine($"Sender in program:{sender.Setup}");

    await context.Response.WriteAsync($"Server listen on:{appurl}\nServer listen from:{clienturl}\n{email}");

});

if (app.Environment.IsDevelopment())
{
    app.Run(conopt.Value.AppUrl);
}
else
{
    app.Run();

}

