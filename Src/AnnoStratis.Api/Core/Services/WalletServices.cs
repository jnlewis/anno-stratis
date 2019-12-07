using Anno.Models.Entities;
using Anno.Api.Core.Const;
using System;
using System.Linq;
using Anno.Api.Core.Api;
using Anno.Api.Models;
using System.IO;
using Anno.Api.Core.Utility;

namespace Anno.Api.Core.Services
{
    public class WalletServices
    {
        public WalletServices()
        {
        }

        public string GenerateWalletAddress()
        {
            //TODO: temporary method. To be replaced with below GenerateWalletFile(string password)
            //Wallet file generated will be kept by user, password to be provided by user as well.
            object locker = new object();
            lock(locker)
            {
                var files = Directory.GetFiles(Config.TempAddressesFolder);
                if (files.Length > 0)
                {
                    string address = File.ReadAllText(files[0]);
                    File.Delete(files[0]);
                    return address;
                }
                else
                {
                    return HashUtility.GenerateHash(34);
                }
            }
        }

        public GeneratedWallet GenerateWalletFile(string password)
        {
            GeneratedWallet wallet = new GeneratedWallet();

            BlockchainApi blockchainApi = new BlockchainApi();

            //Generate mnemonic
            string mnemonic = blockchainApi.GenerateMnemonic();

            //Create wallet
            string walletName = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            blockchainApi.CreateWallet(mnemonic, password, password, walletName);

            //Read wallet address
            string walletAddress = ReadWalletAddressFromFile(walletName);

            //Prepare output
            wallet.WalletName = walletName;
            wallet.WalletAddress = walletAddress;
            wallet.Mnemonic = mnemonic;

            return wallet;
        }

        private string ReadWalletAddressFromFile(string walletName)
        {
            //string fileContent = System.IO.File.ReadAllText(Path.Combine(Config.WalletFolder, walletName + ".wallet.json"));
            return Guid.NewGuid().ToString().Substring(0, 8);  //TODO:
        }

        public void SaveWallet(long ownerId, string ownerType, string address)
        {
            using (var context = new AnnoDBContext())
            {
                context.Wallet.Add(new Wallet()
                {
                    owner_id = ownerId,
                    owner_type = ownerType,
                    address = address,
                    balance = 0,
                    record_status = RecordStatuses.Live,
                    created_date = DateTime.UtcNow
                });
                context.SaveChanges();
            }
        }
        
        public void Transfer(string fromAddress, string toAddress, decimal amount, long? bookingId, string description)
        {
            using (var context = new AnnoDBContext())
            {
                //Insert transaction to database
                context.Transaction.Add(new Transaction()
                {
                    transaction_datetime = DateTime.UtcNow,
                    address_from = fromAddress,
                    address_to = toAddress,
                    amount = amount,
                    booking_id = bookingId,
                    description = description,
                    created_date = DateTime.UtcNow
                });

                //Update sender wallet
                var fromWallet = context.Wallet.Where(x => x.address == fromAddress).FirstOrDefault();
                if(fromWallet != null)
                {
                    fromWallet.balance = fromWallet.balance - amount;
                }

                //Update recipient wallet
                var toWallet = context.Wallet.Where(x => x.address == toAddress).FirstOrDefault();
                if (toWallet != null)
                {
                    toWallet.balance = toWallet.balance + amount;
                }

                context.SaveChanges();
            }
        }
    }
}