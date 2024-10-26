namespace GameClubAPI.BackgroundServices
{
    public class SyncService : BackgroundService
    {
        private readonly DatabaseSynchronizer _synchronizer;

        public SyncService(DatabaseSynchronizer synchronizer)
        {
            _synchronizer = synchronizer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _synchronizer.SyncData();
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
