using System.ComponentModel.DataAnnotations;

namespace GameClubAPI.Dto.Event
{
    public class EventCreateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime ScheduledDateTime { get; set; }

        [Required]
        public int ClubId { get; set; }
    }
}
