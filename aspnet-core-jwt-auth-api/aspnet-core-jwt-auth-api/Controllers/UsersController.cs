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

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthRequest authRequest)
        {
            var response = _userService.Authenticate(authRequest);
            if(response == null)
            {
                return BadRequest(
                    new { message = "User Name or Password is in correct" }
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var returnResult =await _userService.PutUser(id, user);
            if (returnResult == 1)
            {
                return Ok();
            }
            else 
            {
                 return BadRequest(
                    new { message = "User registarion has failed and retry again" }
                );
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var retUser = await _userService.PostUser(user);
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
