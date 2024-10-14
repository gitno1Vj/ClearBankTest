using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Interfaces
{
    public interface ICalculatorService
    {
        void DeductAmountFromAccount(Account account, decimal amount);
    }
}
