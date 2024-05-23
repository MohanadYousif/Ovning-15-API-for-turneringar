using Microsoft.EntityFrameworkCore;

namespace TournamentAPI.Data.Data
{
    public class TournamentApiContext : DbContext
    {
        public TournamentApiContext (DbContextOptions<TournamentApiContext> options)
            : base(options)
        {
        }

        public DbSet<TournamentAPI.Core.Entities.Tournament> Tournament { get; set; } = default!;
        public DbSet<TournamentAPI.Core.Entities.Game> Game { get; set; } = default!;
    }
}
