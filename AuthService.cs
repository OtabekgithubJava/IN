using JobBoard.Application.Services.UserService;
using JobBoard.Domain.Entities.DTOs;
using JobBoard.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JobBoard.Application.Abstractions;
using JobBoard.Application.Abstractions.IService;
using JobBoard.Domain.Entities.DTOs;

namespace JobBoard.Application.Services.AuthService
{
    public class AuthService : IAuthSevice
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepo;

        public AuthService(IConfiguration configuration,IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _configuration = configuration;
        }

        public async Task<ResponseLogin> GenerateToken(RequestLogin request)
        {
            if (request == null)
            {
                return new ResponseLogin()
                {
                    Token = "User Not Found"
                };
            }
            var findUser = await FindUser(request);
            if (findUser!=null)
            {

                var permission = new List<int>();
                
                if (findUser.Role != "Admin")
                {
                    permission = new List<int> { 1, 9, 10, 14, 16 };
                }
                else
                {
                    permission = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16};
                }
                var jsonContent = JsonSerializer.Serialize(permission);
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Role, findUser.Role),
                    new Claim("Login", findUser.Login),
                    new Claim("UserID", findUser.UserId.ToString()),
                    new Claim("CreatedDate", DateTime.UtcNow.ToString()),
                    new Claim("Permissions", jsonContent)
                };

                return await GenerateToken(claims);
            }

            return new ResponseLogin()
            {
                Token = "Un Authorize"
            };
        }
        
        public async Task<string> CreateUser(UserDTO userDTO)
        {
            var res = await _userRepo.GetAll();
            var email = res.Any(x => x.Email == userDTO.Email);
            var login = res.Any(x => x.Login == userDTO.Login);
            if (!email)
            {
                if(!login)
                {
                    var newUser = new User()
                    {
                        FullName = userDTO.FullName,
                        
                        Login = userDTO.Login,
                        Password = userDTO.Password,
                        Email = userDTO.Email,
                        Role = userDTO.Role

                    };
                    await _userRepo.Create(newUser);
                    return "Created";

                }
                return "Such login already exists";
            }
            return "Such email already exists";
        }

        public async Task<ResponseLogin> GenerateToken(IEnumerable<Claim> additionalClaims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var exDate = Convert.ToInt32(_configuration["JWT:ExpireDate"] ?? "10");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64)
            };

            if (additionalClaims?.Any() == true)
                claims.AddRange(additionalClaims);


            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(exDate),
                signingCredentials: credentials);

            return new ResponseLogin()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };


        }
        public async Task<User> FindUser(RequestLogin user)
        {

            var result = await _userRepo.GetByAny(x => x.Login == user.Login);

            if (user.Login == result.Login && user.Password == result.Password)
            {
                return result;
            }

            return new User();
        }
    }
}