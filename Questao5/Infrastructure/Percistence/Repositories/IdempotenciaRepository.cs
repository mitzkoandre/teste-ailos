using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.Repositories;
using System.Data;

namespace Questao5.Infrastructure.Percistence.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly IDbConnection _dbConnection;

        public IdempotenciaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Idempotencia> GetByIdAsync(Guid idIdempotencia)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<Idempotencia>(
                @"SELECT    
                    requisicao, resultado  
                  FROM 
                    idempotencia 
                  WHERE chave_idempotencia = @idIdempotencia",
            new { idIdempotencia = idIdempotencia });
        }

        public async Task InsertAsync(Idempotencia idempotencia)
        {
            await _dbConnection.ExecuteAsync(
                @"INSERT INTO 
                    idempotencia (chave_idempotencia, requisicao, resultado) 
                VALUES 
                    (@IdIdempotencia, @Requisicao, @Resultado)", idempotencia);
        }
    }
}
