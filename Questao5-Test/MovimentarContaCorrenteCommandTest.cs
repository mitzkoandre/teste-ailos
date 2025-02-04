using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers.Commands;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.Repositories;
using Xunit;

namespace Questao5_TestsS
{
    public class MovimentarContaCorrenteCommandTest
    {
        private readonly Mock<IIdempotenciaRepository> _mockIdempotenciaRepository;
        private readonly Mock<IMovimentoRepository> _mockMovimentoRepository;
        private readonly Mock<IContaCorrenteRepository> _mockContaCorrenteRepository;
        private readonly MovimentarContaCorrenteCommandHandler _handler;

        public MovimentarContaCorrenteCommandTest()
        {
            _mockIdempotenciaRepository = new Mock<IIdempotenciaRepository>();
            _mockMovimentoRepository = new Mock<IMovimentoRepository>();
            _mockContaCorrenteRepository = new Mock<IContaCorrenteRepository>();

            _handler = new MovimentarContaCorrenteCommandHandler(_mockContaCorrenteRepository.Object,
                                                                 _mockIdempotenciaRepository.Object,
                                                                 _mockMovimentoRepository.Object);

        }

        [Fact]
        public async Task HandleMovimentoValido()
        {
            var command = new MovimentarContaCorrenteCommand()
            {
                IdIdempotencia = Guid.NewGuid(),
                IdContaCorrente = Guid.NewGuid(),
                TipoMovimento = "C",
                Valor = 250
            };

            _mockContaCorrenteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new ContaCorrente(Guid.Parse("382D323D-7067-ED11-8866-7D5DFA4A16C9"),
                                                789,
                                                "Tevin Mcconnell",
                                                true));

            _mockIdempotenciaRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Idempotencia)null);

            _mockMovimentoRepository.Setup(repo => repo.InsertAsync(It.IsAny<Movimento>()))
                .Returns(Task.CompletedTask);

            _mockIdempotenciaRepository.Setup(repo => repo.InsertAsync(It.IsAny<Idempotencia>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.HasErrors);
            Assert.NotNull(result.IdMovimento);

            _mockMovimentoRepository.Verify(repo => repo.InsertAsync(It.IsAny<Movimento>()), Times.Once);
            _mockIdempotenciaRepository.Verify(repo => repo.InsertAsync(It.IsAny<Idempotencia>()), Times.Once);
        }

        [Fact]
        public async Task HandleMovimentoJaProcessadoErro()
        {
            var command = new MovimentarContaCorrenteCommand()
            {
                IdIdempotencia = Guid.NewGuid(),
                IdContaCorrente = Guid.NewGuid(),
                TipoMovimento = "C",
                Valor = 250
            };

            _mockContaCorrenteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new ContaCorrente(Guid.Parse("382D323D-7067-ED11-8866-7D5DFA4A16C9"),
                                                789,
                                                "Tevin Mcconnell",
                                                true));

            _mockIdempotenciaRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Idempotencia());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.HasErrors);
            Assert.Contains("Movimento já processado anteriormente", result.GetErrors.FirstOrDefault().Value);
        }

        [Fact]
        public async Task HandleMovimentoContaInvalida()
        {
            var command = new MovimentarContaCorrenteCommand()
            {
                IdIdempotencia = Guid.NewGuid(),
                IdContaCorrente = Guid.NewGuid(),
                TipoMovimento = "C",
                Valor = 250
            };

            _mockContaCorrenteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ContaCorrente)null);

            _mockIdempotenciaRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Idempotencia());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.HasErrors);
            Assert.Contains("INVALID_ACCOUNT", result.GetErrors.FirstOrDefault().Value);
        }

        [Fact]
        public async Task HandleMovimentoContaInvativa()
        {
            var command = new MovimentarContaCorrenteCommand()
            {
                IdIdempotencia = Guid.NewGuid(),
                IdContaCorrente = Guid.NewGuid(),
                TipoMovimento = "C",
                Valor = 250
            };

            _mockContaCorrenteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new ContaCorrente(Guid.Parse("382D323D-7067-ED11-8866-7D5DFA4A16C9"),
                                                789,
                                                "Tevin Mcconnell",
                                                false));

            _mockIdempotenciaRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Idempotencia());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.HasErrors);
            Assert.Contains("INACTIVE_ACCOUNT", result.GetErrors.FirstOrDefault().Value);
        }

        [Fact]
        public async Task HandleTipoMovimentoInvalido()
        {
            var command = new MovimentarContaCorrenteCommand()
            {
                IdIdempotencia = Guid.NewGuid(),
                IdContaCorrente = Guid.NewGuid(),
                TipoMovimento = "X",
                Valor = 250
            };

            _mockContaCorrenteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new ContaCorrente(Guid.Parse("382D323D-7067-ED11-8866-7D5DFA4A16C9"),
                                                789,
                                                "Tevin Mcconnell",
                                                true));

            _mockIdempotenciaRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Idempotencia)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.HasErrors);
            Assert.Contains("INVALID_TYPE", result.GetErrors.FirstOrDefault().Value);
        }

        [Fact]
        public async Task HandleValorMovimentoInvalido()
        {
            var command = new MovimentarContaCorrenteCommand()
            {
                IdIdempotencia = Guid.NewGuid(),
                IdContaCorrente = Guid.NewGuid(),
                TipoMovimento = "C",
                Valor = -250
            };

            _mockContaCorrenteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new ContaCorrente(Guid.Parse("382D323D-7067-ED11-8866-7D5DFA4A16C9"),
                                                789,
                                                "Tevin Mcconnell",
                                                true));

            _mockIdempotenciaRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Idempotencia)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.HasErrors);
            Assert.Contains("INVALID_VALUE", result.GetErrors.FirstOrDefault().Value);
        }
    }
}
