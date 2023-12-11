using Microsoft.EntityFrameworkCore;
using SAQL.Entities;

namespace SAQL.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext()
        {

        }
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=dbsaql.database.windows.net;Database=SaqlDB;Trusted_Connection=False;Encrypt=True;User Id=sqladmin;Password=sql!Admin");
        }
        public DbSet<User> Users { get; set; }
    }
}
