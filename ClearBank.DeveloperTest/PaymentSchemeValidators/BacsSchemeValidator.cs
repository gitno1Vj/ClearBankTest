using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.PaymentSchemeValidators
{
    public class BacsSchemeValidator : ISchemeValidator
    {
        public MakePaymentResult ValidateScheme(Account account, decimal requestAmount = 0)
        {
            var paymentResult = new MakePaymentResult();

            if (account != null && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
            {
                paymentResult.Success = true;
            }

            return paymentResult;
        }
    }
}
