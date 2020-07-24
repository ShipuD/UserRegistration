using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using aspnet_core_jwt_auth_api.Models;

namespace aspnet_core_jwt_auth_api.Data
{
    public class UserContext : DbContext
    {
        public UserContext (DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public DbSet<aspnet_core_jwt_auth_api.Models.User> User { get; set; }
    }
}
