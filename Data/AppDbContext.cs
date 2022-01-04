using ClientService.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientService.Data
{
    public class AppDbContext : DbContext
    {
        // Pont entre notre bdd et notre model 
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        //On set le model avec le nom de la bdd
        public DbSet<Client> Client { get; set; }
    }
}