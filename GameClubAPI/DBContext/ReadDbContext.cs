using GameClubAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameClubAPI.DBContext
{
    public class ReadDbContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Event> Events { get; set; }

        public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }
    }
}
