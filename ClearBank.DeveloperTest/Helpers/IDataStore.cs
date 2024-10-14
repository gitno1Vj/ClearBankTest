using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Helpers
{
    public interface IDataStore
    {
        Account GetAccount(string accountNumber);
        void UpdateAccount(Account account);
    }
}
