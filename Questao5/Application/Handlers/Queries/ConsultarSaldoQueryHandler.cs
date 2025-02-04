using MediatR;
using Questao5.Application.Helpers;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Interfaces.Repositories;

namespace Questao5.Application.Handlers.Queries
{
    public class ConsultarSaldoQueryHandler : IRequestHandler<ConsultarSaldoQuery, ConsultarSaldoQueryResponse>
    {
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public ConsultarSaldoQueryHandler(IContaCorrenteRepository contaCorrenteRepository,
                                          IMovimentoRepository movimentoRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
        }

        public async Task<ConsultarSaldoQueryResponse> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
        {
            var contaCorrente = await _contaCorrenteRepository.GetByIdAsync(request.IdContaCorrente);

            var response = new ConsultarSaldoQueryResponse()
            {
                DataHoraConsulta = DateTime.Now
            };

            if (contaCorrente == null)
            {
                response.AddError(MensagemErroConstants.ContaInvalidaKey);
                return response;
            }

            if (!contaCorrente.Ativo)
            {
                response.AddError(MensagemErroConstants.ContaInativaKey);
                return response;
            }

            var creditos = await _movimentoRepository.GetTotalByMovimentoTypeAsync(request.IdContaCorrente, "C");
            var debitos = await _movimentoRepository.GetTotalByMovimentoTypeAsync(request.IdContaCorrente, "D");

            response.NumeroContaCorrente = contaCorrente.Numero;
            response.TitularContaCorrente = contaCorrente.Nome;
            response.Saldo = creditos - debitos;

            return response;
        }
    }
}
