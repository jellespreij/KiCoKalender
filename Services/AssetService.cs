using HttpMultipartParser;
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
        private IFolderRepository _folderRepository;
        private BlobService _blobService;

        public AssetService(IAssetRepository assetRepository, IFolderRepository folderRepository, BlobService blobService)
        {
            _assetRepository = assetRepository;
            _folderRepository = folderRepository;
            _blobService = blobService;
        }

        public async Task<Asset> AddAsset(FilePart file, Guid folderId)
        {
            Folder folder = _folderRepository.GetSingle(folderId);

            CloudBlobContainer container = await _blobService.GetBlobContainer();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(folder.Name + "/" + file.FileName);

            try
            {
                using (var filestream = file.Data)
                {
                    blockBlob.UploadFromStream(filestream);
                }
            }
            catch (StorageException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Asset asset = new()
            {
                Name = file.FileName,
                Url = blockBlob.Uri.ToString(),
                PartitionKey = folder.PartitionKey
            };

            Asset addedAsset = await _assetRepository.Add(asset);
            return _assetRepository.AddAssetToFolder(folder, addedAsset.Id).Result;
        }

        public async Task<Asset> DeleteAsset(Guid id)
        {
            Asset asset = _assetRepository.GetSingle(id);
            CloudBlobContainer container = await _blobService.GetBlobContainer();

            var blob = container.GetBlobReference(asset.Folder.Name + "/" + asset.Name);
            await blob.DeleteIfExistsAsync();

            _folderRepository.RemoveAssetFromFolder(asset);
            return _assetRepository.Delete(id).Result;
        }

        public IEnumerable<Asset> FindAssetsByFolderId(Guid folderId)
        {
            return _assetRepository.FindBy(e => e.FolderId == folderId);
        }

        public Asset UpdateAsset(Asset asset, Guid id)
        {
            return _assetRepository.Update(asset, id).Result;
        }
    }
}
