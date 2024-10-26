using System.ComponentModel.DataAnnotations;

namespace GameClubUI.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Event title is required.")]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Scheduled date/time is required.")]
        public DateTime ScheduledDateTime { get; set; } = DateTime.Now;

        public int ClubId { get; set; }
        public Club? Club { get; set; }
    }
}
