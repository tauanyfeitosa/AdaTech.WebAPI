using System.ComponentModel.DataAnnotations;

namespace AdaTech.WebAPI.SistemaVendas.Utilities.Attributes
{
    public class NotFutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date = Convert.ToDateTime(value);
            if (date > DateTime.Now)
            {
                ErrorMessage = "A data da venda não pode ser no futuro.";
                return false;
            }
            return true;
        }
    }
}
