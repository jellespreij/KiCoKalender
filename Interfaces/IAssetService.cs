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
        Task<Asset> GetAssetById(int assetId);
    }
}
