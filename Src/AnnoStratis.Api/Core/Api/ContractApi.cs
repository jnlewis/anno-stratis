using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using Anno.Api.Core.Utility;
using Newtonsoft.Json;

namespace Anno.Api.Core.Api
{
    public class ContractApi
    {
        private static readonly HttpClient client = new HttpClient();

        private string networkUrl = null;
        private string contractAddress = null;
        private string ownerAddress = null;
        private string walletName = null;
        private string walletPassword = null;

        public ContractApi()
        {
            this.networkUrl = Config.NetworkUrl;
            this.contractAddress = Config.ContractAddress;
            this.ownerAddress = Config.OwnerAddress;
            this.walletName = Config.WalletName;
            this.walletPassword = Config.WalletPassword;
        }

        #region Helpers

        private bool InvokeContract(string methodName, dynamic data)
        {
            if (Config.CommitToBlockchain)
            {
                try
                {
                    // Create method endpoint url
                    string url = $"{this.networkUrl}/api/contract/{this.contractAddress}/method/{methodName}";

                    //Audit log
                    Log.Error($"InvokeContract: {url}");

                    using (var client = new System.Net.WebClient())
                    {
                        client.Headers.Add("Cache-Control", "no-cache");
                        client.Headers.Add("Accept", "*/*");
                        client.Headers.Add("User-Agent", "Anno/1.0");
                        client.Headers.Add("Content-Type", "application/json");
                        client.Headers.Add("GasPrice", "100");
                        client.Headers.Add("GasLimit", "100000");
                        client.Headers.Add("Amount", "0");
                        client.Headers.Add("FeeAmount", "0.01");
                        client.Headers.Add("WalletName", this.walletName);
                        client.Headers.Add("WalletPassword", this.walletPassword);
                        client.Headers.Add("Sender", this.ownerAddress);

                        string jsonData = JsonConvert.SerializeObject(data);
                        string responseInString = client.UploadString(url, "POST", jsonData);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error($"InvokeContract: Failed on method {methodName}. {ex.Message}");
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        #endregion

        public void TransferTo(string toAddress, long amount)
        {
            dynamic data = new ExpandoObject();
            data.to = toAddress;
            data.amount = amount;

            this.InvokeContract("TransferTo", data);
        }

        public void TransferFrom(string fromAddress, string toAddress, long amount)
        {
            dynamic data = new ExpandoObject();
            data.from = fromAddress;
            data.to = toAddress;
            data.amount = amount;

            this.InvokeContract("TransferFrom", data);
        }

        public void CreateHost(string hostAddress, string hostName)
        {
            dynamic data = new ExpandoObject();
            data.hostAddress = hostAddress;
            data.hostName = hostName;

            this.InvokeContract("CreateHost", data);
        }
        
        public void CreateCustomer(string hostAddress, string customerAddress, string refId)
        {
            dynamic data = new ExpandoObject();
            data.customerAddress = customerAddress;
            data.hostAddress = hostAddress;
            data.customerRefId = refId;

            this.InvokeContract("CreateCustomer", data);
        }

        public void CreateEvent(string eventUniqueId, string hostAddress, string refId, string title, DateTime startDate, string status)
        {
            dynamic data = new ExpandoObject();
            data.eventUniqueId = eventUniqueId;
            data.hostAddress = hostAddress;
            data.eventRefId = refId;
            data.title = refId;
            data.startDateTime = Convert.ToUInt64(DateTimeUtility.ToUnixTime(startDate));
            data.status = status;

            this.InvokeContract("CreateEvent", data);
        }

        public void CreateEventTier(string eventTierUniqueId, string hostAddress, string eventUniqueId, string refId, string tierName, int totalTickets, int availableTickets, long price)
        {
            dynamic data = new ExpandoObject();
            data.eventTierUniqueId = eventTierUniqueId;
            data.hostAddress = hostAddress;
            data.eventUniqueId = eventUniqueId;
            data.eventTierRefId = refId;
            data.tierName = tierName;
            data.totalTickets = totalTickets;
            data.availableTickets = availableTickets;
            data.price = price;

            this.InvokeContract("CreateEventTier", data);
        }

        public void BookEvent(string customerAddress, string eventUniqueId, string eventTierUniqueId, List<string> ticketNumbers)
        {
            dynamic data = new ExpandoObject();
            data.customerAddress = customerAddress;
            data.eventUniqueId = eventUniqueId;
            data.eventTierUniqueId = eventTierUniqueId;
            data.ticketNumbersString = string.Join(";", ticketNumbers);

            this.InvokeContract("BookEvent", data);
        }

        public void RedeemTicket(string ticketNumber)
        {
            dynamic data = new ExpandoObject();
            data.ticketNumber = ticketNumber;

            this.InvokeContract("RedeemTicket", data);
        }

        public void CancelTicket(string ticketNumber)
        {
            dynamic data = new ExpandoObject();
            data.ticketNumber = ticketNumber;

            this.InvokeContract("CancelTicket", data);
        }
        
        public void ClaimEarnings(string eventUniqueId)
        {
            dynamic data = new ExpandoObject();
            data.eventUniqueId = eventUniqueId;

            this.InvokeContract("ClaimEarnings", data);
        }
    }
}