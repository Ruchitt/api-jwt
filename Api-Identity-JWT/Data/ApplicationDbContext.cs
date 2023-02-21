using Api_Identity_JWT.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_Identity_JWT.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
        }

        public DbSet<Register> registers { get; set; }
        public DbSet<Login> logins { get; set; }

    }
}
