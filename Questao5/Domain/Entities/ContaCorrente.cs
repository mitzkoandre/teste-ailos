namespace Questao5.Domain.Entities
{
    public class ContaCorrente
    {
        public ContaCorrente() { }

        public ContaCorrente(Guid idContaCorrente, int numero, string nome, bool ativo)
        {
            IdContaCorrente = idContaCorrente;
            Numero = numero;
            Nome = nome;
            Ativo = ativo;
        }

        public Guid IdContaCorrente { get; private set; }
        public int Numero { get; private set; }
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }
    }
}
