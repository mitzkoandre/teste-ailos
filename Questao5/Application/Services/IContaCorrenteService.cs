using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Responses;
using Questao5.Contracts;

namespace Questao5.Application.Services
{
    public interface IContaCorrenteService
    {
        Task<ConsultarSaldoQueryResponse> ConsultarSaldo(Guid idContaCorrente);
        Task<MovimentarContaCorrenteCommandResponse> MovimentarContaCorrente(MovimentoDTO movimentoViewModel);
    }
}
