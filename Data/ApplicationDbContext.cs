using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ByuEgyptSite.Models;
using Microsoft.EntityFrameworkCore;

namespace ByuEgyptSite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Burial> Burials { get; set; }
    }
}