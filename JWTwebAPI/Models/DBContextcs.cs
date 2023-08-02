using Microsoft.EntityFrameworkCore;

namespace JWTwebAPI.Models
{
    public class DBContextcs :DbContext 
    {
        public DBContextcs(DbContextOptions<DBContextcs> options) :base (options)
        {
            
        }
        DbSet<Brands> Brands { get; set; }  

    }
}
