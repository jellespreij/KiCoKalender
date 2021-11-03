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
        IEnumerable<Asset> FindAssetsByFolderId(Guid folderId);
        Task<Asset> AddAsset(FilePart file, Guid folderId);
        Task<Asset> DeleteAsset(Guid id);
        Asset UpdateAsset(Asset asset, Guid id);
    }
}
