using System.ComponentModel.DataAnnotations;

namespace TournamentAPI.Core.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public int? TournamentId { get; set; }
        public Tournament Tournament { get; set; }
    }
}
