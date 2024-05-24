
namespace TournamentAPI.Core.IRepositories
{
    public interface IRepository <T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(T input);
        void Update(T input);
        void Remove(T input);
    }
}
