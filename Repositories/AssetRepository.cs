using Models;
using Repositories.Context;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AssetRepository : BaseRepository<Asset>, IAssetRepository
    {
        public AssetRepository(CosmosDBContext cosmosDBContext) : base(cosmosDBContext)
        {

        }

        public async Task<Asset> AddAssetToFolder(Folder folder, Guid id)
        {
            await _context.Database.EnsureCreatedAsync();

            var assetToUpdate = _context.Set<Asset>().Where(asset => asset.Id == id).FirstOrDefault();
            assetToUpdate.Folder = folder;

            Commit();

            return assetToUpdate;
        }
    }
}
