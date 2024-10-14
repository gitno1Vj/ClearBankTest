using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Helpers
{
    public interface IPaymentsValidatorHelper
    {
        MakePaymentResult Validate(PaymentScheme scheme, Account account, decimal amount = 0);
    }
}
