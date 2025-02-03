using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Services;
using Questao5.Contracts;

namespace Questao5.Infrastructure.Services.Controllers
{
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IContaCorrenteService _contaCorrenteService;

        public ContaCorrenteController(IContaCorrenteService contaCorrenteService)
        {
            _contaCorrenteService = contaCorrenteService;
        }

        [HttpGet("saldo/{idContaCorrente}")]
        public async Task<IActionResult> ConsultarSaldo(Guid idContaCorrente)
        {
            try
            {
                if (idContaCorrente == Guid.Empty)
                    return NotFound();

                var response = await _contaCorrenteService.ConsultarSaldo(idContaCorrente);

                return response.HasErrors ? BadRequest(response.GetErrors) : Ok(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("movimentar")]
        public async Task<IActionResult> Movimentar([FromBody] MovimentoDTO movimentoViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    BadRequest(movimentoViewModel);

                var response = await _contaCorrenteService.MovimentarContaCorrente(movimentoViewModel);

                return response.HasErrors ? BadRequest(response.GetErrors) : Ok(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
