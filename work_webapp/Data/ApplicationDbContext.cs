using Microsoft.EntityFrameworkCore;
using work_webapp.Models;

namespace work_webapp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<RawJson> RawJson => Set<RawJson>();
    }
}
