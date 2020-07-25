using aspnet_core_jwt_auth_api.Data;
using aspnet_core_jwt_auth_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace aspnet_core_jwt_auth_api.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;
        private readonly IConfiguration _configuration;

        public UserService(UserContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public AuthResponse Authenticate(AuthRequest authRequest)
        {
            //Check if user exists with correct password
            var user = _context.User.SingleOrDefault(x => x.UserName == authRequest.UserName 
                                                        && x.Password == authRequest.Password);

            if (user == null)
                return null;

            //Auth Success;generate jwt token
            var jwtToken = GenerateJWTToken(user);
            return new AuthResponse(user, jwtToken);
        }

        private string GenerateJWTToken(User user)
        {
            //Get the secrete Key
            var jwtSecretKey = _configuration.GetValue<string>("AppSettings:Secret");
            var key = Encoding.ASCII.GetBytes(jwtSecretKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            //Key created which is valid for 10 days and could be configured from appsettings
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public AuthResponse SignIn(User user)
        {
            throw new NotImplementedException();
        }

        public AuthResponse SignOut(User user)
        {
            throw new NotImplementedException();
        }

        public AuthResponse Signup(User user)
        {
            throw new NotImplementedException();
        }

        
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

       
        public async Task<User> GetById(int id)
        {
            var user = await _context.User.FindAsync(id);

            return user;
        }

        
        public async Task<int> PutUser(int id, User user)
        {
            
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return -1;
                }
                else
                {
                    throw;
                }
            }

            return 0;
        }

      
        public async Task<User> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            var  createdUser = await GetById(user.Id);
            return createdUser;
        }

        public async Task<User> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
