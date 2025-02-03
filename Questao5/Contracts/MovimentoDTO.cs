using System.ComponentModel.DataAnnotations;

namespace Questao5.Contracts
{
    public class MovimentoDTO
    {
        public Guid? IdIdempotencia { get; set; }

        [Required(ErrorMessage = "O id da conta corrente é obrigatório.")]
        public Guid IdContaCorrente { get; set; }

        [Required(ErrorMessage = "O tipo de movimentação é obrigatório.")]
        public string TipoMovimento { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser um número positivo.")]
        public double Valor { get; set; }
    }
}
