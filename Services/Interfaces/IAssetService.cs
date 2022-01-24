using HttpMultipartParser;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAssetService
    {
        Asset FindAssetByAssetId(Guid assetId);
        IEnumerable<Asset> FindAssetsByFolderId(Guid folderId);
        IEnumerable<AssetDTO> FindAssetsDTOByFolderId(Guid folderId);
        Task<Asset> AddAsset(FilePart file, Guid folderId);
        Task<Asset> DeleteAsset(Guid id);
        Asset UpdateAsset(AssetUpdateDTO assetUpdate, Guid id);
    }
}
