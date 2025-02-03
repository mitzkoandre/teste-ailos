using FluentValidation.Results;
using Questao5.Application.Helpers;

namespace Questao5.Application.Commands.Responses
{
    public class MovimentarContaCorrenteCommandResponse : ResponseHelperAPI
    {
        public Guid IdMovimento { get; set; }
    }
}
