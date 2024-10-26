﻿namespace GameClubAPI.Queue
{
    public interface IBackgroundTaskQueue
    {
        Task QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem);

        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
