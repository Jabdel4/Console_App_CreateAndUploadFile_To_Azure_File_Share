﻿using Azure.Storage;
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
            public class Tasks
            {
                public async Task DummyFileAsync(string shareName, string fileName)
                {

                    // Accessed the secrets
                    var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
                    var connectionString = config.GetSection("StorageCredentials")["StorageConnectionString"];

                    //Log into th File Share
                    ShareClient dummyShare = new(connectionString, shareName);
                    Console.WriteLine("Connection to Azure File Share succeeded...");

                    // Create a file of 50MB or more...
                    // TODO check if the file share already exists. If not, create it. If yes, just create and upload the file to the existing FS
                    await dummyShare.CreateIfNotExistsAsync();
                    Console.WriteLine($"File share {shareName} does not exist...");
                    Console.WriteLine($"So, we created it...");

                    MemoryStream stream = new MemoryStream();
                    stream.Seek(800 * 1024 * 1024, SeekOrigin.Begin);
                    stream.WriteByte(0);

                    // Client to interact with the file in the Azure File Share
                    ShareFileClient dummyFileClient = new ShareFileClient(connectionString, shareName, fileName);

                    // TODO check if the file already exists. If yes, stop the process/give another name. If not, create the file.

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
                    Console.WriteLine($"We have created {fileName} for you...");

                    // Upload the stream to the file in the Azure File Share, using the specified upload options
                    if (stream.Position < stream.Length)
                    {
                        Console.WriteLine($"Upload of {fileName} in progress...\n");
                        await dummyFileClient.UploadAsync(stream, shareFileUploadOptions);
                        Console.WriteLine("Upload successful...");
                    }
                }

            }


            static async Task Main(string[] args)
            {
                Console.WriteLine("Welcome to the DummyFileAsync method");
                Console.WriteLine("Enter the share name");
                var dummyShareName = Console.ReadLine();

                Console.WriteLine("Enter the file name");
                var dummyFile = Console.ReadLine();

                // Call the DummyFileAsync task
                Tasks dummyTask = new Tasks();
                await dummyTask.DummyFileAsync($"{dummyShareName}", $"{dummyFile}");

                Console.WriteLine("Create and Upload operations done.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();

            }
        }
    }
}

