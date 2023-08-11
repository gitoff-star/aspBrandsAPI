using dotnetJWT.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTwebAPI.Models
{
    public class DBContextcs :DbContext 
    {
        public DBContextcs(DbContextOptions<DBContextcs> options) :base (options)
        {
            
        }
      public  DbSet<Brands> Brands { get; set; }  

        public DbSet<user> Users { get; set; }

    }
}
