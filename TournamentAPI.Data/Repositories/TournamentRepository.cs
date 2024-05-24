using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.IRepositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories
{
    public  class TournamentRepository : IRepository<Tournament>
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

        public async Task<IEnumerable<Tournament>> GetAllAsync()
        {
            return await context.Tournament.ToListAsync();
        }

        public async Task<Tournament> GetAsync(int id)
        {
            return await context.Tournament.FindAsync(id);
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
