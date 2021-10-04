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

        public AssetService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public void AddAsset(Asset asset)
        {
            _assetRepository.Add(asset);
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
