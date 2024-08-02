using Azure.Storage;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.IO;
using System.Text;
using System.Xml.Linq;


const long ONE_GIGABYTE = 110737420000;

// Specify the connection string for your Azure File Storage account
string connectionString = "";

// Specify the name of the file share where you want to upload the file
string shareName = "";

// Specify the desired name for the file you want to upload
string fileName = "dummy5.txt";

ShareClient share = new ShareClient(connectionString, shareName);

await share.CreateIfNotExistsAsync();

// Create a file of 50MB or more...
MemoryStream stream = new MemoryStream();
stream.Seek(500 * 1024 * 1024, SeekOrigin.Begin);
stream.WriteByte(0);

// Create a client to interact with the file in the Azure File Storage
ShareFileClient fileClient = new ShareFileClient(connectionString, shareName, fileName);

// Set the maximum size for a file share
int increaseSizeInGiB = 5;

if (await share.ExistsAsync())
{
    // Get and display current share quota
    ShareProperties properties = await share.GetPropertiesAsync();
    Console.WriteLine($"Current share quota: {properties.QuotaInGB} GiB");

    // Get and display current usage stats for the share
    ShareStatistics stats = await share.GetStatisticsAsync();
    Console.WriteLine($"Current share usage: {stats.ShareUsageInBytes} bytes");

    // Convert current usage from bytes into GiB
    int currentGiB = (int)(stats.ShareUsageInBytes / ONE_GIGABYTE);

    // This line sets the quota to be the current 
    // usage of the share plus the increase amount
    await share.SetQuotaAsync(currentGiB + increaseSizeInGiB);

    // Get the new quota and display it
    properties = await share.GetPropertiesAsync();
    Console.WriteLine($"New share quota: {properties.QuotaInGB} GiB");
}
//Specify the upload options, including transfer options
ShareFileUploadOptions uploadOptions = new ShareFileUploadOptions
{
    TransferOptions = new StorageTransferOptions
    {
        // Specify the initial and maximum transfer size for the upload
        InitialTransferSize = 1024 * 1024 * 4,
        MaximumTransferSize = 1024 * 1024 * 4
    }
};

// Create the file on the Azure File Storage with the specified size
await fileClient.CreateAsync(stream.Length);

// Upload the stream to the file in the Azure File Storage, using the specified upload options
if (stream.Position < stream.Length)
{
    await fileClient.UploadAsync(stream, uploadOptions);
    Console.WriteLine("Upload successful...");
}