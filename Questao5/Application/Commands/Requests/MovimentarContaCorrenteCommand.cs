using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentarContaCorrenteCommand : IRequest<MovimentarContaCorrenteCommandResponse>
    {
        public Guid IdIdempotencia { get; set; }
        public Guid IdContaCorrente { get; set; }
        public string TipoMovimento { get; set; }
        public double Valor { get; set; }
        public bool Ativo { get; set; }
    }
}
