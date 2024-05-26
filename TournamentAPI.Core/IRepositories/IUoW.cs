using TournamentAPI.Core.Entities;

namespace TournamentAPI.Core.IRepositories
{
    public interface IUoW
    {
        IRepository<Tournament> tournamentRepository { get; }
        IRepository<Game> gameRepository { get; }
        Task CompleteAsync();
    }
}
