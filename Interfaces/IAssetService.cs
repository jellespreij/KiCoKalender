using KiCoKalender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCoKalender.Interfaces
{
    interface IAssetService
    {
        Task<List<Asset>> FindAssetByUserId(long? userId);
        void AddAsset(Asset asset);
        void DeleteAsset(Asset asset);
        void UpdateAsset(Asset asset);
        Task<List<Asset>> FindByAssetsEnum(long? userId, AssetsEnum assetsEnum);
    }
}
