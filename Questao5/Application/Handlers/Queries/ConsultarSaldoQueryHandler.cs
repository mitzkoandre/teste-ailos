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
                NumeroContaCorrente = contaCorrente.Numero,
                TitularContaCorrente = contaCorrente.Nome,
                DataHoraConsulta = DateTime.Now,
            };

            if (contaCorrente == null || !contaCorrente.Ativo)
            {
                response.AddError($"{MensagemErroConstants.ContaInvalidaKey} ou {MensagemErroConstants.ContaInativaKey}");
                return response;
            }

            var creditos = await _movimentoRepository.GetTotalByMovimentoTypeAsync(request.IdContaCorrente, "C");
            var debitos = await _movimentoRepository.GetTotalByMovimentoTypeAsync(request.IdContaCorrente, "D");

            response.Saldo = creditos - debitos;

            return response;
        }
    }
}
