using GameClubAPI.BackgroundServices;
using GameClubAPI.DBContext;
using GameClubAPI.Dto.Club;
using GameClubAPI.Entities;
using GameClubAPI.Queue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : Controller
    {
        private readonly WriteDbContext _writeContext;
        private readonly ReadDbContext _readContext;
        private readonly DatabaseSynchronizer _synchronizer;
        private readonly IBackgroundTaskQueue _taskQueue;

        public ClubsController(WriteDbContext writeContext,
            ReadDbContext readContext,
            DatabaseSynchronizer synchronizer,
            IBackgroundTaskQueue taskQueue)
        {
            _writeContext = writeContext;
            _readContext = readContext;
            _synchronizer = synchronizer;
            _taskQueue = taskQueue;
        }

        [HttpGet]
        public async Task<IActionResult> GetClubs()
        {
            var clubs = await _readContext.Clubs.ToListAsync();
            return Ok(clubs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClub([FromBody] ClubCreateDto clubDto)
        {
            if (await _writeContext.Clubs.AnyAsync(c => c.Name == clubDto.Name))
                return Conflict(new { message = "Club with this name already exists." });

            var tcs = new TaskCompletionSource<bool>();

            await _taskQueue.QueueBackgroundWorkItemAsync(async token =>
            {
                try
                {
                    var club = new Club { Name = clubDto.Name, Description = clubDto.Description };
                    _writeContext.Clubs.Add(club);
                    await _writeContext.SaveChangesAsync(token);
                    await _synchronizer.SyncData();
                    tcs.SetResult(true);
                }
                catch (Exception ex) { tcs.SetException(ex); }
            });

            return await tcs.Task ? Ok(new { message = "Club created successfully" })
                                  : StatusCode(500, new { message = "Failed to create club" });
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchClubs(string search)
        {
            var clubs = await _readContext.Clubs
                .Where(c => c.Name.Contains(search) || c.Description.Contains(search))
                .ToListAsync();

            if (!clubs.Any())
            {
                return NotFound(new { message = "No clubs found matching the search criteria." });
            }

            return Ok(clubs);
        }
    }
}
