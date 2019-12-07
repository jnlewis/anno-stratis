using System;
using System.Configuration;

namespace Anno.Api.Core
{
    public static class Config
    {

        #region ConnectionStrings

        public static string ConnectionString_Anno { get { return GetConnectionString("Anno"); } }

        #endregion

        #region AppSettings

        //General Settings

        public static string ApplicationName { get { return GetAppSettings("ApplicationName"); } }
        public static bool LogRequests { get { return Convert.ToBoolean(GetAppSettings("LogRequests")); } }

        //Blockchain Settings

        public static bool CommitToBlockchain { get { return Convert.ToBoolean(GetAppSettings("CommitToBlockchain")); } }

        public static string NetworkUrl { get { return GetAppSettings("NetworkUrl"); } }
        public static string ContractAddress { get { return GetAppSettings("ContractAddress"); } }
        public static string OwnerAddress { get { return GetAppSettings("OwnerAddress"); } }
        public static string WalletName { get { return GetAppSettings("WalletName"); } }
        public static string WalletPassword { get { return GetAppSettings("WalletPassword"); } }
        public static string WalletFolder { get { return GetAppSettings("WalletFolder"); } }
        public static string TempAddressesFolder { get { return GetAppSettings("TempAddressesFolder"); } }
        
        #endregion

        #region Private Methods

        private static string GetConnectionString(string name)
        {
            if (ConfigurationManager.ConnectionStrings[name] == null)
                throw new Exception("ConnectionStrings not found: " + name);

            return ConfigurationManager.ConnectionStrings[name].ToString();
        }

        private static string GetAppSettings(string key)
        {
            if (ConfigurationManager.AppSettings[key] == null)
                throw new Exception("AppSettings not found: " + key);

            return ConfigurationManager.AppSettings[key].ToString();
        }

        #endregion

    }
}