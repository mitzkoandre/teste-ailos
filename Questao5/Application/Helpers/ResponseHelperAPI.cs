using FluentValidation.Results;

namespace Questao5.Application.Helpers
{
    public class ResponseHelperAPI
    {
        public ResponseHelperAPI()
        {
            Validations = new ValidationResult();
        }

        private ValidationResult Validations { get; set; }
        public bool HasErrors => !Validations.IsValid;

        public void AddError(string mensagem)
        {
            Validations.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }

        public void AddErrors(List<string> mensagem)
        {
            foreach (var item in mensagem)
                Validations.Errors.Add(new ValidationFailure(string.Empty, item));
        }

        private string[] Errors => Validations.Errors.Select(error => error.ErrorMessage).ToArray();
        public Dictionary<string, string[]> GetErrors => new Dictionary<string, string[]> { { "Messages", Errors } };
    }
}
