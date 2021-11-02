using Microsoft.Azure.Storage;
using System;

namespace Services
{
    public static class StorageAccountSettings
    {
        public static CloudStorageAccount CreateStorageAccountFromConnectionString()
        {
            string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            return storageAccount;
        }
    }
}