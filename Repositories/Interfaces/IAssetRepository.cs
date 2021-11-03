using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAssetRepository : IBaseRepository<Asset>
    {
        public Task<Asset> AddAssetToFolder(Folder folder, Guid id);
    }
}
