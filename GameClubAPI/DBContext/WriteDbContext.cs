using GameClubAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameClubAPI.DBContext
{
    public class WriteDbContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Event> Events { get; set; }

        public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options) { }
    }
}
