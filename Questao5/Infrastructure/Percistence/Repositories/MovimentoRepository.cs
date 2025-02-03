using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.Repositories;
using System.Data;

namespace Questao5.Infrastructure.Percistence.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly IDbConnection _dbConnection;

        public MovimentoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task InsertAsync(Movimento movimento)
        {
            await _dbConnection.ExecuteAsync(
                @"INSERT INTO 
                    movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                  VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
                movimento);
        }

        public async Task<decimal> GetTotalByMovimentoTypeAsync(Guid idContaCorrente, string tipoMovimento)
        {
            return await _dbConnection.QuerySingleAsync<decimal>(
                @"SELECT 
                    COALESCE(SUM(valor), 0) 
                FROM 
                    movimento 
                WHERE 
                    idcontacorrente = @idContaCorrente AND 
                    tipomovimento = @tipoMovimento",
                new { idContaCorrente, tipoMovimento });
        }
    }
}
