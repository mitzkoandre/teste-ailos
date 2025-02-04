using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers.Queries;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.Repositories;
using Xunit;

namespace Questao5_Test
{
    public class ConsultarSaldoQueryTest
    {
        private readonly Mock<IMovimentoRepository> _mockMovimentoRepository;
        private readonly Mock<IContaCorrenteRepository> _mockContaCorrenteRepository;
        private readonly ConsultarSaldoQueryHandler _handler;

        public ConsultarSaldoQueryTest()
        {
            _mockMovimentoRepository = new Mock<IMovimentoRepository>();
            _mockContaCorrenteRepository = new Mock<IContaCorrenteRepository>();

            _handler = new ConsultarSaldoQueryHandler(_mockContaCorrenteRepository.Object,
                                                      _mockMovimentoRepository.Object);
        }

        [Fact]
        public async Task HandleConsultaValida()
        {
            var contaCorrente = new ContaCorrente(Guid.Parse("382D323D-7067-ED11-8866-7D5DFA4A16C9"),
                                                  789,
                                                  "Tevin Mcconnell",
                                                  true);

            _mockContaCorrenteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(contaCorrente);

            _mockMovimentoRepository.Setup(repo => repo.GetTotalByMovimentoTypeAsync(It.IsAny<Guid>(), "C"))
                .ReturnsAsync(1000);
            _mockMovimentoRepository.Setup(repo => repo.GetTotalByMovimentoTypeAsync(It.IsAny<Guid>(), "D"))
                .ReturnsAsync(500);

            var request = new ConsultarSaldoQuery { IdContaCorrente = Guid.NewGuid() };

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.False(result.HasErrors);
            Assert.Equal(contaCorrente.Numero, result.NumeroContaCorrente);
            Assert.Equal(contaCorrente.Nome, result.TitularContaCorrente);
            Assert.Equal(500, result.Saldo);
            Assert.Equal(DateTime.Now.Date, result.DataHoraConsulta.Date);
        }

        [Fact]
        public async Task HandleContaInvalida()
        {
            var request = new ConsultarSaldoQuery { IdContaCorrente = Guid.NewGuid() };

            _mockContaCorrenteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ContaCorrente)null);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.True(result.HasErrors);
            Assert.Contains("INVALID_ACCOUNT", result.GetErrors.FirstOrDefault().Value);
        }

        [Fact]
        public async Task HandleMovimentoContaInvativa()
        {
            var request = new ConsultarSaldoQuery { IdContaCorrente = Guid.NewGuid() };

            _mockContaCorrenteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new ContaCorrente(Guid.Parse("382D323D-7067-ED11-8866-7D5DFA4A16C9"),
                                                789,
                                                "Tevin Mcconnell",
                                                false));

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.True(result.HasErrors);
            Assert.Contains("INACTIVE_ACCOUNT", result.GetErrors.FirstOrDefault().Value);
        }
    }
}
