# Simple Azure Project

This project helps me to undersand how to create a console application with .NET 8 and Azure Files.

It realizes some actions like:
+ Connect to a file share using a conncetion string
+ Create a file share if it doesn't exist
+ Create a dummy file with a varying size
+ Set the maximum size of a file share
+ Upload file previously created to the file share

For security purposes, I use a nuget package named __Microsoft.Extensions.Configuration.UserSecrets__ to keep my storage account credentials secret.

I provide some useful links to help anybody to have a solid understanding of how to create and interact with Azure File Share programmatically.