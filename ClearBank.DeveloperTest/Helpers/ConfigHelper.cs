using System.Configuration;

namespace ClearBank.DeveloperTest.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public string DataStoreType => ConfigurationManager.AppSettings["DataStoreType"];
    }
}
