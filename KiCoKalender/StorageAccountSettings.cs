using Microsoft.Azure.Storage;

namespace ConsoleAppBlobStorage.Infrastructure
{
    public static class StorageAccountSettings
    {
        public static CloudStorageAccount CreateStorageAccountFromConnectionString()
        {
            CloudStorageAccount storageAccount;
            storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");

            return storageAccount;
        }
    }
}
