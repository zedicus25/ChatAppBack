using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccessEF.Data
{
    public partial class ChatDbContext : IdentityDbContext<IdentityUser>
    {
        protected readonly IConfiguration _configuration;
        public ChatDbContext(DbContextOptions configuration) : base(configuration)
        {
        }
    }
}
