using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Helpers;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Interfaces.Repositories;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Questao5.Application.Handlers.Commands
{
    public class MovimentarContaCorrenteCommandHandler : IRequestHandler<MovimentarContaCorrenteCommand, MovimentarContaCorrenteCommandResponse>
    {
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;

        public MovimentarContaCorrenteCommandHandler(IContaCorrenteRepository contaCorrenteRepository,
                                                     IIdempotenciaRepository idempotenciaRepository,
                                                     IMovimentoRepository movimentoRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _idempotenciaRepository = idempotenciaRepository;
            _movimentoRepository = movimentoRepository;
        }

        public async Task<MovimentarContaCorrenteCommandResponse> Handle(MovimentarContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            var contaCorrente = await _contaCorrenteRepository.GetByIdAsync(request.IdContaCorrente);
            var response = new MovimentarContaCorrenteCommandResponse();

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

            var errors = await ValidateRequestAsync(request);

            if (errors.Count > 0)
            {
                response.AddErrors(errors);
                return response;
            }

            var movimento = new Movimento(Guid.NewGuid(),
                                          request.IdContaCorrente,
                                          DateTime.Now.ToString(),
                                          request.TipoMovimento.ToUpper(),
                                          request.Valor);

            await _movimentoRepository.InsertAsync(movimento);

            response.IdMovimento = movimento.IdMovimento;

            var idempotencia = new Idempotencia(request.IdIdempotencia,
                                                JsonSerializer.Serialize(request),
                                                JsonSerializer.Serialize(response));

            await _idempotenciaRepository.InsertAsync(idempotencia);

            return response;
        }

        private async Task<List<string>> ValidateRequestAsync(MovimentarContaCorrenteCommand request)
        {
            var errors = new List<string>();

            var tipoMovimentoChar = char.Parse(request.TipoMovimento);

            if (string.IsNullOrEmpty(request.TipoMovimento) ||
                                    !(tipoMovimentoChar == (char)TipoMovimento.Credito || tipoMovimentoChar == (char)TipoMovimento.Debito))
            {
                errors.Add(MensagemErroConstants.TipoMovimentoInvalidoKey);
            }

            if (request.Valor <= 0)
            {
                errors.Add(MensagemErroConstants.ValorInvalidoKey);
            }

            if (request.IdIdempotencia != Guid.Empty)
            {
                var movimentoExistente = await _idempotenciaRepository.GetByIdAsync(request.IdIdempotencia);

                if (movimentoExistente != null)
                    errors.Add("Movimento já processado anteriormente");
            }

            return errors;
        }
    }
}
