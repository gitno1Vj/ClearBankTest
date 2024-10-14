using ClearBank.DeveloperTest.Data;

namespace ClearBank.DeveloperTest.Helpers
{
    public class DataStoreHelper : IDataStoreHelper
    {
        public IDataStore GetDataStore(string dataStoreType)
        {
            if (dataStoreType == "Backup")
            {
                return new BackupAccountDataStore();
            }
            return new AccountDataStore();
        }
    }
}
