using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BlobService
    {
        private const string CONTAINERPREFIX = "kicokalender";

        public async Task<CloudBlobContainer> GetBlobContainer()
        {
            CloudStorageAccount storageAccount = StorageAccountSettings.CreateStorageAccountFromConnectionString();

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(CONTAINERPREFIX);

            try
            {
                // Retry policy - optional
                BlobRequestOptions optionsWithRetryPolicy = new BlobRequestOptions() { RetryPolicy = new Microsoft.Azure.Storage.RetryPolicies.LinearRetry(TimeSpan.FromSeconds(20), 4) };

                await container.CreateIfNotExistsAsync(optionsWithRetryPolicy, null);
                await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            catch (StorageException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return container;
        }
    }
}
