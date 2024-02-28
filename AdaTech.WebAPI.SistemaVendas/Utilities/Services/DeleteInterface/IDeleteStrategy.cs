using AdaTech.WebAPI.DadosLibrary.Repository;
using Microsoft.EntityFrameworkCore;

namespace AdaTech.WebAPI.SistemaVendas.Utilities.Services.DeleteInterface
{
    public interface IDeleteStrategy<T> where T : class
    {
        Task<string> DeleteAsync(IRepository<T> repository, ILogger logger, DbContext context, int id);
    }

}
