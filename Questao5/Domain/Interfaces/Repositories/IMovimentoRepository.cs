using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.Repositories
{
    public interface IMovimentoRepository
    {
        Task InsertAsync(Movimento entity);
        Task<decimal> GetTotalByMovimentoTypeAsync(Guid idContaCorrente, string tipoMovimento);
    }
}
