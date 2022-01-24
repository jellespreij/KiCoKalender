using System.Collections.Generic;
using System.Linq;

namespace Models.Helpers
{
    public static class AssetDTOHelper
    {
        public static IEnumerable<AssetDTO> ToDTO(IEnumerable<Asset> assets)
        {
            List<AssetDTO> assetsDTOs = new();

            foreach (Asset asset in assets)
            {
                assetsDTOs.Add(new AssetDTO
                {
                    Id = asset.Id,
                    Name = asset.Name,
                    Description = asset.Description,
                    CreatedDate = asset.CreatedDate,
                    Url = asset.Url,
                });
            }

            return assetsDTOs.AsEnumerable();
        }
    }
}