using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAssetService
    {
        IEnumerable<Asset> FindAssetsByFolderId(Guid folderId);
        Task<Asset> AddAsset(Asset asset, Guid folderId, string localUrl);
        Task<Asset> DeleteAsset(Guid id);
        Asset UpdateAsset(Asset asset, Guid id);
    }
}
