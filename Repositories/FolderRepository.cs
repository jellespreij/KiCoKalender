using Context;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class FolderRepository : BaseRepository<Folder>, IFolderRepository
    {
        public FolderRepository(CosmosDBContext cosmosDBContext) : base(cosmosDBContext)
        {

        }

        public async void RemoveAssetFromFolder(Asset asset)
        {
            await _context.Database.EnsureCreatedAsync();

            Folder folder = _context.Set<Folder>().Where(entity => entity.Id == asset.FolderId).FirstOrDefault();

            folder.Assets.Remove(asset);

            Commit();
        }
    }
}
