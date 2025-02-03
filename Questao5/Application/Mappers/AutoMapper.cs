using AutoMapper;
using Questao5.Application.Commands.Requests;
using Questao5.Contracts;

namespace Questao5.Application.Mappers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<MovimentoDTO, MovimentarContaCorrenteCommand>()
                .ForMember(dest => dest.IdIdempotencia, opt => opt.MapFrom(src => src.IdIdempotencia ?? Guid.NewGuid()));
        }
    }
}
