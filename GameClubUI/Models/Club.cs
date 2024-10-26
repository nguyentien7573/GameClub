using System.ComponentModel.DataAnnotations;

namespace GameClubUI.Models
{
    public class Club
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Club name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        public List<Event> Events { get; set; } = new();
    }
}
