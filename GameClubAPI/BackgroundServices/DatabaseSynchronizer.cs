using GameClubAPI.DBContext;
using Microsoft.EntityFrameworkCore;

namespace GameClubAPI.BackgroundServices
{
    public class DatabaseSynchronizer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DatabaseSynchronizer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task SyncData()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var writeContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
                var readContext = scope.ServiceProvider.GetRequiredService<ReadDbContext>();

                readContext.Clubs.RemoveRange(readContext.Clubs);
                readContext.Events.RemoveRange(readContext.Events);
                await readContext.SaveChangesAsync();

                var clubs = await writeContext.Clubs.ToListAsync();
                var events = await writeContext.Events.ToListAsync();

                readContext.Clubs.AddRange(clubs);
                readContext.Events.AddRange(events);
                await readContext.SaveChangesAsync();
            }
        }
    }
}
