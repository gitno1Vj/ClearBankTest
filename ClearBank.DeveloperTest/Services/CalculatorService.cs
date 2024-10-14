using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Services
{
    public class CalculatorService : ICalculatorService
    {
        public void DeductAmountFromAccount(Account account, decimal amount)
        {
            account.Balance -= amount;
        }
    }
}
