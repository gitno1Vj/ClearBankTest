using ClearBank.DeveloperTest.PaymentSchemeValidators;
using ClearBank.DeveloperTest.Types;
using System.Collections.Generic;

namespace ClearBank.DeveloperTest.Helpers
{
    public class PaymentsValidatorHelper : IPaymentsValidatorHelper
    {
        public Dictionary<PaymentScheme, ISchemeValidator> Validators { get; set; }

        public PaymentsValidatorHelper()
        {
            Validators = new Dictionary<PaymentScheme, ISchemeValidator>
            {
                {PaymentScheme.Bacs, new BacsSchemeValidator()},
                {PaymentScheme.FasterPayments, new FasterPaymentsSchemeValidator()},
                {PaymentScheme.Chaps, new ChapsSchemeValidator()}
            };
        }

        public MakePaymentResult Validate(PaymentScheme scheme, Account account, decimal amount = 0)
        {
            return Validators[scheme].ValidateScheme(account, amount);
        }
    }
}
