using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.PaymentSchemeValidators
{
    public class ChapsSchemeValidator : ISchemeValidator
    {
        public MakePaymentResult ValidateScheme(Account account, decimal requestAmount = 0)
        {
            var paymentResult = new MakePaymentResult();

            if (account != null
                && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps)
                && account.Status == AccountStatus.Live)
            {
                paymentResult.Success = true;
            }

            return paymentResult;
        }
    }
}
