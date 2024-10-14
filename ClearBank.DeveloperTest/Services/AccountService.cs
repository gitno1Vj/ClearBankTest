using ClearBank.DeveloperTest.Helpers;
using ClearBank.DeveloperTest.Interfaces;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDataStoreHelper _datastoreHelper;
        private readonly IConfigHelper _configHelper;

        public AccountService(IDataStoreHelper dataStoreHelper, IConfigHelper configHelper)
        {
            _datastoreHelper = dataStoreHelper;
            _configHelper = configHelper;
        }

        public Account GetAccount(string accountNumber)
        {
            return GetDataStore().GetAccount(accountNumber);
        }

        public void UpdateAccount(Account account)
        {
            GetDataStore().UpdateAccount(account);
        }

        public IDataStore GetDataStore()
        {
            return _datastoreHelper.GetDataStore(_configHelper.DataStoreType);
        }
    }
}
