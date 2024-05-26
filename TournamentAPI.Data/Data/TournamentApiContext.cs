using TournamentAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace TournamentAPI.Data.Data
{
    public class TournamentApiContext : DbContext
    {
        public TournamentApiContext (DbContextOptions<TournamentApiContext> options)
            : base(options)
        {
        }

        public DbSet<Tournament> Tournament { get; set; } = default!;
        public DbSet<Game> Game { get; set; } = default!;
    }
}
