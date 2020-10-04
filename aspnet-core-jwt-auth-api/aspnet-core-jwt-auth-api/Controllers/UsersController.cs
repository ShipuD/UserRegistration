using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aspnet_core_jwt_auth_api.Data;
using aspnet_core_jwt_auth_api.Models;
using aspnet_core_jwt_auth_api.Services;
using Microsoft.AspNetCore.Cors;
using System.Security.Cryptography;
using System.Text;

namespace aspnet_core_jwt_auth_api.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {   
            _userService = userService;
        }

        /// <summary>
        /// Create the hash of the password of the user
        /// </summary>
        /// <param name="item">User</param>
        private string GenerateHash(User item)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(item.Password));

                StringBuilder stringbuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringbuilder.Append(bytes[i].ToString("x2"));
                }

                item.Password = stringbuilder.ToString();
            }
            return item.Password;
        }

        /// <summary>
        /// Create the hash of the password of the user
        /// </summary>
        /// <param name="item">AuthRequest</param>
        private string GenerateHash(AuthRequest item)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(item.Password));

                StringBuilder stringbuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringbuilder.Append(bytes[i].ToString("x2"));
                }

                item.Password = stringbuilder.ToString();
            }
            return item.Password;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthRequest authRequest)
        {
            // generate the hash
            authRequest.Password = GenerateHash(authRequest);

            var response = _userService.Authenticate(authRequest);
            if(response == null)
            {
                return BadRequest(
                    new { message = "User Name or Password is incorrect" }
                );
            }
            return Ok(response);
        }
        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userService.GetUsers();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            // generate the hash
            user.Password = GenerateHash(user);

            var returnResult =await _userService.PutUser(id, user);
            if (returnResult == 1)
            {
                return Ok();
            }
            else 
            {
                 return BadRequest(
                    new { message = "User update has failed and retry again." }
                );
            }
        }

        // POST: api/Users
        [HttpPost("signup")]
        public async Task<ActionResult<User>> Signup(User user)
        {
            //Check if user exists
            var existingUsers = await _userService.GetUsers();
            var existingUser = existingUsers.Count(r => r.UserName == user.UserName);
            if (existingUser > 0)
            {
                return BadRequest(
                   new { message = "UserName already exists. Please choose some other User Name" }
               );
            }

            // generate the hash
            user.Password = GenerateHash(user);

            var retUser = await _userService.PostUser(user);
            if(retUser.Id <= 0)
            {
                return BadRequest(
                   new { message = "Registration failed, retry again." }
               );
            }
            return retUser;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _userService.DeleteUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
    }
}
