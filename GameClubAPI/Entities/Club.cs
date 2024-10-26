using System.ComponentModel.DataAnnotations;

namespace GameClubAPI.Entities
{
    public class Club
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Club name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
