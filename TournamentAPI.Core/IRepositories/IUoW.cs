namespace TournamentAPI.Core.IRepositories
{
    public interface IUoW
    {
        ITournamentRepository tournamentRepository { get; }
        IGameRepository gameRepository { get; }
        Task CompleteAsync();
    }
}
