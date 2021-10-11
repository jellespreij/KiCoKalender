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
    public class AssetService : IAssetService
    {
        private IAssetRepository _assetRepository;

        private const string CONTAINERPREFIX = "powerpuffgirls";

        public AssetService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async void AddAsset(Asset asset)
        {
           // _assetRepository.Add(asset);

            CloudStorageAccount storageAccount = StorageAccountSettings.CreateStorageAccountFromConnectionString();

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(CONTAINERPREFIX);
            try
            {
                // Retry policy - optional
                BlobRequestOptions optionsWithRetryPolicy = new BlobRequestOptions() { RetryPolicy = new Microsoft.Azure.Storage.RetryPolicies.LinearRetry(TimeSpan.FromSeconds(20), 4) };

                await container.CreateIfNotExistsAsync(optionsWithRetryPolicy, null);
            }
            catch (StorageException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            // Upload a BlockBlob(.png) to the newly created container
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(asset.Url);

            // Browser now knows it as an image.
            blockBlob.Properties.ContentType = "image/jpg";

            try
            {
                await blockBlob.UploadFromFileAsync(asset.Url);
            }
            catch (StorageException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void DeleteAsset(Asset asset)
        {
            _assetRepository.Delete(asset);
        }

        public IEnumerable<Asset> FindAssetByUserIdAndFolder(long userId, Folder folder)
        {
            return _assetRepository.FindBy(e => e.UserId == userId && e.Folder == folder);
        }

        public IEnumerable<Asset> FindAssetByFamilyIdAndFolder(long familyId, Folder folder)
        {
            return _assetRepository.FindBy(e => e.FamilyId == familyId && e.Folder == folder);
        }

        public void UpdateAsset(Asset asset)
        {
            _assetRepository.Update(asset);
        }
    }
}
