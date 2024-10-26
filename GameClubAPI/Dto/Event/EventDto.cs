namespace GameClubAPI.Dto.Event
{
    public class EventDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime ScheduledDateTime { get; set; }

        public int ClubId { get; set; }
    }
}
