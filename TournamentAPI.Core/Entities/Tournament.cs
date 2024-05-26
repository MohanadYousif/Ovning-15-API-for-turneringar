using System.ComponentModel.DataAnnotations;

namespace TournamentAPI.Core.Entities
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}
