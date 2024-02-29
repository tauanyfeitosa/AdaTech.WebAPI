using AdaTech.WebAPI.DadosLibrary.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdaTech.WebAPI.Aplicacoes.Services.DeleteInterface
{
    public interface IDeleteStrategy<T> where T : class
    {
        Task<string> DeleteAsync(IRepository<T> repository, ILogger logger, DbContext context, int id);
    }

}
