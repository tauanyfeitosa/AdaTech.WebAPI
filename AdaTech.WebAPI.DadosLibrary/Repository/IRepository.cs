
namespace AdaTech.WebAPI.DadosLibrary.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync();
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetInactiveAsync();
        Task<T> GetByIdAsync(int id);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<T> GetByIdActivateAsync(int id);
    }

}
