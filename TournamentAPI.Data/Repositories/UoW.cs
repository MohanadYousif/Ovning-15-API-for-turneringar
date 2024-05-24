using TournamentAPI.Core.Entities;
using TournamentAPI.Core.IRepositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories
{
    public class UoW : IUoW
    {
        private TournamentApiContext dbContext;

        public UoW(TournamentApiContext dbContext)
        {
            this.dbContext = dbContext;
            tournamentRepository = new TournamentRepository(dbContext);
            gameRepository = new GameRepository(dbContext);

        }

        public IRepository<Tournament> tournamentRepository { get; set; }

        public IRepository<Game> gameRepository { get; set; }

        public async Task CompleteAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
