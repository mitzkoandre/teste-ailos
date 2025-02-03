using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.Repositories
{
    public interface IContaCorrenteRepository
    {
        Task<ContaCorrente> GetByIdAsync(Guid id);
    }
}
