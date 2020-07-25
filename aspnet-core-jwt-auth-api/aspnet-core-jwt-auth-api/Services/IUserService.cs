using aspnet_core_jwt_auth_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_core_jwt_auth_api.Services
{
    public interface IUserService
    {

        public AuthResponse Signup(User user);
        public AuthResponse Authenticate(AuthRequest authRequest);
        public AuthResponse SignIn(User user);
        public AuthResponse SignOut(User user);
        public Task<IEnumerable<User>> GetUsers();
        public Task<User> GetById(int Id);
        public Task<int> PutUser(int Id,User user);
        public Task<User> PostUser(User user);
        public Task<User> DeleteUser(int Id);

    }
}
