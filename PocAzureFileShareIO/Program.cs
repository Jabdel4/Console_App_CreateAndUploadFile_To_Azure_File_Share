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

            public async Task DummyFileAsync(string shareName, string fileName)
            {

                    // Accessed the secrets
                    var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
                    var connectionString = config.GetSection("StorageCredentials")["StorageConnectionString"];

                    //Log into th File Share
                    ShareClient dummyShare = new(connectionString, shareName);
                    Console.WriteLine("Connection to Azure Storage succeeded...");

                    // Create a file of 50MB or more...
                    // Firstly check if the file we want to upload already exists
                    await dummyShare.CreateIfNotExistsAsync();

                    MemoryStream stream = new MemoryStream();
                    stream.Seek(500 * 1024 * 1024, SeekOrigin.Begin);
                    stream.WriteByte(0);

                    // Client to interact with the file in the Azure File Share
                    ShareFileClient dummyFileClient = new ShareFileClient(connectionString, shareName, fileName);

                    // Specify the upload options, including transfer options
                    ShareFileUploadOptions shareFileUploadOptions = new ShareFileUploadOptions
                    {
                        TransferOptions = new StorageTransferOptions
                        {
                            // Specify the initial and maximum transfer size for the upload
                            InitialTransferSize = 1024 * 1024 * 4,
                            MaximumTransferSize = 1024 * 1024 * 4
                        }
                    };

                    // Create the file on the Azure FIle Share with the specified size
                    await dummyFileClient.CreateAsync(stream.Length);

                    // Upload the stream to the file in the Azure File Share, using the specified upload options
                    if (stream.Position < stream.Length)
                    {
                        await dummyFileClient.UploadAsync(stream, shareFileUploadOptions);
                        Console.WriteLine("Upload successful...");
                    }              
            }
        }
    }
}

