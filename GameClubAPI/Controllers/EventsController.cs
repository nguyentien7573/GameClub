using GameClubAPI.BackgroundServices;
using GameClubAPI.DBContext;
using GameClubAPI.Dto.Event;
using GameClubAPI.Entities;
using GameClubAPI.Queue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly WriteDbContext _writeContext;
        private readonly ReadDbContext _readContext;
        private readonly DatabaseSynchronizer _synchronizer;
        private readonly IBackgroundTaskQueue _taskQueue;

        public EventsController(
            WriteDbContext writeContext,
            ReadDbContext readContext,
            DatabaseSynchronizer synchronizer,
            IBackgroundTaskQueue taskQueue)
        {
            _writeContext = writeContext;
            _readContext = readContext;
            _synchronizer = synchronizer;
            _taskQueue = taskQueue;
        }

        [HttpPost("/api/clubs/{clubId}/events")]
        public async Task<IActionResult> CreateEvent(int clubId, [FromBody] EventCreateDto eventDto)
        {
            var club = await _readContext.Clubs.FindAsync(clubId);
            if (club == null)
                return NotFound(new { message = "Club not found." });

            var existingEvent = await _readContext.Events.AnyAsync(e => e.ClubId == clubId && 
                                                                        e.Title == eventDto.Title);

            if (existingEvent)
            {
                return Conflict("An event with the same title already exists in this club.");
            }

            var tcs = new TaskCompletionSource<bool>();

            await _taskQueue.QueueBackgroundWorkItemAsync(async token =>
            {
                try
                {
                    var gameEvent = new Event
                    {
                        Title = eventDto.Title,
                        Description = eventDto.Description,
                        ScheduledDateTime = eventDto.ScheduledDateTime,
                        ClubId = clubId
                    };
                    _writeContext.Events.Add(gameEvent);
                    await _writeContext.SaveChangesAsync(token);
                    await _synchronizer.SyncData();
                    tcs.SetResult(true);
                }
                catch (Exception ex) { tcs.SetException(ex); }
            });

            return await tcs.Task ? Ok(new { message = "Event created successfully" })
                                  : StatusCode(500, new { message = "Failed to create event" });
        }

        [HttpGet("/api/clubs/{clubId}/events")]
        public async Task<IActionResult> GetClubEvents(int clubId)
        {
            var events = await _readContext.Events
                .Where(e => e.ClubId == clubId)
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    ScheduledDateTime = e.ScheduledDateTime,
                    ClubId = e.ClubId
                })
                .ToListAsync();

            return Ok(events);
        }
    }
}
