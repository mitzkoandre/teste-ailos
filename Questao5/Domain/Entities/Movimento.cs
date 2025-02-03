namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public Movimento(Guid idMovimento, Guid idContaCorrente, string dataMovimento, string tipoMovimento, double valor)
        {
            IdMovimento = idMovimento;
            IdContaCorrente = idContaCorrente;
            DataMovimento = dataMovimento;
            TipoMovimento = tipoMovimento;
            Valor = valor;
        }

        public Guid IdMovimento { get; private set; }
        public Guid IdContaCorrente { get; private set; }
        public string DataMovimento { get; private set; }
        public string TipoMovimento { get; private set; }
        public double Valor { get; private set; }
    }
}
