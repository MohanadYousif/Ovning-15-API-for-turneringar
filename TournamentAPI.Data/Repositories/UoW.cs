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

        public ITournamentRepository tournamentRepository { get; set; }

        public IGameRepository gameRepository { get; set; }

        public async Task CompleteAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
