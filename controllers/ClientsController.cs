using BuyingLibrary.Contexts;
using BuyingLibrary.models.classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BuyingLibrary.AppSettings;
using Microsoft.IdentityModel.Tokens;

namespace Aspnet_server.controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ClientsController:ControllerBase
    {

        private readonly IService<Client> service;
        private readonly JWTOptions _jwtopt;

        public ClientsController(MongoContext context, JWTOptions opt)
        {

            service = new ClientService(context);
            _jwtopt = opt;

        }

        [Authorize]
        [HttpGet("id:length(24)")]
        public async Task<ActionResult<Client>> GetClient(string id)
        {

            var client = await service.GetAsync(id);
            
            if (client!=null)
            {
                return Ok(client);
            }

            return BadRequest();

        }

        [Authorize]
        [HttpDelete("id:length(24)")]
        public async Task<ActionResult<Client>> DeleteClient(string id)
        {

            return await service.DeleteAsync(id);

        }

        [Route("registration")]
        [HttpPost]
        public async Task<ActionResult<Client>> Registration(RegClientValid regClient)
        {
            var validation_results = new List<ValidationResult>();
            var validation_context = new ValidationContext(regClient);

            if(Validator.TryValidateObject(regClient,validation_context,validation_results,true))
            {
                var new_client =await service.GetAsync(regClient.Login,regClient.Email);
                if (new_client!=null)
                {
                    Console.WriteLine(new_client.ToString());
                    return BadRequest(new ArgumentException("This client already exists"));
                }

                Client client = new Client(regClient.Login,regClient.Email, regClient.Password);
                
                var result = await service.PostAsync(client);
                var response = new { 
                    access_token = MakeToken(result),
                    login = result.Name,
                    email = result.Email,
                };

                return Ok(response);

            }

            foreach (var res in validation_results)
            {
                Console.WriteLine(res.ErrorMessage);
            }
    
            return BadRequest(validation_results);

        }

        [HttpPost("/login")]
        public async Task<ActionResult> Login(LogClientValid logClient)
        {
            List<ValidationResult> validation_results = new List<ValidationResult>();
            ValidationContext validation_context = new ValidationContext(logClient);

            if (Validator.TryValidateObject(logClient, validation_context, validation_results, true))
            {
                var client = await service.GetAsync(logClient.EmailOrLogin);
                if (client!=null)
                {
                    if (BCrypt.Net.BCrypt.Verify(logClient.Password, client.Password))
                    {
                        var response = new{
                            access_token = MakeToken(client),
                            login = client.Name,
                            email = client.Email,
                        };

                        return Ok(response);
                    }
                }

                return Unauthorized("Invalid password or user");

            }

            foreach (var item in validation_results)
            {
                Console.WriteLine(item.ErrorMessage);
            }
            return BadRequest(validation_results);

        }

        private string MakeToken(Client client)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,client.Name),
                new Claim(ClaimTypes.Email,client.Email),
            };

            var jwt = new JwtSecurityToken(
                issuer: _jwtopt.ISSUER,
                audience: _jwtopt.ISSUER,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
                signingCredentials: new SigningCredentials(_jwtopt.GetSymmetricKey(),SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

    }
}
