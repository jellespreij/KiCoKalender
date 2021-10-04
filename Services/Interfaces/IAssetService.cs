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
        IEnumerable<Asset> FindAssetByUserIdAndFolder(long userId, Folder folder);
        IEnumerable<Asset> FindAssetByFamilyIdAndFolder(long familyId, Folder folder);
        void AddAsset(Asset asset);
        void DeleteAsset(Asset asset);
        void UpdateAsset(Asset asset);
    }
}
