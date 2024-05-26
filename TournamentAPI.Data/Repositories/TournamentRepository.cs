using TournamentAPI.Data.Data;
using TournamentAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.IRepositories;

namespace TournamentAPI.Data.Repositories
{
    public class TournamentRepository : IRepository<Tournament>
    {

        private readonly TournamentApiContext context;

        public TournamentRepository(TournamentApiContext context)
        {
            this.context = context;
        }

        public void Add(Tournament tournament)
        {
            context.Add<Tournament>(tournament);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await context.Tournament.AnyAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Tournament>> GetAllAsync(bool includeGames)
        {
            return (includeGames) ? await context.Tournament.Where(t => t.Games != null).Include(t => t.Games).ToListAsync()
                : await context.Tournament.Where(t => t.Games == null).ToListAsync();
        }

        public Task<IEnumerable<Tournament>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Tournament> GetAsync(int id)
        {
            return await context.Tournament.FindAsync(id);
        }

        public Task<Tournament> GetAsync(string input)
        {
            throw new NotImplementedException();
        }

        public void Remove(Tournament tournament)
        {
            context.Remove<Tournament>(tournament);
        }

        public void Update(Tournament tournament)
        {
            context.Entry(tournament).State = EntityState.Modified;
        }
    }
}
