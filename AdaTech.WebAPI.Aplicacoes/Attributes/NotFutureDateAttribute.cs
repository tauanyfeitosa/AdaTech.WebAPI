using System.ComponentModel.DataAnnotations;

namespace AdaTech.WebAPI.Aplicacoes.Attributes
{
    public class NotFutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date = Convert.ToDateTime(value);
            if (date.Date > DateTime.Now.Date)
            {
                ErrorMessage = "A data da venda não pode ser no futuro.";
                return false;
            }
            return true;
        }
    }
}
