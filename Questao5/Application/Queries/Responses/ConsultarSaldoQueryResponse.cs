using Questao5.Application.Helpers;

namespace Questao5.Application.Queries.Responses
{
    public class ConsultarSaldoQueryResponse : ResponseHelperAPI
    {
        public int NumeroContaCorrente { get; set; }
        public string TitularContaCorrente { get; set; }
        public DateTime DataHoraConsulta { get; set; }
        public decimal Saldo { get; set; }
    }
}
