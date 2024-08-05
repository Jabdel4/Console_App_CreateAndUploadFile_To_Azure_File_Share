using Azure.Storage;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace AzureFileShareIO
{
    internal class Program
    {
        class DummyFile
        {
            //private async Task CredentialsAsync(string shareName)
            //{



            //}

            public async Task DummyFileAsync(string shareName, string fileName) {
            {

                    // Accessed the secrets
                    var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
                    var connectionString = config.GetSection("StorageCredentials")["StorageConnectionString"];

                    //Log into th File Share
                    ShareClient share = new(connectionString, shareName);
                    Console.WriteLine("Connection to Azure Storage succeeded...");


 

        }
    }
}
