using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.IRepositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories
{
    public class GameRepository : IRepository<Game>
    {
        private readonly TournamentApiContext context;

        public GameRepository(TournamentApiContext context)
        {
            this.context = context;
        }

        public void Add(Game game)
        {
            context.Add<Game>(game);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await context.Game.AnyAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await context.Game.ToListAsync();
        }

        public async Task<Game> GetAsync(int id)
        {
            return await context.Game.FindAsync(id);
        }

        public void Remove(Game game)
        {
            context.Remove<Game>(game);
        }

        public void Update(Game game)
        {
            context.Entry(game).State = EntityState.Modified;
        }
    }
}
