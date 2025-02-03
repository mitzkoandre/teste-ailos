using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.Repositories;
using System.Data;

namespace Questao5.Infrastructure.Percistence.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly IDbConnection _dbConnection;

        public ContaCorrenteRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ContaCorrente> GetByIdAsync(Guid id)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<ContaCorrente>(
                @"SELECT 
                    idcontacorrente, numero, nome, ativo 
                  FROM 
                    contacorrente
                  WHERE 
                    idcontacorrente = @Id AND ativo = 1", new { Id = id });
        }
    }
}
