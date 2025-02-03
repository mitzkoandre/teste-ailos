using AutoMapper;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Contracts;

namespace Questao5.Application.Services
{
    public class ContaCorrenteService : IContaCorrenteService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ContaCorrenteService(IMapper mapper,
                                    IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ConsultarSaldoQueryResponse> ConsultarSaldo(Guid idContaCorrente)
        {
            var query = new ConsultarSaldoQuery() { IdContaCorrente = idContaCorrente };
            var response = await _mediator.Send(query);
            return response;
        }

        public async Task<MovimentarContaCorrenteCommandResponse> MovimentarContaCorrente(MovimentoDTO movimentoDTO)
        {
            var command = _mapper.Map<MovimentarContaCorrenteCommand>(movimentoDTO);
            var response = await _mediator.Send(command);
            return response;
        }
    }
}
