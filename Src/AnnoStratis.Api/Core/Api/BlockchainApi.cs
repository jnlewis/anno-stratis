using System;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Anno.Api.Core.Api
{
    public class BlockchainApi
    {
        private static readonly HttpClient client = new HttpClient();

        private string networkUrl = null;

        public BlockchainApi()
        {
            this.networkUrl = Config.NetworkUrl;
        }

        public void CreateWallet(string mnemonic, string password, string passphrase, string name)
        {
            string url = $"{this.networkUrl}/api/Wallet/create";

            dynamic requestBody = new ExpandoObject();
            requestBody.mnemonic = mnemonic;
            requestBody.password = password;
            requestBody.passphrase = passphrase;
            requestBody.name = name;

            string jsonData = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, content).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;
        }

        public string GenerateMnemonic()
        {
            string url = $"{this.networkUrl}/api/Wallet/mnemonic";

            var response = client.GetAsync(url).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            return responseString;
        }
    }
}