namespace Questao5.Domain.Entities
{
    public class Idempotencia
    {
        public Idempotencia() { }

        public Idempotencia(Guid chaveIdempotencia, string requisicao, string resultado)
        {
            IdIdempotencia = chaveIdempotencia;
            Requisicao = requisicao;
            Resultado = resultado;
        }

        public Guid IdIdempotencia { get; private set; }
        public string Requisicao { get; private set; }
        public string Resultado { get; private set; }
    }
}
