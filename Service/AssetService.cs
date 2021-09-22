using KiCoKalender.Interfaces;
using KiCoKalender.Models;
using KiCoKalender.Repository;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCoKalender.Service
{
    class AssetService : IAssetService
    {
        private AssetRepository AssetRepository { get; }

        public AssetService(AssetRepository AssetRepository)
        {
            this.AssetRepository = AssetRepository;
        }

        public async Task<List<Asset>> FindAssetByUserId(long? userId)
        {
            return await AssetRepository.FindAssetByUserId(userId);
        }

        public void AddAsset(Asset asset)
        {
            AssetRepository.AddAsset(asset);
        }

        public void DeleteAsset(Asset asset)
        {
            AssetRepository.AddAsset(asset);
        }

        public void UpdateAsset(Asset asset)
        {
            AssetRepository.UpdateAsset(asset);
        }

        public async Task<List<Asset>> FindByAssetsEnum(long? userId, AssetsEnum assetsEnum)
        {
            return await AssetRepository.FindByAssetsEnum(userId, assetsEnum);
        }
    }
}
