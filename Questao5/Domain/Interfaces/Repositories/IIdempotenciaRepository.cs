using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.Repositories
{
    public interface IIdempotenciaRepository
    {
        Task<Idempotencia> GetByIdAsync(Guid id);
        Task InsertAsync(Idempotencia entity);
    }
}
