using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.PaymentSchemeValidators
{
    public class FasterPaymentsSchemeValidator : ISchemeValidator
    {
        public MakePaymentResult ValidateScheme(Account account, decimal requestAmount = 0)
        {
            var paymentResult = new MakePaymentResult();
            if (account != null
                && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments)
                && requestAmount > 0
                && (account.Balance > requestAmount))
            {
                paymentResult.Success = true;
            }
            return paymentResult;
        }
    }
}
