using KiCoKalender.Controllers;
using KiCoKalender.Models;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCoKalender.Repository
{
    class AssetRepository
    {
        ILogger Logger { get; }

        public AssetRepository(ILogger<AssetsHttpTrigger> Logger)
        {
            this.Logger = Logger;
        }

        public async Task<List<Asset>> FindAssetByUserId(long? userId)
        {
            List<Asset> assets = new()
            {
                new Asset() { assetId = 33, assetDescription = "description", assetCreatedDate = DateTime.Now }
            };

            Logger.LogInformation("Found assets by id: ", userId);

            return assets;
        }

        public void AddAsset(Asset asset) 
        {
            Logger.LogInformation("Added asset");
        }

        public void DeleteAsset(Asset asset)
        {
            Logger.LogInformation("Deleted asset");
        }

        public void UpdateAsset(Asset asset) 
        {
            Logger.LogInformation("Updated asset");
        }

        public async Task<List<Asset>> FindByAssetsEnum(long? userId, AssetsEnum assetsEnum) 
        {
            List<Asset> assets = new()
            {
                new Asset() { assetId = 33, assetDescription = "description", assetCreatedDate = DateTime.Now, assetsEnum = assetsEnum }
            };

            Logger.LogInformation("Found assets by assetsEnum");

            return assets;
        }
    }
}
