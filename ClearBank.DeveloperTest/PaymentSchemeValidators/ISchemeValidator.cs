using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.PaymentSchemeValidators
{
    public interface ISchemeValidator
    {
        MakePaymentResult ValidateScheme(Account account, decimal requestAmount = 0);
    }
}
